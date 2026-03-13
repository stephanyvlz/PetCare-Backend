namespace PetCare.API.Models.DTOs;

public record ConsultationDto(
    Guid id_consultation,
    Guid id_appointment,
    string diagnosis,
    string observations,
    DateTime consultation_date
);

public record CreateConsultationDto(
    Guid id_consultation,
    Guid id_appointment,
    string diagnosis,
    string observations
);