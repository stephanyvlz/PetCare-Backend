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
    public async Task<List<UserDto>> GetVeterinariansByClinicAsync(Guid id_clinic)
    {
        // RolId 2 = veterinario según el seed
        var veterinarians = await _repo.GetByClinicAndRolAsync(id_clinic, 2);
        return veterinarians.Select(MapToDto).ToList();
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");

        return MapToDto(user);
    }

    public async Task<UserDto> UpdateProfileAsync(Guid id, UpdateProfileDto dto)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");

        user.name = dto.name;
        user.phone = dto.phone;
        user.updated_at = DateTime.UtcNow;

        await _repo.SaveChangesAsync();
        return MapToDto(user);
    }

   public async Task<UserDto> AdminUpdateAsync(Guid id, AdminUpdateUserDto dto)
{
    var user = await _repo.GetByIdAsync(id)
        ?? throw new Exception("Usuario no encontrado");

    user.name = dto.name;
    user.phone = dto.phone;
    user.id_role = dto.id_role;
    user.id_clinic = dto.id_clinic;
    user.updated_at = DateTime.UtcNow;

    if (dto.id_role == 2)
    {
        if (string.IsNullOrWhiteSpace(dto.schedule))
            throw new Exception("El horario es obligatorio para el veterinario");

        user.schedule = dto.schedule;
    }
    else
    {
        user.schedule = null; // ← limpia si no es veterinario
    }

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
    new(
        u.id_user,
        u.name,
        u.email,
        u.id_role,
        u.phone,
        u.schedule,
        u.created_at,
        u.updated_at,
        u.id_clinic
    );

}