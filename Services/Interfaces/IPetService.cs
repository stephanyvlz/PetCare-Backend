using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IPetService
{
    Task<List<PetDto>> GetAllAsync();
    Task<List<PetDto>> GetByUserAsync(Guid id_user);
    Task<PetDto> GetByIdAsync(Guid id_pet);
    Task<PetDto> CreateAsync(Guid id_user, CreatePetDto dto);
    Task<PetDto> UpdateAsync(Guid id_pet, UpdatePetDto dto);
    Task DeleteAsync(Guid id_pet);
}