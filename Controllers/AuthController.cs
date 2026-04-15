using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Responses;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
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

    [HttpPost("request-reset")]
    public async Task<IActionResult> RequestReset([FromBody] string email)
    {
        await _authService.RequestPasswordResetAsync(email);
        return Ok("Si el correo existe, se enviaron instrucciones");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);
        return Ok("Contraseña actualizada");
    }
}
