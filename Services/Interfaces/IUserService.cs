using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<List<UserDto>> GetVeterinarianAsync();
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto);
    Task DeleteAsync(Guid id);
}