using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IConsultationRepository
{
    Task<Consultation?> GetByCitaAsync(Guid idCita);
    Task<Consultation?> GetByIdAsync(Guid id);
    Task<Consultation?> GetByAppointmentAsync(Guid id_appointment);
    Task AddAsync(Consultation consulta);
    Task SaveChangesAsync();
}