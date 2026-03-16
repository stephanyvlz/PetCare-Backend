using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByEmailAsync(string email) =>
        _db.Users
           .Include(u => u.role)
           .FirstOrDefaultAsync(u => u.email == email);

    public Task<User?> GetByIdAsync(Guid id) =>
        _db.Users
           .Include(u => u.role)
           .FirstOrDefaultAsync(u => u.id_user == id);

    public Task<List<User>> GetAllAsync() =>
        _db.Users
           .Include(u => u.role)
           .ToListAsync();

    public Task<List<User>> GetByRolAsync(int rolId) =>
        _db.Users
           .Include(u => u.role)
           .Where(u => u.id_role == rolId)
           .ToListAsync();

    public async Task AddAsync(User user) =>
        await _db.Users.AddAsync(user);

    public Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }
    public Task DeleteAsync(User user)
    {
        _db.Users.Remove(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}