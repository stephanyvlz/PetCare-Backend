using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IClinicRepository
{
    Task<List<Clinic>> GetAllAsync();
    Task<Clinic?> GetByIdAsync(Guid id);
    Task<Clinic> CreateAsync(Clinic clinic);
    Task<Clinic> UpdateAsync(Clinic clinic);
    Task DeleteAsync(Clinic clinic);
}