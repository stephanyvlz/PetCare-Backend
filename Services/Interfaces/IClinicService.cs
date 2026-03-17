using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IClinicService
{
    Task<List<ClinicDto>> GetAllAsync();
    Task<ClinicDto> GetByIdAsync(Guid id);
    Task<ClinicDto> CreateAsync(CreateClinicDto dto);
    Task<ClinicDto> UpdateAsync(Guid id, CreateClinicDto dto);
    Task DeleteAsync(Guid id);
}