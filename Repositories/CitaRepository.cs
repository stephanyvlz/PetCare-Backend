using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class CitaRepository : ICitaRepository
{
    private readonly AppDbContext _db;
    public CitaRepository(AppDbContext db) => _db = db;

    public Task<List<Cita>> GetAllAsync() =>
        _db.Citas
           .Include(c => c.Usuario)
           .Include(c => c.Mascota)
           .Include(c => c.Clinica)
           .Include(c => c.Veterinario)
           .ToListAsync();

    public Task<List<Cita>> GetByUsuarioAsync(Guid idUsuario) =>
        _db.Citas
           .Include(c => c.Mascota)
           .Include(c => c.Clinica)
           .Include(c => c.Veterinario)
           .Where(c => c.IdUsuario == idUsuario)
           .ToListAsync();

    public Task<List<Cita>> GetByVeterinarioAsync(Guid idVeterinario) =>
        _db.Citas
           .Include(c => c.Usuario)
           .Include(c => c.Mascota)
           .Include(c => c.Clinica)
           .Where(c => c.IdVeterinario == idVeterinario)
           .ToListAsync();

    public Task<Cita?> GetByIdAsync(Guid id) =>
        _db.Citas
           .Include(c => c.Usuario)
           .Include(c => c.Mascota)
           .Include(c => c.Clinica)
           .Include(c => c.Veterinario)
           .FirstOrDefaultAsync(c => c.IdCita == id);

    public async Task AddAsync(Cita cita) =>
        await _db.Citas.AddAsync(cita);

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}