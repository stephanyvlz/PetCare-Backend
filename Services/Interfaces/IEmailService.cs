namespace PetCare.API.Services.Interfaces;

public interface IEmailService
{
    Task SendAppointmentConfirmedAsync(
        string toEmail,
        string clientName,
        string petName,
        string veterinarianName,
        string clinicName,
        DateTime appointmentDate,
        string service
    );
    Task SendPasswordResetAsync(
    string toEmail,
    string userName,
    string resetLink
);
}