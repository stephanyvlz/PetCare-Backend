using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;
using System.Security.Claims;

namespace PetCare.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Users")]
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


    // PUT api/v1/Users/perfil — cualquier usuario edita solo nombre y teléfono
    [HttpPut("perfil"!)]
    [Authorize(Roles = "admin,cliente,veterinario")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.UpdateProfileAsync(id_user, dto);
        return Ok(ApiResponse<UserDto>.Ok(user, "Perfil actualizado exitosamente"));
    }
    // PUT api/v1/Users/{id} — solo admin puede editar a otros usuarios y asignar clínica
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AdminUpdate(Guid id, [FromBody] AdminUpdateUserDto dto)
    {
        var user = await _userService.AdminUpdateAsync(id, dto);
        return Ok(ApiResponse<UserDto>.Ok(user, "Usuario actualizado exitosamente"));
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