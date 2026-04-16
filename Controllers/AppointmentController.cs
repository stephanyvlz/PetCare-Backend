// PetCare.API/Controllers/AppointmentController.cs
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;
using System.Security.Claims;

namespace PetCare.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Appointments")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IUserService _userService;
    private readonly ILogService _logService;
    public AppointmentController(IAppointmentService appointmentService, IUserService userService, ILogService logService)
    {
        _appointmentService = appointmentService;
        _userService = userService;
        _logService = logService;
    }

    // GET api/v1/citas
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _appointmentService.GetAllAsync();
        return Ok(ApiResponse<List<AppointmentDto>>.Ok(appointments));
    }

    // GET api/v1/Appointments/mi-clinica — admin ve citas de su clínica
    [HttpGet("mi-clinica")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetByMyClinic()
    {
        try
        {
            var id_admin = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var admin = await _userService.GetByIdAsync(id_admin);

            if (admin?.id_clinic == null)
                return BadRequest(ApiResponse<string>.Fail("No tienes una clínica asignada"));

            var appointments = await _appointmentService.GetByClinicAsync(admin.id_clinic.Value);
            return Ok(ApiResponse<List<AppointmentDto>>.Ok(appointments));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    //GET api/v1/citas/{id}
    [HttpGet("{id_appointment}")]
    [Authorize(Roles = "admin,veterinario,cliente")]
    public async Task<IActionResult> GetById(Guid id_appointment)
    {
        var appointment = await _appointmentService.GetByIdAsync(id_appointment);
        return Ok(ApiResponse<AppointmentDto>.Ok(appointment));
    }

    // GET api/v1/citas/mias
    [HttpGet("mias")]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> GetMyAppointments()
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var appointments = await _appointmentService.GetByUserAsync(id_user);
        return Ok(ApiResponse<List<AppointmentDto>>.Ok(appointments));
    }

    // GET api/v1/citas/mis-pacientes
    [HttpGet("my-patients")]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> GetMyPatients()
    {
        var id_veterinarian = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var appointments = await _appointmentService.GetByVeterinarinarianAsync(id_veterinarian);
        return Ok(ApiResponse<List<AppointmentDto>>.Ok(appointments));
    }

    // PATCH api/v1/Appointments/cancelar/{id}  — cliente cancela su propia cita
    [HttpPatch("cancelar/{id_appointment}")]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> CancelMyAppointment(Guid id_appointment)
    {
        try
        {
            var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var appointment = await _appointmentService.CancelMyAppointmentAsync(id_appointment, id_user);
            return Ok(ApiResponse<AppointmentDto>.Ok(appointment, "Cita cancelada exitosamente"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    // ✅ NUEVO ENDPOINT - Obtener horarios disponibles
    // GET api/v1/Appointments/available-slots?veterinarianId=xxx&date=2024-03-25
    [HttpGet("available-slots")]
    [Authorize(Roles = "admin,cliente")]
    public async Task<IActionResult> GetAvailableSlots(
        [FromQuery] Guid veterinarianId,
        [FromQuery] DateTime date)
    {
        try
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(veterinarianId, date);
            return Ok(ApiResponse<List<string>>.Ok(slots, "Horarios disponibles"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }
    // PetCare.API/Controllers/AppointmentController.cs
    // Agregar este endpoint

    // GET api/v1/Appointments/available-dates?veterinarianId=xxx
    [HttpGet("available-dates")]
    [Authorize(Roles = "admin,cliente")]
    public async Task<IActionResult> GetAvailableDates(
        [FromQuery] Guid veterinarianId)
    {
        try
        {
            var dates = await _appointmentService.GetAvailableDatesAsync(veterinarianId);
            return Ok(ApiResponse<List<string>>.Ok(dates, "Fechas disponibles"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }
    // POST api/v1/citas
    [HttpPost]
    [Authorize(Roles = "cliente,admin")]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        var appointment = await _appointmentService.CreateAsync(dto);
        return Ok(ApiResponse<AppointmentDto>.Ok(appointment, "Cita agendada exitosamente"));
    }

    // PATCH api/v1/citas/{idCita}/estado
    [HttpPatch("{id_appointment}/status")]
    [Authorize(Roles = "veterinario,admin")]
    public async Task<IActionResult> ChangeStatus(Guid id_appointment, [FromBody] ChangeStatusDto dto)
    {
        var appointment = await _appointmentService.ChangeStatusAsync(id_appointment, dto.status);
        return Ok(ApiResponse<AppointmentDto>.Ok(appointment, "Estado actualizado"));
    }

    //PUT api/api/citas/{id}
    [HttpPut("{id_appointment}")]
    [Authorize(Roles = "cliente, admin")]
    public async Task<IActionResult> Update(Guid id_appointment, [FromBody] UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentService.UpdateAsync(id_appointment, dto);
        return Ok(ApiResponse<AppointmentDto>.Ok(appointment, "Cita actualizada exitosamente"));
    }

    // DELETE api/v1/Appointments/{id}
    [HttpDelete("{id_appointment}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id_appointment)
    {
        await _appointmentService.DeleteAsync(id_appointment);
        return Ok(ApiResponse<string>.Ok("Cita eliminada exitosamente"));
    }
}