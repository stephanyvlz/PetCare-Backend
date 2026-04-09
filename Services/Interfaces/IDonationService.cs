using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IDonationService
{
    Task<DonationDto> CreateOrderAsync(CreateDonationDto dto);
    Task<DonationDto> CaptureOrderAsync(string paypal_order_id);
    Task<List<DonationDto>> GetAllAsync();
}