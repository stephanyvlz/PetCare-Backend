using PetCare.API.Models.DTOs;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly ILogService _log;

    public UserService(IUserRepository repo, ILogService log)
    {
        _repo = repo;
        _log = log;
    }

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
    var user = await _repo.GetByIdAsync(id);

    if (user == null)
    {
        await _log.LogError($"Usuario no encontrado: {id}");
        await _repo.SaveChangesAsync();

        throw new Exception("Usuario no encontrado");
    }

    return MapToDto(user);
}

    public async Task<UserDto> UpdateProfileAsync(Guid id, UpdateProfileDto dto)
        {
            try
            {
                var user = await _repo.GetByIdAsync(id)
                    ?? throw new Exception("Usuario no encontrado");

                user.name = dto.name;
                user.phone = dto.phone;
                user.updated_at = DateTime.UtcNow;

                await _log.LogInfo("Perfil actualizado", user.id_user.ToString());

                await _repo.SaveChangesAsync();

                return MapToDto(user);
            }
            catch (Exception ex)
            {
                await _log.LogError($"Error al actualizar perfil: {ex.Message}");
                await _repo.SaveChangesAsync();
                throw;
            }
        }

    public async Task<UserDto> AdminUpdateAsync(Guid id, AdminUpdateUserDto dto)
    {
        try
        {
            var user = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Usuario no encontrado");

            var oldRole = user.id_role;
            var oldClinic = user.id_clinic;

            user.name = dto.name;
            user.phone = dto.phone;
            user.id_role = dto.id_role;
            user.id_clinic = dto.id_clinic;
            user.updated_at = DateTime.UtcNow;

            if (dto.id_role == 2)
            {
                if (string.IsNullOrWhiteSpace(dto.schedule))
                {
                    await _log.LogError("AdminUpdate fallido: veterinario sin horario");
                    await _repo.SaveChangesAsync();

                    throw new Exception("El horario es obligatorio para el veterinario");
                }

                user.schedule = dto.schedule;
            }
            else
            {
                user.schedule = null;
            }

            await _repo.SaveChangesAsync();

            await _log.LogInfo(
                $"Admin actualizó usuario. Rol: {oldRole} → {dto.id_role}, Clínica: {oldClinic} → {dto.id_clinic}",
                user.id_user.ToString()
            );

            await _repo.SaveChangesAsync();

            return MapToDto(user);
        }
        catch (Exception ex)
        {
            await _log.LogError($"Error en AdminUpdate: {ex.Message}");
            await _repo.SaveChangesAsync();
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");
        await _repo.DeleteAsync(user);
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