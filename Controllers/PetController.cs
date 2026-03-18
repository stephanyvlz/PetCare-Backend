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
[Route("api/v{version:apiVersion}/Pets")]
[Authorize]
public class PetController : ControllerBase
{
    private readonly IPetService _petService;
    public PetController(IPetService petService) => _petService = petService;

    // GET api/v1/Pets — solo admin ve todas las mascotas
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var pets = await _petService.GetAllAsync();
        return Ok(ApiResponse<List<PetDto>>.Ok(pets));
    }
    // GET api/v1/Pets/My-Pets — cliente ve sus mascotas
    [HttpGet("My-Pets")]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> GetMyPets()
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pets = await _petService.GetByUserAsync(id_user);
        return Ok(ApiResponse<List<PetDto>>.Ok(pets));
    }

    // GET api/v1/Pets/user/{id_user} — admin ve mascotas de cualquier usuario
    [HttpGet("user/{id_user}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetByUser(Guid id_user)
    {
        var pets = await _petService.GetByUserAsync(id_user);
        return Ok(ApiResponse<List<PetDto>>.Ok(pets));
    }

    // GET api/v1/Pets/{id_pet}
    [HttpGet("{id_pet}")]
    [Authorize(Roles = "admin,cliente,veterinario")]
    public async Task<IActionResult> GetById(Guid id_pet)
    {
        var pet = await _petService.GetByIdAsync(id_pet);
        return Ok(ApiResponse<PetDto>.Ok(pet));
    }

    // POST api/v1/Pets
    [HttpPost]
    [Authorize(Roles = "cliente")]
    public async Task<IActionResult> Create([FromBody] CreatePetDto dto)
    {
        var id_user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pet = await _petService.CreateAsync(id_user, dto);
        return Ok(ApiResponse<PetDto>.Ok(pet, "Mascota registrada exitosamente"));
    }

    // PUT api/v1/Pets/{id_pet}
    [HttpPut("{id_pet}")]
    [Authorize(Roles = "cliente,admin")]
    public async Task<IActionResult> Update(Guid id_pet, [FromBody] UpdatePetDto dto)
    {
        var pet = await _petService.UpdateAsync(id_pet, dto);
        return Ok(ApiResponse<PetDto>.Ok(pet, "Mascota actualizada exitosamente"));
    }

    // DELETE api/v1/Pets/{id_pet}
    [HttpDelete("{id_pet}")]
    [Authorize(Roles = "admin,cliente")]
    public async Task<IActionResult> Delete(Guid id_pet)
    {
        await _petService.DeleteAsync(id_pet);
        return Ok(ApiResponse<string>.Ok("Mascota eliminada exitosamente"));
    }
}