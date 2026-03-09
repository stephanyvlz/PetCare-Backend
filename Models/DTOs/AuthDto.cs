namespace PetCare.API.Models.DTOs;

public record RegisterDto(
    string Nombre,
    string Correo,
    string Password,
    int RolId
);

public record LoginDto(
    string Correo,
    string Password
);

public record UsuarioDto(
    Guid IdUsuario,
    string Nombre,
    string Correo,
    string Rol
);

public record UpdateUsuarioDto(
    string Nombre,
    string Correo
);

public record LoginResponseDto(
    string Token
);