using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class PetRepository : IPetRepository
{
    private readonly AppDbContext _db;
    public PetRepository(AppDbContext db) => _db = db;

    public Task<List<Pet>> GetByUserAsync(Guid id_user) =>
        _db.Pets.Where(m => m.id_user == id_user).ToListAsync();

    public Task<Pet?> GetByIdAsync(Guid id) =>
        _db.Pets.FirstOrDefaultAsync(m => m.id_pet == id);

    public async Task AddAsync(Pet pet) =>
        await _db.Pets.AddAsync(pet);

    public Task DeleteAsync(Pet pet)
    {
        _db.Pets.Remove(pet);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();

    public Task<List<Pet>> GetByPetAsync(Guid id_user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Pet pet)
    {
        throw new NotImplementedException();
    }
}