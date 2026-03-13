using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface ITreatmentRepository
{
    Task<List<Treatment>> GetByConsultaAsync(Guid idConsulta);
    Task AddAsync(Treatment tratamiento);
    Task SaveChangesAsync();
    Task<IEnumerable<object>> GetByConsultationAsync(Guid id_consultation);
}