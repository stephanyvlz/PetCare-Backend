using Microsoft.EntityFrameworkCore;
using PetCare.API.Data;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;


namespace PetCare.API.Repositories;

public class ConsultationRepository : IConsultationRepository
{
    private readonly AppDbContext _db;
    public ConsultationRepository(AppDbContext db) => _db = db;
    public Task<List<Consultation>> GetAllAsync() =>
    _db.Consultation
       .Include(c => c.Appointments)
       .ToListAsync();

    public Task<Consultation?> GetByIdAsync(Guid id) =>
        _db.Consultation
           .Include(c => c.Appointments)
           .FirstOrDefaultAsync(c => c.id_consultation == id);

    public Task<Consultation?> GetByAppointmentAsync(Guid id_appointment) =>
        _db.Consultation
           .Include(c => c.Appointments)
           .FirstOrDefaultAsync(c => c.id_appointment == id_appointment);

    public async Task AddAsync(Consultation consulta) =>
        await _db.Consultation.AddAsync(consulta);

    public Task UpdateAsync(Consultation consulta)
    {
        _db.Consultation.Update(consulta);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Consultation consulta)
    {
        _db.Consultation.Remove(consulta);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _db.SaveChangesAsync();
}