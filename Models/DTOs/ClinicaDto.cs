namespace PetCare.API.Models.DTOs;

public record ClinicaDto(
    Guid IdClinica,
    string Ubicacion,
    string Horario
);

public record CreateClinicaDto(
    string Ubicacion,
    string Horario
);