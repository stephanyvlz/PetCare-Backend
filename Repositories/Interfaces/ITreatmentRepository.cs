using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface ITreatmentRepository
{
    Task<List<Treatment>> GetAllAsync();
    Task<Treatment?> GetByIdAsync(Guid id);
    Task<List<Treatment>> GetByConsultationAsync(Guid id_consultation);
    Task AddAsync(Treatment treatment);
    Task UpdateAsync(Treatment treatment);
    Task DeleteAsync(Treatment treatment);
    Task SaveChangesAsync();
}