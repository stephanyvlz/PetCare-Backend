using PetCare.API.Helpers;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repo;
    private readonly JwtHelper _jwt;

    public AuthService(IUsuarioRepository repo, JwtHelper jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (await _repo.GetByCorreoAsync(dto.Correo) is not null)
            throw new Exception("El correo ya está registrado");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Correo = dto.Correo,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RolId = dto.RolId
        };

        await _repo.AddAsync(usuario);
        await _repo.SaveChangesAsync();

        return "Usuario registrado exitosamente";
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var usuario = await _repo.GetByCorreoAsync(dto.Correo)
            ?? throw new Exception("Credenciales inválidas");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
            throw new Exception("Credenciales inválidas");

        return _jwt.GenerateToken(usuario);
    }
}