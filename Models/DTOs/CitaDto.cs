namespace PetCare.API.Models.DTOs;

public record CitaDto(
    Guid IdCita,
    string NombreUsuario,
    string NombreMascota,
    string NombreVeterinario,
    DateTime Fecha,
    string Servicio,
    decimal Costo,
    string Estado
);

public record CreateCitaDto(
    Guid IdUsuario,
    Guid IdMascota,
    Guid IdClinica,
    Guid IdVeterinario,
    DateTime Fecha,
    string Servicio,
    decimal Costo
);

public record CambiarEstadoDto(
    string Estado
);