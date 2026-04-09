using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class DonationRepository : IDonationRepository
{
    private readonly AppDbContext _db;
    public DonationRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Donation donation) =>
        await _db.Donations.AddAsync(donation);

    public Task<Donation?> GetByPaypalOrderIdAsync(string paypal_order_id) =>
        _db.Donations
           .FirstOrDefaultAsync(d => d.paypal_order_id == paypal_order_id);

    public Task<List<Donation>> GetAllAsync() =>
        _db.Donations
           .OrderByDescending(d => d.created_at)
           .ToListAsync();

    public Task UpdateAsync(Donation donation)
    {
        _db.Donations.Update(donation);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}