using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface ITreatmentService
{
    Task<TreatmentDto> CreateAsync(CreateTreatmentDto dto);
    Task<List<TreatmentDto>> GetByConsultationAsync(Guid id_consultation);
}