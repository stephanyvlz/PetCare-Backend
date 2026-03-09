using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _db;
    public UsuarioRepository(AppDbContext db) => _db = db;

    public Task<Usuario?> GetByCorreoAsync(string correo) =>
        _db.Usuarios
           .Include(u => u.Rol)
           .FirstOrDefaultAsync(u => u.Correo == correo);

    public Task<Usuario?> GetByIdAsync(Guid id) =>
        _db.Usuarios
           .Include(u => u.Rol)
           .FirstOrDefaultAsync(u => u.IdUsuario == id);

    public Task<List<Usuario>> GetAllAsync() =>
        _db.Usuarios
           .Include(u => u.Rol)
           .ToListAsync();

    public Task<List<Usuario>> GetByRolAsync(int rolId) =>
        _db.Usuarios
           .Include(u => u.Rol)
           .Where(u => u.RolId == rolId)
           .ToListAsync();

    public async Task AddAsync(Usuario usuario) =>
        await _db.Usuarios.AddAsync(usuario);

    public Task UpdateAsync(Usuario usuario)
    {
        _db.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}