namespace PetCare.API.Models.DTOs;

public record PetDto(
    Guid id_pet,
    string name,
    string breed,
    decimal weight,
    int age,
    string user_name,
    int id_user
)
{
    // Constructor auxiliar que reutiliza el constructor primario posicional mediante 'this(...)'
    public PetDto(Guid id_pet, string name, string breed, decimal weight, int age)
        : this(id_pet, name, breed, weight, age, string.Empty, 0)
    {
    }
}

public record CreatePetDto(
    string name,
    string breed,
    decimal weight,
    int age
);