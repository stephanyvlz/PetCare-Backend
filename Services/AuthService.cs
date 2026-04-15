using System.Security.Cryptography;
using PetCare.API.Helpers;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;
    private readonly JwtHelper _jwt;

    public AuthService(IUserRepository repo, JwtHelper jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    private readonly IEmailService _emailService;

    public AuthService(IUserRepository repo, JwtHelper jwt, IEmailService emailService)
    {
        _repo = repo;
        _jwt = jwt;
        _emailService = emailService;
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
{
    if (await _repo.GetByEmailAsync(dto.email) is not null)
        throw new Exception("El correo ya está registrado");

    string? schedule = null;

    if (dto.id_role == 2) // veterinario
    {
        if (string.IsNullOrWhiteSpace(dto.schedule))
            throw new Exception("El horario es obligatorio para el veterinario");

        schedule = dto.schedule;
    }

    var user = new User
    {
        name = dto.name,
        email = dto.email,
        Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        id_role = dto.id_role,
        phone = dto.phone,
        created_at = DateTime.UtcNow,
        updated_at = DateTime.UtcNow,
        id_clinic = dto.id_clinic,
        schedule = schedule
    };

    await _repo.AddAsync(user);
    await _repo.SaveChangesAsync();

    return "Usuario registrado exitosamente";
}

public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
{
    var user = await _repo.GetByEmailAsync(dto.email)
        ?? throw new Exception("Credenciales inválidas");

    if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        throw new Exception("Credenciales inválidas");

    var token = _jwt.GenerateToken(user);

    return new LoginResponseDto(token);
}
public async Task RequestPasswordResetAsync(string email)
{
    var user = await _repo.GetByEmailAsync(email);

    // No revelar si existe o no
    if (user == null) return;

    // Generar token seguro
    var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    var resetToken = new PasswordResetToken
    {
        UserId = user.id_user,
        Token = token,
        Expiration = DateTime.UtcNow.AddMinutes(30)
    };

    await _repo.AddResetTokenAsync(resetToken);
    await _repo.SaveChangesAsync();

    var link = $"https://PetCare.com/reset-password?token={token}";

    var body = $@"
        <h2>Restablecer contraseña</h2>
        <p>Haz clic en el siguiente enlace:</p>
        <a href='{link}'>Restablecer contraseña</a>
        <p>Este enlace expira en 30 minutos.</p>
    ";

    await _emailService.SendAsync(user.email, "Recuperación de contraseña", body);
}
public async Task ResetPasswordAsync(string token, string newPassword)
{
    var resetToken = await _repo.GetResetTokenAsync(token);

    if (resetToken == null || resetToken.IsUsed || resetToken.Expiration < DateTime.UtcNow)
        throw new Exception("Token inválido o expirado");

    var user = resetToken.User;

    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
    user.updated_at = DateTime.UtcNow;

    resetToken.IsUsed = true;

    await _repo.SaveChangesAsync();
}
}