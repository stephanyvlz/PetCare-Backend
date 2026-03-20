using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetAllAsync();
    Task<List<Appointment>> GetByUserAsync(Guid id_user);
    Task<List<Appointment>> GetByVeterinarianAsync(Guid id_veterinarian);
    Task<Appointment?> GetByIdAsync(Guid id);
    Task AddAsync(Appointment appointment);
    Task DeleteAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task SaveChangesAsync();
}