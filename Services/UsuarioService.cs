using PetCare.API.Models.DTOs;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    public UsuarioService(IUsuarioRepository repo) => _repo = repo;

    public async Task<List<UsuarioDto>> GetAllAsync()
    {
        var usuarios = await _repo.GetAllAsync();
        return usuarios.Select(MapToDto).ToList();
    }

    public async Task<List<UsuarioDto>> GetVeterinariosAsync()
    {
        // RolId 2 = veterinario según el seed
        var veterinarios = await _repo.GetByRolAsync(2);
        return veterinarios.Select(MapToDto).ToList();
    }

    public async Task<UsuarioDto> GetByIdAsync(Guid id)
    {
        var usuario = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");

        return MapToDto(usuario);
    }

    public async Task<UsuarioDto> UpdateAsync(Guid id, UpdateUsuarioDto dto)
    {
        var usuario = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");

        usuario.Nombre = dto.Nombre;
        usuario.Correo = dto.Correo;

        await _repo.UpdateAsync(usuario);
        await _repo.SaveChangesAsync();

        return MapToDto(usuario);
    }

    public async Task DeleteAsync(Guid id)
    {
        var usuario = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");

        _repo.GetAllAsync(); // solo valida que existe
        await _repo.SaveChangesAsync();
    }

    private static UsuarioDto MapToDto(Models.Entities.Usuario u) =>
        new(u.IdUsuario, u.Nombre, u.Correo, u.Rol.NombreRol);
}