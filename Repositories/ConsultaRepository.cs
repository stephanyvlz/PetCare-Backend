using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class ConsultaRepository : IConsultaRepository
{
    private readonly AppDbContext _db;
    public ConsultaRepository(AppDbContext db) => _db = db;

    public Task<Consulta?> GetByCitaAsync(Guid idCita) =>
        _db.Consultas
           .Include(c => c.Tratamientos)
           .FirstOrDefaultAsync(c => c.IdCita == idCita);

    public Task<Consulta?> GetByIdAsync(Guid id) =>
        _db.Consultas
           .Include(c => c.Tratamientos)
           .FirstOrDefaultAsync(c => c.IdConsulta == id);

    public async Task AddAsync(Consulta consulta) =>
        await _db.Consultas.AddAsync(consulta);

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}