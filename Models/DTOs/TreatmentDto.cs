namespace PetCare.API.Models.DTOs;

public record TreatmentDto(
    Guid id_treatment,
    string medication,// Medicamento
    string dosage, // Dosis
    string duration,// Plazo
    decimal cost // Costo
);

public record CreateTreatmentDto(
    Guid id_consultation,
    string medication,
    string dosage,
    string duration,
    decimal cost
);

public record UpdateTreatmentDto(
    string medication,
    string dosage,
    string duration,
    decimal cost
);