using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAsync();
    Task<List<AppointmentDto>> GetByUserAsync(Guid id_user);
    Task<AppointmentDto> GetByIdAsync(Guid id_appointment);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentDto> UpdateAsync(Guid id_appointment, UpdateAppointmentDto Dto);
    Task<AppointmentDto> ChangeStatusAsync(Guid id_appointment, string status);
    Task<List<AppointmentDto>> GetByVeterinarinarianAsync(Guid id_veterinarian);
    Task DeleteAsync(Guid id_appointment);
}