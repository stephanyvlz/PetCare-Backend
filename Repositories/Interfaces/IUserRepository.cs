using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string correo);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>> GetAllAsync();
    Task<List<User>> GetByRolAsync(int rolId);
    Task AddAsync(User usuario);
    Task UpdateAsync(User usuario);
    Task SaveChangesAsync();
}