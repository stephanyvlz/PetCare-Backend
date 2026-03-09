using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByCorreoAsync(string correo);
    Task<Usuario?> GetByIdAsync(Guid id);
    Task<List<Usuario>> GetAllAsync();
    Task<List<Usuario>> GetByRolAsync(int rolId);
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task SaveChangesAsync();
}