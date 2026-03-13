using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;
using System.Security.Claims;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/Appointments")]
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
    public async Task<IActionResult> ChangeStatus(Guid id_appointment, [FromBody] AppointmentDto dto)//ChangeStatusDto dto)
    {
        var appointment = await _appointmentService.ChangeStatusAsync(id_appointment, dto.status);
        return Ok(ApiResponse<AppointmentDto>.Ok(appointment, "Estado actualizado"));
    }
}