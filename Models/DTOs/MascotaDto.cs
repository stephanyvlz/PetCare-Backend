namespace PetCare.API.Models.DTOs;

public record MascotaDto(
    Guid IdMascota,
    string Nombre,
    string Raza,
    decimal Peso,
    int Edad
);

public record CreateMascotaDto(
    string Nombre,
    string Raza,
    decimal Peso,
    int Edad
);