using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
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
    public AppointmentController(IAppointmentService appointmentService) => _appointmentService = appointmentService;

    // GET api/v1/citas
    // Solo admin ve todas las citas
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _appointmentService.GetAllAsync();
        return Ok(ApiResponse<List<AppointmentDto>>.Ok(appointments));
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
    // El cliente ve sus propias citas
    [HttpGet("mias")]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> GetMyAppointments()
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var appointments = await _appointmentService.GetByUserAsync(id_user);
        return Ok(ApiResponse<List<AppointmentDto>>.Ok(appointments));
    }

    // GET api/v1/citas/mis-pacientes
    // El veterinario ve sus citas asignadas
    [HttpGet("my-patients")]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> GetMyPatients()
    {
        var id_veterinarian = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var appointments = await _appointmentService.GetByVeterinarinarianAsync(id_veterinarian);
        return Ok(ApiResponse<List<AppointmentDto>>.Ok(appointments));
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
    //PATCH api/api/citas/{id} - solo pendientes
    [HttpPut("{id_appointment}")]
    [Authorize(Roles = "cliente, admin")]
    public async Task<IActionResult> Update(Guid id_appointment, [FromBody] UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentService.UpdateAsync(id_appointment, dto);
        return Ok(ApiResponse<AppointmentDto>.Ok(appointment, "Cita actualizada exitosamente"));
    }

    // DELETE api/v1/Appointments/{id}
    // Admin: elimina físicamente | Cliente: solo puede cancelar (PATCH status)
    [HttpDelete("{id_appointment}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id_appointment)
    {
        await _appointmentService.DeleteAsync(id_appointment);
        return Ok(ApiResponse<string>.Ok("Cita eliminada exitosamente"));
    }



}