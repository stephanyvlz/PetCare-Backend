using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IConsultationRepository
{
    Task<List<Consultation>> GetAllAsync();
    Task<Consultation?> GetByIdAsync(Guid id);
    Task<Consultation?> GetByAppointmentAsync(Guid id_appointment);
    Task AddAsync(Consultation consulta);
    Task UpdateAsync(Consultation consulta);
    Task DeleteAsync(Consultation consulta);
    Task SaveChangesAsync();
}