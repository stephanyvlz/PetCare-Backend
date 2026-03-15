using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
}