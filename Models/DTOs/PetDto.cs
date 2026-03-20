namespace PetCare.API.Models.DTOs;

public record PetDto(
    Guid id_pet,
    string name,
    string breed,
    decimal weight,
    int age,
    string user_name,
    Guid id_user
);

public record CreatePetDto(
    string name,
    string breed,
    decimal weight,
    int age
);

public record UpdatePetDto(
    string name,
    string breed,
    decimal weight,
    int age
);