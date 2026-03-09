using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class MascotaRepository : IMascotaRepository
{
    private readonly AppDbContext _db;
    public MascotaRepository(AppDbContext db) => _db = db;

    public Task<List<Mascota>> GetByUsuarioAsync(Guid idUsuario) =>
        _db.Mascotas.Where(m => m.IdUsuario == idUsuario).ToListAsync();

    public Task<Mascota?> GetByIdAsync(Guid id) =>
        _db.Mascotas.FirstOrDefaultAsync(m => m.IdMascota == id);

    public async Task AddAsync(Mascota mascota) =>
        await _db.Mascotas.AddAsync(mascota);

    public Task DeleteAsync(Mascota mascota)
    {
        _db.Mascotas.Remove(mascota);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}