using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Donations")]
public class DonationController : ControllerBase
{
    private readonly IDonationService _donationService;
    public DonationController(IDonationService donationService) => _donationService = donationService;

    // POST api/v1/Donations/crear-orden
    [HttpPost("crear-orden")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOrder([FromBody] CreateDonationDto dto)
    {
        try
        {
            var donation = await _donationService.CreateOrderAsync(dto);
            return Ok(ApiResponse<DonationDto>.Ok(donation, "Orden de donación creada exitosamente"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    // POST api/v1/Donations/capturar-orden
    [HttpPost("capturar-orden")]
    [AllowAnonymous]
    public async Task<IActionResult> CaptureOrder([FromBody] CaptureDonationDto dto)
    {
        try
        {
            var donation = await _donationService.CaptureOrderAsync(dto.paypal_order_id);
            return Ok(ApiResponse<DonationDto>.Ok(donation, "Donación completada exitosamente"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    // GET api/v1/Donations
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var donations = await _donationService.GetAllAsync();
        return Ok(ApiResponse<List<DonationDto>>.Ok(donations));
    }
}