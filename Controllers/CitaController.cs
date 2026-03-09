using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;
using System.Security.Claims;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/citas")]
[Authorize]
public class CitaController : ControllerBase
{
    private readonly ICitaService _citaService;
    public CitaController(ICitaService citaService) => _citaService = citaService;

    // GET api/v1/citas
    // Solo admin ve todas las citas
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var citas = await _citaService.GetAllAsync();
        return Ok(ApiResponse<List<CitaDto>>.Ok(citas));
    }

    // GET api/v1/citas/mias
    // El cliente ve sus propias citas
    [HttpGet("mias")]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> GetMisCitas()
    {
        var idUsuario = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var citas = await _citaService.GetByUsuarioAsync(idUsuario);
        return Ok(ApiResponse<List<CitaDto>>.Ok(citas));
    }

    // GET api/v1/citas/mis-pacientes
    // El veterinario ve sus citas asignadas
    [HttpGet("mis-pacientes")]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> GetMisPacientes()
    {
        var idVeterinario = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var citas = await _citaService.GetByVeterinarioAsync(idVeterinario);
        return Ok(ApiResponse<List<CitaDto>>.Ok(citas));
    }

    // POST api/v1/citas
    [HttpPost]
    [Authorize(Roles = "cliente,admin")]
    public async Task<IActionResult> Create([FromBody] CreateCitaDto dto)
    {
        var cita = await _citaService.CreateAsync(dto);
        return Ok(ApiResponse<CitaDto>.Ok(cita, "Cita agendada exitosamente"));
    }

    // PATCH api/v1/citas/{idCita}/estado
    [HttpPatch("{idCita}/estado")]
    [Authorize(Roles = "veterinario,admin")]
    public async Task<IActionResult> CambiarEstado(Guid idCita, [FromBody] CambiarEstadoDto dto)
    {
        var cita = await _citaService.CambiarEstadoAsync(idCita, dto.Estado);
        return Ok(ApiResponse<CitaDto>.Ok(cita, "Estado actualizado"));
    }
}