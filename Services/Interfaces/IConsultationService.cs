using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IConsultationService
{
    Task<List<ConsultationDto>> GetAllAsync();
    Task<ConsultationDto> GetByIdAsync(Guid id_consultation);
    Task<ConsultationDto?> GetByAppointmentAsync(Guid id_appointment);
    Task<ConsultationDto> CreateAsync(CreateConsultationDto dto);
    Task<ConsultationDto> UpdateAsync(Guid id_consultation, UpdateConsultationDto dto);
    Task DeleteAsync(Guid id_consultation);
}   