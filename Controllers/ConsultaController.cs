using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/consultas")]
[Authorize]
public class ConsultaController : ControllerBase
{
    private readonly IConsultaService _consultaService;
    public ConsultaController(IConsultaService consultaService) => _consultaService = consultaService;

    // GET api/v1/consultas/cita/{idCita}
    [HttpGet("cita/{idCita}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetByCita(Guid idCita)
    {
        var consulta = await _consultaService.GetByCitaAsync(idCita);
        if (consulta is null)
            return NotFound(ApiResponse<string>.Fail("No hay consulta para esta cita"));

        return Ok(ApiResponse<ConsultaDto>.Ok(consulta));
    }

    // POST api/v1/consultas
    // Solo el veterinario puede crear una consulta
    [HttpPost]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Create([FromBody] CreateConsultaDto dto)
    {
        var consulta = await _consultaService.CreateAsync(dto);
        return Ok(ApiResponse<ConsultaDto>.Ok(consulta, "Consulta registrada exitosamente"));
    }
}