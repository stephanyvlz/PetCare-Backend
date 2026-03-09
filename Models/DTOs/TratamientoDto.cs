namespace PetCare.API.Models.DTOs;

public record TratamientoDto(
    Guid IdTratamiento,
    string Medicamento,
    string Dosis,
    string Plazo,
    decimal Costo
);

public record CreateTratamientoDto(
    Guid IdConsulta,
    string Medicamento,
    string Dosis,
    string Plazo,
    decimal Costo
);