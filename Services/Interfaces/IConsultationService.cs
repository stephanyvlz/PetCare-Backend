using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IConsultationService
{
    Task<ConsultationDto> CreateAsync(CreateConsultationDto dto);
    Task<ConsultationDto?> GetByAppointmentAsync(Guid id_appointment);
}