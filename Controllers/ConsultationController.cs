using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Consultations")]
[Authorize]
public class ConsultationController : ControllerBase
{
    private readonly IConsultationService _consultationService;
    public ConsultationController(IConsultationService consultationService) => _consultationService = consultationService;

    // GET api/v1/consultas/cita/{idCita}
    [HttpGet("Appoinment/{id_Appointment}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetByAppointment(Guid id_appointment)
    {
        var consultation = await _consultationService.GetByAppointmentAsync(id_appointment);
        if (consultation is null)
            return NotFound(ApiResponse<string>.Fail("No hay consulta para esta cita"));

        return Ok(ApiResponse<ConsultationDto>.Ok(consultation));
    }

    // POST api/v1/consultas
    // Solo el veterinario puede crear una consulta
    [HttpPost]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Create([FromBody] CreateConsultationDto dto)
    {
        var consultation = await _consultationService.CreateAsync(dto);
        return Ok(ApiResponse<ConsultationDto>.Ok(consultation, "Consulta registrada exitosamente"));
    }
}