namespace PetCare.API.Models.DTOs;

public record CreateDonationDto(
    decimal amount,
    string donor_name,
    string? donor_email,
    string? message
);

public record CaptureDonationDto(
    string paypal_order_id
);

public record DonationDto(
    Guid id_donation,
    string paypal_order_id,
    string approval_url,
    decimal amount,
    string status,
    string donor_name,
    string? donor_email,
    string? message,
    DateTime created_at
);