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

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (await _repo.GetByEmailAsync(dto.email) is not null)
            throw new Exception("El correo ya está registrado");

        var user = new User
        {
            name = dto.name,
            email = dto.email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            id_role = dto.id_role
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

    var userDto = new UserSessionDto(
        user.id_user,
        user.name,
        user.email,
        user.id_role
    );

    return new LoginResponseDto(token, userDto);
}
}