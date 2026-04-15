namespace PetCare.API.Models.DTOs;

public record RegisterDto(
    string name,
    string email,
    string Password,
    string? phone,
    int id_role,
    Guid? id_clinic,
    string? schedule
);
public record LoginDto(
    string email,
    string Password
);

public record UserDto(
    Guid id_user, // identificador único del usuario
    string name, // nombre del usuario
    string email, // correo electrónico del usuario unico
    int id_role, // rol del usuario (1: Admin, 2: Veterinario, 3: Cliente)
    string? phone,
    string? schedule, 
    DateTime created_at, // fecha de creación del usuario 
    DateTime updated_at, // ultima actualización del usuario
    Guid? id_clinic
);

// Para que el usuario/veterinario edite su propio perfil
public record UpdateProfileDto(
    string name,
    string email,
    string password,
    string? phone
);

// Para que el admin edite cualquier usuario (incluyendo clínica)
public record AdminUpdateUserDto(
    string name,
    string email,
    string? phone,
    int id_role,
    Guid? id_clinic,
    string? schedule
);

public record UserSessionDto(
    Guid id_user,
    string name,
    string email,
    int id_role,
    string? phone,
    string? schedule
);

public record LoginResponseDto(
    string Token
);

public class ResetPasswordDto
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}