using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Clinics")]
[Authorize]
public class ClinicController : ControllerBase
{
    private readonly IClinicService _clinicService;

    public ClinicController(IClinicService clinicService) =>
        _clinicService = clinicService;

    // GET api/v1/clinics
    [HttpGet]
    [Authorize(Roles = "admin,veterinario,cliente")]
    public async Task<IActionResult> GetAll()
    {
        var clinics = await _clinicService.GetAllAsync();
        return Ok(ApiResponse<List<ClinicDto>>.Ok(clinics));
    }

    // GET api/v1/clinics/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "admin,veterinario,cliente")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var clinic = await _clinicService.GetByIdAsync(id);
        return Ok(ApiResponse<ClinicDto>.Ok(clinic));
    }

    // POST api/v1/clinics
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateClinicDto dto)
    {
        var clinic = await _clinicService.CreateAsync(dto);
        return Ok(ApiResponse<ClinicDto>.Ok(clinic, "Clínica creada exitosamente"));
    }

    // PUT api/v1/clinics/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateClinicDto dto)
    {
        var clinic = await _clinicService.UpdateAsync(id, dto);
        return Ok(ApiResponse<ClinicDto>.Ok(clinic, "Clínica actualizada exitosamente"));
    }

    // DELETE api/v1/clinics/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _clinicService.DeleteAsync(id);
        return Ok(ApiResponse<string>.Ok("Clínica eliminada exitosamente"));
    }
}