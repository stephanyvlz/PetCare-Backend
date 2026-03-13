using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class PetService : IPetService
{
    private readonly IPetRepository _repo;
    public PetService(IPetRepository repo) => _repo = repo;

    public async Task<List<PetDto>> GetByUsuarioAsync(Guid id_user)
    {
        var pets = await _repo.GetByPetAsync(id_user);
        return pets.Select(m => new PetDto(
            m.id_pet, m.name, m.breed, m.weight, m.age)).ToList();
    }

    public async Task<PetDto> CreateAsync(Guid id_user, CreatePetDto dto)
    {
        var pet = new Pet
        {
            name = dto.name,
            breed = dto.breed,
            weight = dto.weight,
            age = dto.age,
            id_user = id_user
        };

        await _repo.AddAsync(pet);
        await _repo.SaveChangesAsync();

        return new PetDto(pet.id_pet, pet.name,
            pet.breed, pet.weight, pet.age);
    }

    public async Task DeleteAsync(Guid id_pet)
    {
        var pet = await _repo.GetByIdAsync(id_pet)
            ?? throw new Exception("Mascota no encontrada");

        await _repo.DeleteAsync(pet);
        await _repo.SaveChangesAsync();
    }

    public Task<List<PetDto>> GetByUserAsync(Guid id_user)
    {
        throw new NotImplementedException();
    }

    public Task<PetDto> UpdateAsync(Guid id_pet, UpdateUserDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<List<PetDto>> GetByPetAsync(Guid id_user)
    {
        throw new NotImplementedException();
    }
}