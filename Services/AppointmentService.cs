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
    private readonly ILogService _logService;

    // Configuración de horarios
    private readonly TimeSpan WORK_START = new TimeSpan(9, 0, 0);    // 9:00 AM
    private readonly TimeSpan WORK_END = new TimeSpan(18, 0, 0);     // 6:00 PM
    private readonly int SLOT_DURATION_MINUTES = 30;                  // 30 minutos por cita

    public AppointmentService(IAppointmentRepository repo, IEmailService emailService, ILogService logService)
    {
        _repo         = repo;
        _emailService = emailService;
        _logService   = logService;
    }

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        try
        {
            var appointments = await _repo.GetAllAsync();
            await _logService.LogInfo($"Se consultaron todas las citas. Total devuelto: {appointments.Count}", null);
            return appointments.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al obtener todas las citas: {ex.Message}", null);
            throw;
        }
    }

    public async Task<List<AppointmentDto>> GetByUserAsync(Guid id_user)
    {
        try
        {
            var appointments = await _repo.GetByUserAsync(id_user);
            await _logService.LogInfo($"Se consultaron citas del usuario {id_user}. Total: {appointments.Count}", id_user.ToString());
            return appointments.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al obtener citas del usuario {id_user}: {ex.Message}", id_user.ToString());
            throw;
        }
    }

    public async Task<AppointmentDto> GetByIdAsync(Guid id_appointment)
    {
        try
        {
            var appointment = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Cita no encontrada");

            await _logService.LogInfo($"Consulta de detalle de cita ID: {id_appointment}", null);
            return MapToDto(appointment);
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al obtener cita por ID {id_appointment}: {ex.Message}", null);
            throw;
        }
    }

    public async Task<List<AppointmentDto>> GetByClinicAsync(Guid id_clinic)
    {
        try
        {
            var appointments = await _repo.GetByClinicAsync(id_clinic);
            await _logService.LogInfo($"Se consultaron citas de la clínica {id_clinic}. Total: {appointments.Count}", null);
            return appointments.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al obtener citas de la clínica {id_clinic}: {ex.Message}", null);
            throw;
        }
    }

    public async Task<List<AppointmentDto>> GetByVeterinarinarianAsync(Guid id_veterinarian)
    {
        try
        {
            var appointments = await _repo.GetByVeterinarianAsync(id_veterinarian);
            await _logService.LogInfo($"Se consultaron citas del veterinario {id_veterinarian}. Total: {appointments.Count}", id_veterinarian.ToString());
            return appointments.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al obtener citas del veterinario {id_veterinarian}: {ex.Message}", id_veterinarian.ToString());
            throw;
        }
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        try
        {
            var appointmentDate = dto.appointment_date.ToUniversalTime();

            var occupiedSlots = await _repo.GetOccupiedSlotsAsync(dto.id_veterinarian, appointmentDate);

            if (occupiedSlots.Any(slot => slot.TimeOfDay == appointmentDate.TimeOfDay))
            {
                await _logService.LogInfo($"Intento fallido de crear cita: horario ya ocupado. Veterinario: {dto.id_veterinarian}, Fecha: {appointmentDate}, Usuario: {dto.id_user}", dto.id_user.ToString());
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

            await _logService.LogInfo($"Cita creada exitosamente. ID: {created.id_appointment}, Usuario: {dto.id_user}, Veterinario: {dto.id_veterinarian}, Fecha: {appointmentDate}", dto.id_user.ToString());

            return MapToDto(created);
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al crear cita para usuario {dto.id_user}: {ex.Message}", dto.id_user.ToString());
            throw;
        }
    }

    public async Task<AppointmentDto> UpdateAsync(Guid id_appointment, UpdateAppointmentDto dto)
    {
        try
        {
            var appointment = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Cita no encontrada");

            if (appointment.status != "pendiente")
            {
                await _logService.LogInfo($"Intento de modificar cita {id_appointment} que no está en estado pendiente", appointment.id_user.ToString());
                throw new Exception("Solo se pueden modificar citas con estado 'pendiente'");
            }

            appointment.appointment_date = dto.appointment_date.ToUniversalTime();
            appointment.service = dto.service;
            appointment.id_veterinarian = dto.id_veterinarian;

            await _repo.UpdateAsync(appointment);
            await _repo.SaveChangesAsync();

            var updated = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Error al actualizar la cita");

            await _logService.LogInfo($"Cita actualizada exitosamente. ID: {id_appointment}, Nueva fecha: {updated.appointment_date}", updated.id_user.ToString());

            return MapToDto(updated);
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al actualizar cita {id_appointment}: {ex.Message}", null);
            throw;
        }
    }

    public async Task<AppointmentDto> CancelMyAppointmentAsync(Guid id_appointment, Guid id_user)
    {
        try
        {
            var appointment = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Cita no encontrada");

            if (appointment.id_user != id_user)
            {
                await _logService.LogError($"Intento no autorizado de cancelar cita {id_appointment} por usuario {id_user}", id_user.ToString());
                throw new UnauthorizedAccessException("No puedes cancelar una cita que no es tuya");
            }

            if (appointment.status == "atendida")
            {
                await _logService.LogInfo($"Intento de cancelar cita ya atendida {id_appointment}", id_user.ToString());
                throw new Exception("No se puede cancelar una cita que ya fue atendida");
            }

            if (appointment.status == "cancelada")
            {
                await _logService.LogInfo($"Intento de cancelar cita ya cancelada {id_appointment}", id_user.ToString());
                throw new Exception("Esta cita ya está cancelada");
            }

            appointment.status = "cancelada";
            await _repo.UpdateAsync(appointment);
            await _repo.SaveChangesAsync();

            var updated = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Error al cancelar la cita");

            await _logService.LogInfo($"Cita cancelada exitosamente. ID: {id_appointment} por usuario {id_user}", id_user.ToString());

            return MapToDto(updated);
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al cancelar cita {id_appointment}: {ex.Message}", id_user.ToString());
            throw;
        }
    }

    public async Task<AppointmentDto> ChangeStatusAsync(Guid id_appointment, string status)
    {
        try
        {
            var validStatuses = new[] { "pendiente", "confirmada", "atendida", "cancelada" };
 
            if (!validStatuses.Contains(status))
            {
                await _logService.LogError($"Intento de usar estado inválido '{status}' en cita {id_appointment}", null);
                throw new Exception("Estado inválido. Use: pendiente, confirmada, atendida o cancelada");
            }

            var appointment = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Cita no encontrada");
 
            if (appointment.status == "atendida" || appointment.status == "cancelada")
            {
                await _logService.LogInfo($"Intento de cambiar estado de cita finalizada {id_appointment} a '{status}'", appointment.id_user.ToString());
                throw new Exception("No se puede cambiar el estado de una cita ya atendida o cancelada");
            }
 
            appointment.status = status;
            await _repo.UpdateAsync(appointment);
            await _repo.SaveChangesAsync();
 
            var updated = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Error al actualizar estado");

            await _logService.LogInfo($"Estado de cita {id_appointment} cambiado a '{status}'", updated.User?.id_user.ToString());

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
                catch (Exception emailEx)
                {
                    await _logService.LogError($"Fallo al enviar correo de confirmación para cita {id_appointment}: {emailEx.Message}", updated.User?.id_user.ToString());
                }
            }

            return MapToDto(updated);
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al cambiar estado de cita {id_appointment} a {status}: {ex.Message}", null);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id_appointment)
    {
        try
        {
            var appointment = await _repo.GetByIdAsync(id_appointment)
                ?? throw new Exception("Cita no encontrada");

            await _repo.DeleteAsync(appointment);
            await _repo.SaveChangesAsync();

            await _logService.LogInfo($"Cita eliminada exitosamente. ID: {id_appointment}", appointment.id_user.ToString());
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al eliminar cita {id_appointment}: {ex.Message}", null);
            throw;
        }
    }

    public async Task<List<string>> GetAvailableDatesAsync(Guid id_veterinarian)
    {
        try
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

            await _logService.LogInfo($"Consulta de fechas disponibles para veterinario {id_veterinarian}. Fechas disponibles: {availableDates.Count}", id_veterinarian.ToString());

            return availableDates;
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al obtener fechas disponibles para veterinario {id_veterinarian}: {ex.Message}", id_veterinarian.ToString());
            throw;
        }
    }

    public async Task<List<string>> GetAvailableSlotsAsync(Guid id_veterinarian, DateTime date)
    {
        try
        {
            var utcDate = date.ToUniversalTime();

            var occupiedSlots = await _repo.GetOccupiedSlotsAsync(id_veterinarian, utcDate);
            var allSlots = GenerateAllSlots(utcDate);

            var availableSlots = allSlots
                .Where(slot => !occupiedSlots.Any(occupied => occupied.TimeOfDay == slot.TimeOfDay))
                .Select(slot => slot.ToString("HH:mm"))
                .ToList();

            await _logService.LogInfo($"Consulta de slots disponibles para veterinario {id_veterinarian} en fecha {utcDate:yyyy-MM-dd}. Slots disponibles: {availableSlots.Count}", id_veterinarian.ToString());

            return availableSlots;
        }
        catch (Exception ex)
        {
            await _logService.LogError($"Error al obtener slots disponibles para veterinario {id_veterinarian} en fecha {date}: {ex.Message}", id_veterinarian.ToString());
            throw;
        }
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