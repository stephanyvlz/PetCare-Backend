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
public class PetController : ControllerBase
{
    private readonly IPetService _petService;
    public PetController(IPetService petService) => _petService = petService;

    // GET api/v1/mascotas/mias
    // El cliente ve solo sus mascotas
    [HttpGet("My-Pets")]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> GetMyPets()
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pets = await _petService.GetByPetAsync(id_user);
        return Ok(ApiResponse<List<PetDto>>.Ok(pets));
    }

    // GET api/v1/mascotas/usuario/{idUsuario}
    // El admin ve mascotas de cualquier usuario
    [HttpGet("user/{id_user}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetByUser(Guid id_user)
    {
        var pets = await _petService.GetByUserAsync(id_user);
        return Ok(ApiResponse<List<PetDto>>.Ok(pets));
    }

    // POST api/v1/mascotas
    [HttpPost]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> Create([FromBody] CreatePetDto dto)
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pet = await _petService.CreateAsync(id_user, dto);
        return Ok(ApiResponse<PetDto>.Ok(pet, "Mascota registrada exitosamente"));
    }

    // DELETE api/v1/mascotas/{idMascota}
    [HttpDelete("{id_pet}")]
    [Authorize(Roles = "admin,cliente")]
    public async Task<IActionResult> Delete(Guid id_pet)
    {
        await _petService.DeleteAsync(id_pet);
        return Ok(ApiResponse<string>.Ok("Mascota eliminada exitosamente"));
    }
}