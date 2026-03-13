using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IPetRepository
{
    Task<List<Pet>> GetByPetAsync(Guid id_user);
    Task<Pet?> GetByIdAsync(Guid id);
    Task UpdateAsync(Pet pet);
    Task AddAsync(Pet pet);
    Task DeleteAsync(Pet pet);
    Task SaveChangesAsync();
}