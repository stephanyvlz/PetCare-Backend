namespace PetCare.API.Models.DTOs;

public record RegisterDto(
    string name,
    string email,
    string Password,
    int id_role
);

public record LoginDto(
    string email,
    string Password
);

public record UserDto(
    Guid id_user, // identificador único del usuario
    string name, // nombre del usuario
    string email, // correo electrónico del usuario unico
    string password, // contraseña encriptada
    int id_role, // rol del usuario (1: Admin, 2: Veterinario, 3: Cliente)
    DateTime created_at, // fecha de creación del usuario 
    DateTime updated_at // ultima actualización del usuario
);

public record UpdateUserDto(
    string name,
    string email
);

public record LoginResponseDto(
    string Token
);