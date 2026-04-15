namespace PetCare.API.Models.DTOs;

public record AppointmentDto(
    Guid id_appointment,

    // ── Dueño
    string user_name,

    // ── Mascota
    string pet_name,
    string pet_breed,
    decimal pet_weight,
    int pet_age,

    // ── Veterinario
    string veterinarian_name,
    string veterinarian_email,
    string? veterinarian_phone,

    // ── Clínica
    string? clinic_name,
    string? clinic_location,

    // ── Cita
    DateTime date,
    string service,
    decimal cost,
    string status
);

public record CreateAppointmentDto(
    Guid id_user,
    Guid id_pet,
    Guid id_clinic,
    Guid id_veterinarian,
    DateTime appointment_date,
    string service,
    decimal cost
);

public record UpdateAppointmentDto(
    Guid id_veterinarian,
    DateTime appointment_date,
    string service
);

public record ChangeStatusDto(
    string status
);
