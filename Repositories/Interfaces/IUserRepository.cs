using PetCare.API.Models.Entities;

namespace PetCare.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>> GetAllAsync();
    Task<List<User>> GetByRolAsync(int rolId);
    Task<List<User>> GetByClinicAndRolAsync(Guid id_clinic, int rolId);
    Task DeleteAsync(User user);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task SaveChangesAsync();
    Task AddResetTokenAsync(PasswordResetToken token);
    Task<PasswordResetToken?> GetResetTokenAsync(string token);

    
}