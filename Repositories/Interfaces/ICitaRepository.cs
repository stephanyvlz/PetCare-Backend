using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface ICitaRepository
{
    Task<List<Cita>> GetAllAsync();
    Task<List<Cita>> GetByUsuarioAsync(Guid idUsuario);
    Task<List<Cita>> GetByVeterinarioAsync(Guid idVeterinario);
    Task<Cita?> GetByIdAsync(Guid id);
    Task AddAsync(Cita cita);
    Task SaveChangesAsync();
}