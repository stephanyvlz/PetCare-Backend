using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IPetService
{
    Task<List<PetDto>> GetByUserAsync(Guid id_user);
    Task<PetDto> CreateAsync(Guid id_user, CreatePetDto dto);
    Task<PetDto> UpdateAsync(Guid id_pet, UpdateUserDto dto);
    Task<List<PetDto>> GetByPetAsync(Guid id_user);
    Task DeleteAsync(Guid id_pet);
}