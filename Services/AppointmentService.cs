using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repo;
    public AppointmentService(IAppointmentRepository repo) => _repo = repo;

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _repo.GetAllAsync();
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<List<AppointmentDto>> GetByUserAsync(Guid id_user)
    {
        var appointments = await _repo.GetByUserAsync(id_user);
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<AppointmentDto> GetByIdAsync(Guid id_appointment)
    {
        var appointment = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Cita no encontrada");
        return MapToDto(appointment);
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        var appointment = new Appointment
        {
            id_user = dto.id_user,
            id_pet = dto.id_pet,
            id_clinic = dto.id_clinic,
            id_veterinarian = dto.id_veterinarian,
            appointment_date = dto.appointment_date,
            service = dto.service,
            cost = dto.cost,
            status = "pendiente"
        };

        await _repo.AddAsync(appointment);
        await _repo.SaveChangesAsync();

        var created = await _repo.GetByIdAsync(appointment.id_appointment)
            ?? throw new Exception("Error al crear la cita");
        return MapToDto(created);
    }

    public async Task<AppointmentDto> UpdateAsync(Guid id_appointment, UpdateAppointmentDto dto)
    {
        var appointment = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Cita no encontrada");

        // ✅ Solo se pueden editar citas pendientes
        if (appointment.status != "pendiente")
            throw new Exception("Solo se pueden modificar citas con estado 'pendiente'");

        appointment.appointment_date = dto.appointment_date;
        appointment.service = dto.service;
        appointment.id_veterinarian = dto.id_veterinarian;

        await _repo.UpdateAsync(appointment);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Error al actualizar la cita");
        return MapToDto(updated);
    }

    public async Task<AppointmentDto> ChangeStatusAsync(Guid id_appointment, string status)
    {
        var validStatuses = new[] { "pendiente", "atendida", "cancelada" };

        if (!validStatuses.Contains(status))
            throw new Exception("Estado inválido. Use: pendiente, atendida o cancelada");

        var appointment = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Cita no encontrada");

        if (appointment.status == "atendida" || appointment.status == "cancelada")
            throw new Exception("No se puede cambiar el estado de una cita ya atendida o cancelada");

        appointment.status = status;
        await _repo.UpdateAsync(appointment);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Error al actualizar estado");
        return MapToDto(updated);
    }

    public async Task<List<AppointmentDto>> GetByVeterinarinarianAsync(Guid id_veterinarian)
    {
        var appointments = await _repo.GetByVeterinarianAsync(id_veterinarian);
        return appointments.Select(MapToDto).ToList();
    }

    public async Task DeleteAsync(Guid id_appointment)
    {
        var appointment = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Cita no encontrada");

        if (appointment.status == "atendida")
            throw new Exception("No se puede eliminar una cita que ya fue atendida");

        await _repo.DeleteAsync(appointment);
        await _repo.SaveChangesAsync();
    }

    private static AppointmentDto MapToDto(Appointment c) => new(
        c.id_appointment,
        c.User.name,
        c.Pet.name,
        c.veterinarian.name,
        c.appointment_date,
        c.service,
        c.cost,
        c.status
    );
}