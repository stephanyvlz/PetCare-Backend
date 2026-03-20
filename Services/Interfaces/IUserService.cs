using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<List<UserDto>> GetVeterinarianAsync();
    Task<List<UserDto>> GetVeterinariansByClinicAsync(Guid id_clinic);
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> UpdateProfileAsync(Guid id, UpdateProfileDto dto);
    Task<UserDto> AdminUpdateAsync(Guid id, AdminUpdateUserDto dto);
    Task DeleteAsync(Guid id);
}