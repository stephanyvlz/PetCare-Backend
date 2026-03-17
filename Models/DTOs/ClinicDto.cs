namespace PetCare.API.Models.DTOs;

public record ClinicDto(
    Guid id_clinic,
    string name,
    string location,
    string schedule
);

public record CreateClinicDto(
    string name,
    string location,
    string schedule
);