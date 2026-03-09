using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;
using System.Security.Claims;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/mascotas")]
[Authorize]
public class MascotaController : ControllerBase
{
    private readonly IMascotaService _mascotaService;
    public MascotaController(IMascotaService mascotaService) => _mascotaService = mascotaService;

    // GET api/v1/mascotas/mias
    // El cliente ve solo sus mascotas
    [HttpGet("mias")]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> GetMisMascotas()
    {
        var idUsuario = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var mascotas = await _mascotaService.GetByUsuarioAsync(idUsuario);
        return Ok(ApiResponse<List<MascotaDto>>.Ok(mascotas));
    }

    // GET api/v1/mascotas/usuario/{idUsuario}
    // El admin ve mascotas de cualquier usuario
    [HttpGet("usuario/{idUsuario}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetByUsuario(Guid idUsuario)
    {
        var mascotas = await _mascotaService.GetByUsuarioAsync(idUsuario);
        return Ok(ApiResponse<List<MascotaDto>>.Ok(mascotas));
    }

    // POST api/v1/mascotas
    [HttpPost]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> Create([FromBody] CreateMascotaDto dto)
    {
        var idUsuario = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var mascota = await _mascotaService.CreateAsync(idUsuario, dto);
        return Ok(ApiResponse<MascotaDto>.Ok(mascota, "Mascota registrada exitosamente"));
    }

    // DELETE api/v1/mascotas/{idMascota}
    [HttpDelete("{idMascota}")]
    [Authorize(Roles = "admin,cliente")]
    public async Task<IActionResult> Delete(Guid idMascota)
    {
        await _mascotaService.DeleteAsync(idMascota);
        return Ok(ApiResponse<string>.Ok("Mascota eliminada exitosamente"));
    }
}