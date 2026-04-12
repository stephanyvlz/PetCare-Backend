using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAsync();
    Task<List<AppointmentDto>> GetByUserAsync(Guid id_user);
    Task<List<AppointmentDto>> GetByClinicAsync(Guid id_clinic);
    Task<AppointmentDto> GetByIdAsync(Guid id_appointment);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentDto> UpdateAsync(Guid id_appointment, UpdateAppointmentDto Dto);
    Task<AppointmentDto> ChangeStatusAsync(Guid id_appointment, string status);
    Task<List<AppointmentDto>> GetByVeterinarinarianAsync(Guid id_veterinarian);
    Task<AppointmentDto> CancelMyAppointmentAsync(Guid id_appointment, Guid id_user);
    Task DeleteAsync(Guid id_appointment);
    Task<List<string>> GetAvailableSlotsAsync(Guid id_veterinarian, DateTime date);
    Task<List<string>> GetAvailableDatesAsync(Guid id_veterinarian);


}