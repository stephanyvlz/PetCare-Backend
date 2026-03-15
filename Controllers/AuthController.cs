using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var mensaje = await _authService.RegisterAsync(dto);
        return Ok(ApiResponse<string>.Ok(mensaje));
    }

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    var response = await _authService.LoginAsync(dto);

    return Ok(
        ApiResponse<LoginResponseDto>.Ok(
            response,
            "Login exitoso"
        )
    );
}
}