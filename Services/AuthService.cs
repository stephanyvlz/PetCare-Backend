using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
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

    private readonly IEmailService _emailService;

    private readonly ILogService _log;

    private readonly IConfiguration _config;


        public AuthService(IUserRepository repo, JwtHelper jwt, IEmailService emailService, ILogService log,  IConfiguration config)
        {
            _repo = repo;
            _jwt = jwt;
            _emailService = emailService;
            _log = log;
            _config = config;;
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
    // Solo validar captcha si viene en la petición
    if (!string.IsNullOrEmpty(dto.CaptchaToken))
    {
        var isCaptchaValid = await VerifyCaptcha(dto.CaptchaToken);

        if (!isCaptchaValid)
        {
            try
            {
                await _log.LogError($"Login bloqueado por captcha inválido ({dto.email})");
                await _repo.SaveChangesAsync();
            }
            catch { }

            throw new Exception("Captcha inválido");
        }
    }

    var user = await _repo.GetByEmailAsync(dto.email);

            if (user == null)
            {
                try
                {
                    await _log.LogError($"Login fallido: usuario no existe ({dto.email})");
                    await _repo.SaveChangesAsync();
                }
                catch { }

                throw new Exception("Credenciales inválidas");
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                try
                {
                    await _log.LogError($"Login fallido: contraseña incorrecta ({dto.email})", user.id_user.ToString());
                    await _repo.SaveChangesAsync();
                }
                catch { }

                throw new Exception("Credenciales inválidas");
            }

            var token = _jwt.GenerateToken(user);

            await _log.LogInfo($"Login exitoso: {user.email}", user.id_user.ToString());
            await _repo.SaveChangesAsync();

            return new LoginResponseDto(token);
        }

        private async Task<bool> VerifyCaptcha(string token)
            {
                var client = new HttpClient();
                var secret = _config["Recaptcha:SecretKey"];

                var response = await client.PostAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}",
                    null
                );

                var json = await response.Content.ReadAsStringAsync();

               var result = JsonSerializer.Deserialize<JsonElement>(json);

                return result.GetProperty("success").GetBoolean();
            }
        

    public async Task RequestPasswordResetAsync(string email)
    {
        try
        {
            await _log.LogInfo($"Solicitud de recuperación de contraseña: {email}");
            await _repo.SaveChangesAsync();

            var user = await _repo.GetByEmailAsync(email);

            if (user == null)
            {
                await _log.LogInfo($"Password reset solicitado para email no existente: {email}");
                await _repo.SaveChangesAsync();
                return;
            }

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var resetToken = new PasswordResetToken
            {
                UserId = user.id_user,
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(30)
            };

            await _repo.AddResetTokenAsync(resetToken);
            await _repo.SaveChangesAsync();

            var link = $"http://localhost:4200/reset-password?token={Uri.EscapeDataString(token)}";

            await _emailService.SendPasswordResetAsync(user.email, user.name, link);

            await _log.LogInfo(
                $"Token de recuperación generado y enviado: {user.email}",
                user.id_user.ToString()
            );

            await _repo.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _log.LogError($"Error en RequestPasswordReset: {ex.Message}");
            await _repo.SaveChangesAsync();
            throw;
        }
    }

public async Task ResetPasswordAsync(string token, string newPassword)
{
    try
    {
        var resetToken = await _repo.GetResetTokenAsync(token);

        if (resetToken == null || resetToken.IsUsed || resetToken.Expiration < DateTime.UtcNow)
        {
            await _log.LogError("Intento de reset con token inválido o expirado");
            await _repo.SaveChangesAsync();

            throw new Exception("Token inválido o expirado");
        }

        var user = resetToken.User;

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.updated_at = DateTime.UtcNow;

        resetToken.IsUsed = true;

        await _log.LogInfo("Contraseña actualizada correctamente", user.id_user.ToString());

        await _repo.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        await _log.LogError($"Error en ResetPassword: {ex.Message}");
        await _repo.SaveChangesAsync();
        throw;
    }
}
}