namespace PetCare.API.Models.DTOs;

public record ConsultaDto(
    Guid IdConsulta,
    Guid IdCita,
    string Diagnostico,
    string Observaciones,
    DateTime Fecha
);

public record CreateConsultaDto(
    Guid IdCita,
    string Diagnostico,
    string Observaciones
);