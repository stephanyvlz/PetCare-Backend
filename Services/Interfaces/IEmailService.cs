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
}