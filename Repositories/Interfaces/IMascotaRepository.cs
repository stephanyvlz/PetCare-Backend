using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IMascotaRepository
{
    Task<List<Mascota>> GetByUsuarioAsync(Guid idUsuario);
    Task<Mascota?> GetByIdAsync(Guid id);
    Task AddAsync(Mascota mascota);
    Task DeleteAsync(Mascota mascota);
    Task SaveChangesAsync();
}