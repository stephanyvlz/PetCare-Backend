using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IConsultaRepository
{
    Task<Consulta?> GetByCitaAsync(Guid idCita);
    Task<Consulta?> GetByIdAsync(Guid id);
    Task AddAsync(Consulta consulta);
    Task SaveChangesAsync();
}