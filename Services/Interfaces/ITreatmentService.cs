using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface ITreatmentService
{
    Task<List<TreatmentDto>> GetAllAsync();
    Task<TreatmentDto> GetByIdAsync(Guid treatment_id);
    Task<List<TreatmentDto>> GetByConsultationAsync(Guid id_consultation);
    Task<TreatmentDto> CreateAsync(CreateTreatmentDto dto);
    Task<TreatmentDto> UpdateAsync(Guid treatment_id, UpdateTreatmentDto dto);
    Task DeleteAsync(Guid treatment_id);
}