namespace PetCare.API.Repositories.Interfaces;

using PetCare.API.Models.Entities;

public interface IDonationRepository
{
    Task AddAsync(Donation donation);
    Task<Donation?> GetByPaypalOrderIdAsync(string paypal_order_id);
    Task<List<Donation>> GetAllAsync();
    Task UpdateAsync(Donation donation);
    Task SaveChangesAsync();
}