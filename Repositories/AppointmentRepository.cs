using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;

namespace PetCare.API.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;
    public AppointmentRepository(AppDbContext db) => _db = db;

    public Task<List<Appointment>> GetAllAsync() =>
        _db.Appointment
           .Include(c => c.User)
           .Include(c => c.Pet)
           .Include(c => c.Clinic)
           .Include(c => c.veterinarian)
           .ToListAsync();

    public Task<List<Appointment>> GetByUserAsync(Guid id_user) =>
        _db.Appointment
           .Include(c => c.Pet)
           .Include(c => c.Clinic)
           .Include(c => c.veterinarian)
           .Where(c => c.id_user == id_user)
           .ToListAsync();

    public Task<List<Appointment>> GetByVeterinarianAsync(Guid id_veterinarian) =>
        _db.Appointment
           .Include(c => c.User)
           .Include(c => c.Pet)
           .Include(c => c.Clinic)
           .Where(c => c.id_veterinarian == id_veterinarian)
           .ToListAsync();

    public Task<Appointment?> GetByIdAsync(Guid id) =>
        _db.Appointment
           .Include(c => c.User)
           .Include(c => c.Pet)
           .Include(c => c.Clinic)
           .Include(c => c.veterinarian)
           .FirstOrDefaultAsync(c => c.id_appointment == id);

    public Task DeleteAsync(Appointment appointment)
    {
        _db.Appointment.Remove(appointment);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Appointment appointment)
    {
        _db.Appointment.Update(appointment);
        return Task.CompletedTask;
    }

    public async Task AddAsync(Appointment appointment) =>
        await _db.Appointment.AddAsync(appointment);

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}