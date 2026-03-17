using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class ClinicRepository : IClinicRepository
{
    private readonly AppDbContext _context;

    public ClinicRepository(AppDbContext context) =>
        _context = context;

    public async Task<List<Clinic>> GetAllAsync() =>
        await _context.Clinic.ToListAsync();

    public async Task<Clinic?> GetByIdAsync(Guid id) =>
        await _context.Clinic.FindAsync(id);

    public async Task<Clinic> CreateAsync(Clinic clinic)
    {
        _context.Clinic.Add(clinic);
        await _context.SaveChangesAsync();
        return clinic;
    }

    public async Task<Clinic> UpdateAsync(Clinic clinic)
    {
        _context.Clinic.Update(clinic);
        await _context.SaveChangesAsync();
        return clinic;
    }

    public async Task DeleteAsync(Clinic clinic)
    {
        _context.Clinic.Remove(clinic);
        await _context.SaveChangesAsync();
    }
}