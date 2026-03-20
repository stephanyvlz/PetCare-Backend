using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Treatments")]
[Authorize]
public class TreatmentController : ControllerBase
{
    private readonly ITreatmentService _treatmentService;
    public TreatmentController(ITreatmentService treatmentService) =>
        _treatmentService = treatmentService;

    // GET api/v1/Treatments — solo admin
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var treatments = await _treatmentService.GetAllAsync();
        return Ok(ApiResponse<List<TreatmentDto>>.Ok(treatments));
    }

    // GET api/v1/Treatments/{id}
    [HttpGet("{treatment_id}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetById(Guid treatment_id)
    {
        var treatment = await _treatmentService.GetByIdAsync(treatment_id);
        return Ok(ApiResponse<TreatmentDto>.Ok(treatment));
    }

    // GET api/v1/Treatments/consultation/{id_consultation}
    [HttpGet("consultation/{id_consultation}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetByConsultation(Guid id_consultation)
    {
        var treatments = await _treatmentService.GetByConsultationAsync(id_consultation);
        return Ok(ApiResponse<List<TreatmentDto>>.Ok(treatments));
    }

    // POST api/v1/Treatments — solo veterinario
    [HttpPost]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Create([FromBody] CreateTreatmentDto dto)
    {
        var treatment = await _treatmentService.CreateAsync(dto);
        return Ok(ApiResponse<TreatmentDto>.Ok(treatment, "Tratamiento registrado exitosamente"));
    }

    // PUT api/v1/Treatments/{id} — solo veterinario
    [HttpPut("{treatment_id}")]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Update(Guid treatment_id, [FromBody] UpdateTreatmentDto dto)
    {
        var treatment = await _treatmentService.UpdateAsync(treatment_id, dto);
        return Ok(ApiResponse<TreatmentDto>.Ok(treatment, "Tratamiento actualizado exitosamente"));
    }

    // DELETE api/v1/Treatments/{id} — veterinario o admin
    [HttpDelete("{treatment_id}")]
    [Authorize(Roles = "veterinario,admin")]
    public async Task<IActionResult> Delete(Guid treatment_id)
    {
        await _treatmentService.DeleteAsync(treatment_id);
        return Ok(ApiResponse<string>.Ok("Tratamiento eliminado exitosamente"));
    }
}