using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class TreatmentRepository : ITreatmentRepository
{
    private readonly AppDbContext _db;
    public TreatmentRepository(AppDbContext db) => _db = db;

    public Task<List<Treatment>> GetByConsultaAsync(Guid idConsulta) =>
        _db.Treatment
           .Where(t => t.id_consultation == idConsulta)
           .ToListAsync();

    public async Task AddAsync(Treatment tratamiento) =>
        await _db.Treatment.AddAsync(tratamiento);

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();

    public Task<IEnumerable<object>> GetByConsultationAsync(Guid id_consultation)
    {
        throw new NotImplementedException();
    }
}