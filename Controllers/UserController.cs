using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;
using System.Security.Claims;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) =>
        _userService = userService;

    // GET api/v1/usuarios
    // Solo admin ve todos los usuarios
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(ApiResponse<List<UserDto>>.Ok(users));
    }

    // GET api/v1/usuarios/veterinarios
    // Útil para que el cliente elija veterinario al agendar cita
    [HttpGet("veterinarians")]
    [Authorize(Roles = "admin,cliente")]
    public async Task<IActionResult> GetVeterinarios()
    {
        var veterinarians = await _userService.GetVeterinarianAsync();
        return Ok(ApiResponse<List<UserDto>>.Ok(veterinarians));
    }

    // GET api/v1/usuarios/perfil
    // Cualquier usuario ve su propio perfil
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetByIdAsync(id_user);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    // GET api/v1/usuarios/{id}
    // Solo admin puede ver el perfil de otro usuario
    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    // PUT api/v1/usuarios/perfil
    // Cualquier usuario puede editar su propio perfil
    [HttpPut("perfil")]
    public async Task<IActionResult> UpdatePerfil([FromBody] UpdateUserDto dto)
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.UpdateAsync(id_user, dto);
        return Ok(ApiResponse<UserDto>.Ok(user, "Perfil actualizado exitosamente"));
    }

    // DELETE api/v1/usuarios/{id}
    // Solo admin puede eliminar usuarios
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return Ok(ApiResponse<string>.Ok("Usuario eliminado exitosamente"));
    }
}