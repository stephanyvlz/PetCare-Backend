using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class TreatmentRepository : ITreatmentRepository
{
    private readonly AppDbContext _db;
    public TreatmentRepository(AppDbContext db) => _db = db;
    public Task<List<Treatment>> GetAllAsync() =>
    _db.Treatment
       .Include(t => t.Consultations)
       .ToListAsync();
    public Task<Treatment?> GetByIdAsync(Guid id) =>
        _db.Treatment
           .Include(t => t.Consultations)
           .FirstOrDefaultAsync(t => t.treatment_id == id);

    public Task<List<Treatment>> GetByConsultationAsync(Guid id_consultation) =>
        _db.Treatment
           .Where(t => t.id_consultation == id_consultation)
           .ToListAsync();

    public async Task AddAsync(Treatment treatment) =>
        await _db.Treatment.AddAsync(treatment);

    public Task UpdateAsync(Treatment treatment)
    {
        _db.Treatment.Update(treatment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Treatment treatment)
    {
        _db.Treatment.Remove(treatment);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}