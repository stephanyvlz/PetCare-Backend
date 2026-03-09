using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface ITratamientoRepository
{
    Task<List<Tratamiento>> GetByConsultaAsync(Guid idConsulta);
    Task AddAsync(Tratamiento tratamiento);
    Task SaveChangesAsync();
}