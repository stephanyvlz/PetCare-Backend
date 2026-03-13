namespace PetCare.API.Models.DTOs;

public record ClinicDto(
    Guid id_clinic,
    string location,
    string schedule
);

public record CreateClinicDto(
    string location,
    string schedule
);