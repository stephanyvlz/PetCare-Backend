using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/tratamientos")]
[Authorize]
public class TratamientoController : ControllerBase
{
    private readonly ITratamientoService _tratamientoService;
    public TratamientoController(ITratamientoService tratamientoService) =>
        _tratamientoService = tratamientoService;

    // GET api/v1/tratamientos/consulta/{idConsulta}
    [HttpGet("consulta/{idConsulta}")]
    [Authorize(Roles = "veterinario,admin,cliente")]
    public async Task<IActionResult> GetByConsulta(Guid idConsulta)
    {
        var tratamientos = await _tratamientoService.GetByConsultaAsync(idConsulta);
        return Ok(ApiResponse<List<TratamientoDto>>.Ok(tratamientos));
    }

    // POST api/v1/tratamientos
    // Solo el veterinario puede agregar tratamientos
    [HttpPost]
    [Authorize(Roles = "veterinario")]
    public async Task<IActionResult> Create([FromBody] CreateTratamientoDto dto)
    {
        var tratamiento = await _tratamientoService.CreateAsync(dto);
        return Ok(ApiResponse<TratamientoDto>.Ok(tratamiento, "Tratamiento registrado exitosamente"));
    }
}