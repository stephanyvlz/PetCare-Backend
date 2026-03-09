using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;
using System.Security.Claims;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    public UsuarioController(IUsuarioService usuarioService) =>
        _usuarioService = usuarioService;

    // GET api/v1/usuarios
    // Solo admin ve todos los usuarios
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();
        return Ok(ApiResponse<List<UsuarioDto>>.Ok(usuarios));
    }

    // GET api/v1/usuarios/veterinarios
    // Útil para que el cliente elija veterinario al agendar cita
    [HttpGet("veterinarios")]
    [Authorize(Roles = "admin,cliente")]
    public async Task<IActionResult> GetVeterinarios()
    {
        var veterinarios = await _usuarioService.GetVeterinariosAsync();
        return Ok(ApiResponse<List<UsuarioDto>>.Ok(veterinarios));
    }

    // GET api/v1/usuarios/perfil
    // Cualquier usuario ve su propio perfil
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var idUsuario = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var usuario = await _usuarioService.GetByIdAsync(idUsuario);
        return Ok(ApiResponse<UsuarioDto>.Ok(usuario));
    }

    // GET api/v1/usuarios/{id}
    // Solo admin puede ver el perfil de otro usuario
    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        return Ok(ApiResponse<UsuarioDto>.Ok(usuario));
    }

    // PUT api/v1/usuarios/perfil
    // Cualquier usuario puede editar su propio perfil
    [HttpPut("perfil")]
    public async Task<IActionResult> UpdatePerfil([FromBody] UpdateUsuarioDto dto)
    {
        var idUsuario = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var usuario = await _usuarioService.UpdateAsync(idUsuario, dto);
        return Ok(ApiResponse<UsuarioDto>.Ok(usuario, "Perfil actualizado exitosamente"));
    }

    // DELETE api/v1/usuarios/{id}
    // Solo admin puede eliminar usuarios
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _usuarioService.DeleteAsync(id);
        return Ok(ApiResponse<string>.Ok("Usuario eliminado exitosamente"));
    }
}