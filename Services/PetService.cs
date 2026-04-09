using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class PetService : IPetService
{
    private readonly IPetRepository _repo;
    public PetService(IPetRepository repo) => _repo = repo;

    public async Task<List<PetDto>> GetAllAsync()
    {
        var pets = await _repo.GetAllAsync();
        return pets.Select(MapToDto).ToList();
    }
    public async Task<List<PetDto>> GetByUserAsync(Guid id_user)
    {
        var pets = await _repo.GetByPetAsync(id_user);
        return pets.Select(MapToDto).ToList();
    }

    public async Task<PetDto> GetByIdAsync(Guid id_pet)
    {
        var pet = await _repo.GetByIdAsync(id_pet)
            ?? throw new Exception("Mascota no encontrada");
        return MapToDto(pet);
    }

    public async Task<PetDto> CreateAsync(Guid id_user, CreatePetDto dto)
    {
        var pet = new Pet
        {
            name = dto.name,
            breed = dto.breed,
            species = dto.species,
            weight = dto.weight,
            age = dto.age,
            id_user = id_user
        };

        await _repo.AddAsync(pet);
        await _repo.SaveChangesAsync();

        // Recargar para traer la navegación User
        var created = await _repo.GetByIdAsync(pet.id_pet)
            ?? throw new Exception("Error al crear la mascota");
        return MapToDto(created);
    }

    public async Task<PetDto> UpdateAsync(Guid id_pet, UpdatePetDto dto)
    {
        var pet = await _repo.GetByIdAsync(id_pet)
            ?? throw new Exception("Mascota no encontrada");

        pet.name = dto.name;
        pet.breed = dto.breed;
        pet.species = dto.species;
        pet.weight = dto.weight;
        pet.age = dto.age;

        await _repo.UpdateAsync(pet);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdAsync(id_pet)
            ?? throw new Exception("Error al actualizar la mascota");
        return MapToDto(updated);
    }

    public async Task DeleteAsync(Guid id_pet)
    {
        var pet = await _repo.GetByIdAsync(id_pet)
            ?? throw new Exception("Mascota no encontrada");

        await _repo.DeleteAsync(pet);
        await _repo.SaveChangesAsync();
    }

    private static PetDto MapToDto(Pet p) => new(
        p.id_pet,
        p.name,
        p.breed,
        p.species,
        p.weight,
        p.age,
        p.user?.name ?? "",
        p.id_user
    );
}