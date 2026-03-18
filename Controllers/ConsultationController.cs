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
    public ConsultationController(IConsultationService consultationService)
        => _consultationService = consultationService;

    // GET api/v1/Consultations/{id}
    [HttpGet("{id_consultation}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetById(Guid id_consultation)
    {
        var consultation = await _consultationService.GetByIdAsync(id_consultation);
        return Ok(ApiResponse<ConsultationDto>.Ok(consultation));
    }

    // GET api/v1/Consultations — solo admin
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var consultations = await _consultationService.GetAllAsync();
        return Ok(ApiResponse<List<ConsultationDto>>.Ok(consultations));
    }

    // GET api/v1/Consultations/appointment/{id_appointment} 
    [HttpGet("appointment/{id_appointment}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetByAppointment(Guid id_appointment)
    {
        var consultation = await _consultationService.GetByAppointmentAsync(id_appointment);
        if (consultation is null)
            return NotFound(ApiResponse<string>.Fail("No hay consulta para esta cita"));

        return Ok(ApiResponse<ConsultationDto>.Ok(consultation));
    }

    // POST api/v1/Consultations — solo veterinario
    [HttpPost]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Create([FromBody] CreateConsultationDto dto)
    {
        var consultation = await _consultationService.CreateAsync(dto);
        return Ok(ApiResponse<ConsultationDto>.Ok(consultation, "Consulta registrada exitosamente"));
    }

    // PUT api/v1/Consultations/{id} — solo veterinario
    [HttpPut("{id_consultation}")]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Update(Guid id_consultation, [FromBody] UpdateConsultationDto dto)
    {
        var consultation = await _consultationService.UpdateAsync(id_consultation, dto);
        return Ok(ApiResponse<ConsultationDto>.Ok(consultation, "Consulta actualizada exitosamente"));
    }

    // DELETE api/v1/Consultations/{id} — solo admin
    [HttpDelete("{id_consultation}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id_consultation)
    {
        await _consultationService.DeleteAsync(id_consultation);
        return Ok(ApiResponse<string>.Ok("Consulta eliminada exitosamente"));
    }
}