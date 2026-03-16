namespace PetCare.API.Models.DTOs;

public record RegisterDto(
    string name,
    string email,
    string Password,
    int id_role,
    string? phone //el teléfono es opcional, por eso el tipo string?  
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
    DateTime created_at, // fecha de creación del usuario 
    DateTime updated_at // ultima actualización del usuario
);

public record UpdateUserDto(
    string name,
    string email,
    string? phone,
    string? Password
);


public record UserSessionDto(
    Guid id_user,
    string name,
    string email,
    int id_role,
    string? phone
);


public record LoginResponseDto(
    string Token,
    UserSessionDto User
);