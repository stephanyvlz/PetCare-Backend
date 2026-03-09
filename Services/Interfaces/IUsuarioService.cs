using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> GetAllAsync();
    Task<List<UsuarioDto>> GetVeterinariosAsync();
    Task<UsuarioDto> GetByIdAsync(Guid id);
    Task<UsuarioDto> UpdateAsync(Guid id, UpdateUsuarioDto dto);
    Task DeleteAsync(Guid id);
}