namespace PetCare.API.Models.DTOs;

public record AppointmentDto(
    Guid id_appointment,
    string user_name,
    string pet_name,
    string veterinarian_name,
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
