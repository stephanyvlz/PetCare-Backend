// PetCare.API/Services/AppointmentService.cs
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repo;
    private readonly IEmailService _emailService;

    // Configuración de horarios
    private readonly TimeSpan WORK_START = new TimeSpan(9, 0, 0);    // 9:00 AM
    private readonly TimeSpan WORK_END = new TimeSpan(18, 0, 0);     // 6:00 PM
    private readonly int SLOT_DURATION_MINUTES = 30;                  // 30 minutos por cita

    public AppointmentService(IAppointmentRepository repo, IEmailService emailService)
    {
        _repo         = repo;
        _emailService = emailService;
    }

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

    public async Task<List<AppointmentDto>> GetByClinicAsync(Guid id_clinic)
    {
        var appointments = await _repo.GetByClinicAsync(id_clinic);
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        // ✅ Convertir fecha a UTC
        var appointmentDate = dto.appointment_date.ToUniversalTime();

        var occupiedSlots = await _repo.GetOccupiedSlotsAsync(dto.id_veterinarian, appointmentDate);

        if (occupiedSlots.Any(slot => slot.TimeOfDay == appointmentDate.TimeOfDay))
        {
            throw new Exception("El horario seleccionado ya está ocupado. Por favor elige otro.");
        }

        var appointment = new Appointment
        {
            id_user = dto.id_user,
            id_pet = dto.id_pet,
            id_clinic = dto.id_clinic,
            id_veterinarian = dto.id_veterinarian,
            appointment_date = appointmentDate,
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

        if (appointment.status != "pendiente")
            throw new Exception("Solo se pueden modificar citas con estado 'pendiente'");

        // ✅ Convertir fecha a UTC
        appointment.appointment_date = dto.appointment_date.ToUniversalTime();
        appointment.service = dto.service;
        appointment.id_veterinarian = dto.id_veterinarian;

        await _repo.UpdateAsync(appointment);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Error al actualizar la cita");
        return MapToDto(updated);
    }

    public async Task<AppointmentDto> CancelMyAppointmentAsync(Guid id_appointment, Guid id_user)
    {
        var appointment = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Cita no encontrada");

        if (appointment.id_user != id_user)
            throw new UnauthorizedAccessException("No puedes cancelar una cita que no es tuya");

        if (appointment.status == "atendida")
            throw new Exception("No se puede cancelar una cita que ya fue atendida");

        if (appointment.status == "cancelada")
            throw new Exception("Esta cita ya está cancelada");

        appointment.status = "cancelada";
        await _repo.UpdateAsync(appointment);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Error al cancelar la cita");
        return MapToDto(updated);
    }

    public async Task<List<string>> GetAvailableDatesAsync(Guid id_veterinarian)
    {
        var startDate = DateTime.UtcNow.Date;
        var endDate = DateTime.UtcNow.Date.AddDays(30);

        var occupiedDates = await _repo.GetOccupiedDatesAsync(id_veterinarian, startDate, endDate);

        var allDates = new List<DateTime>();
        var current = startDate;

        while (current <= endDate)
        {
            if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
            {
                allDates.Add(current);
            }
            current = current.AddDays(1);
        }

        var availableDates = new List<string>();

        foreach (var date in allDates)
        {
            var occupiedSlots = await _repo.GetOccupiedSlotsAsync(id_veterinarian, date);
            var allSlots = GenerateAllSlots(date);
            var availableSlots = allSlots.Where(slot => !occupiedSlots.Any(occupied =>
                occupied.TimeOfDay == slot.TimeOfDay)).ToList();

            if (availableSlots.Any())
            {
                availableDates.Add(date.ToString("yyyy-MM-dd"));
            }
        }

        return availableDates;
    }

    public async Task<AppointmentDto> ChangeStatusAsync(Guid id_appointment, string status)
    {
        var validStatuses = new[] { "pendiente", "confirmada", "atendida", "cancelada" };
 
        if (!validStatuses.Contains(status))
            throw new Exception("Estado inválido. Use: pendiente, confirmada, atendida o cancelada");
 
        var appointment = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Cita no encontrada");
 
        if (appointment.status == "atendida" || appointment.status == "cancelada")
            throw new Exception("No se puede cambiar el estado de una cita ya atendida o cancelada");
 
        appointment.status = status;
        await _repo.UpdateAsync(appointment);
        await _repo.SaveChangesAsync();
 
        var updated = await _repo.GetByIdAsync(id_appointment)
            ?? throw new Exception("Error al actualizar estado");
 
        // Enviar correo al cliente cuando la cita es confirmada
        if (status == "confirmada")
        {
            try
            {
                await _emailService.SendAppointmentConfirmedAsync(
                    toEmail:          updated.User.email,
                    clientName:       updated.User.name,
                    petName:          updated.Pet.name,
                    veterinarianName: updated.veterinarian.name,
                    clinicName:       updated.Clinic.name,
                    appointmentDate:  updated.appointment_date,
                    service:          updated.service
                );
            }
            catch
            {
                // El correo es best-effort: si falla no interrumpe el flujo
            }
        }
 
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
 
        await _repo.DeleteAsync(appointment);
        await _repo.SaveChangesAsync();
    }

    public async Task<List<string>> GetAvailableSlotsAsync(Guid id_veterinarian, DateTime date)
    {
        // ✅ Convertir fecha a UTC
        var utcDate = date.ToUniversalTime();

        var occupiedSlots = await _repo.GetOccupiedSlotsAsync(id_veterinarian, utcDate);
        var allSlots = GenerateAllSlots(utcDate);

        var availableSlots = allSlots
            .Where(slot => !occupiedSlots.Any(occupied =>
                occupied.TimeOfDay == slot.TimeOfDay))
            .Select(slot => slot.ToString("HH:mm"))
            .ToList();

        return availableSlots;
    }

    private List<DateTime> GenerateAllSlots(DateTime date)
    {
        var slots = new List<DateTime>();
        var baseDate = date.ToUniversalTime().Date;
        var current = baseDate + WORK_START;
        var end = baseDate + WORK_END;

        while (current < end)
        {
            slots.Add(current);
            current = current.AddMinutes(SLOT_DURATION_MINUTES);
        }

        return slots;
    }

    private static AppointmentDto MapToDto(Appointment c) => new(
    c.id_appointment,

    // Dueño
    c.User.name,

    // Mascota
    c.Pet.name,
    c.Pet.breed,
    c.Pet.weight,
    c.Pet.age,

    // Veterinario
    c.veterinarian.name,
    c.veterinarian.email,
    c.veterinarian.phone,

    // Clínica
    c.Clinic.name,
    c.Clinic.location,

    // Cita
    c.appointment_date,
    c.service,
    c.cost,
    c.status
);
}