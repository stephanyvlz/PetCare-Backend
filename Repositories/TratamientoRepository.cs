using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class TratamientoRepository : ITratamientoRepository
{
    private readonly AppDbContext _db;
    public TratamientoRepository(AppDbContext db) => _db = db;

    public Task<List<Tratamiento>> GetByConsultaAsync(Guid idConsulta) =>
        _db.Tratamientos
           .Where(t => t.IdConsulta == idConsulta)
           .ToListAsync();

    public async Task AddAsync(Tratamiento tratamiento) =>
        await _db.Tratamientos.AddAsync(tratamiento);

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}