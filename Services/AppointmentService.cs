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

        var appointmentCreate = await _repo.GetByIdAsync(appointment.id_appointment)
            ?? throw new Exception("Error al crear la cita");

        return MapToDto(appointmentCreate);
    }

    public async Task<AppointmentDto> ChangeStatusAsync(Guid id_appointment, string status)
    {
        var StatusValidation = new[] { "pendiente", "atendida", "cancelada" };

        if (!StatusValidation.Contains(status))
            throw new Exception("Estado inválido. Use: pendiente, atendida o cancelada");

        var appointment = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Cita no encontrada");

        appointment.status = status;
        await _repo.SaveChangesAsync();

        return MapToDto(appointment);
    }
    public async Task<List<AppointmentDto>> GetByVeterinarianAsync(Guid id_veterinarian)
    {
        var Appointments = await _repo.GetByVeterinarianAsync(id_veterinarian);
        return Appointments.Select(MapToDto).ToList();
    }

    // Mapeo reutilizable
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

    Task<List<AppointmentDto>> IAppointmentService.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<List<AppointmentDto>> IAppointmentService.GetByUserAsync(Guid id_user)
    {
        throw new NotImplementedException();
    }

    Task<AppointmentDto> IAppointmentService.CreateAsync(CreateAppointmentDto dto)
    {
        throw new NotImplementedException();
    }

    Task<AppointmentDto> IAppointmentService.ChangeStatusAsync(Guid id_appointment, string status)
    {
        throw new NotImplementedException();
    }

    Task<List<AppointmentDto>> IAppointmentService.GetByVeterinarinarianAsync(Guid id_veterinarian)
    {
        throw new NotImplementedException();
    }
}