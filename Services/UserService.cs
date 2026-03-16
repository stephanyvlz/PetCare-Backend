using PetCare.API.Models.DTOs;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return users.Select(MapToDto).ToList();
    }

    public async Task<List<UserDto>> GetVeterinarianAsync()
    {
        // RolId 2 = veterinario según el seed
        var veterinarians = await _repo.GetByRolAsync(2);
        return veterinarians.Select(MapToDto).ToList();
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");

        return MapToDto(user);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");
        user.name = dto.name;
        user.email = dto.email;
        user.updated_at = DateTime.UtcNow; 
        await _repo.UpdateAsync(user);
        await _repo.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");
        await _repo.DeleteAsync(user);  // ← usa el método del repo
        await _repo.SaveChangesAsync();
    }

    private static UserDto MapToDto(Models.Entities.User u) =>
        new(u.id_user, u.name, u.email, u.id_role,u.phone, u.created_at, u.updated_at);
}