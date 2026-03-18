using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class PetRepository : IPetRepository
{
    private readonly AppDbContext _db;
    public PetRepository(AppDbContext db) => _db = db;

    public Task<List<Pet>> GetAllAsync() =>
    _db.Pets
       .Include(p => p.user)
       .ToListAsync();
    public Task<List<Pet>> GetByPetAsync(Guid id_user) =>
        _db.Pets
           .Include(p => p.user)
           .Where(p => p.id_user == id_user)
           .ToListAsync();

    public Task<Pet?> GetByIdAsync(Guid id) =>
        _db.Pets
           .Include(p => p.user)
           .FirstOrDefaultAsync(p => p.id_pet == id);

    public async Task AddAsync(Pet pet) =>
        await _db.Pets.AddAsync(pet);

    public Task UpdateAsync(Pet pet)
    {
        _db.Pets.Update(pet);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Pet pet)
    {
        _db.Pets.Remove(pet);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}