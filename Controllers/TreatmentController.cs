using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/treatments")]
[Authorize]
public class TreatmentController : ControllerBase
{
    private readonly ITreatmentService _treatmentService;
    public TreatmentController(ITreatmentService treatmentService) =>
        _treatmentService = treatmentService;

    // GET api/v1/tratamientos/consulta/{idConsulta}
    [HttpGet("consultation/{id_consultation}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetByConsultation(Guid id_consultation)
    {
        var treatments = await _treatmentService.GetByConsultationAsync(id_consultation);
        return Ok(ApiResponse<List<TreatmentDto>>.Ok(treatments));
    }

    // POST api/v1/tratamientos
    // Solo el veterinario puede agregar tratamientos
    [HttpPost]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Create([FromBody] CreateTreatmentDto dto)
    {
        var treatment = await _treatmentService.CreateAsync(dto);
        return Ok(ApiResponse<TreatmentDto>.Ok(treatment, "Tratamiento registrado exitosamente"));
    }
}