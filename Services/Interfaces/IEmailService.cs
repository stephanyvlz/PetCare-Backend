// PetCare.API/Services/IEmailService.cs
namespace PetCare.API.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendAppointmentConfirmationAsync(
        string toEmail, 
        string userName, 
        DateTime appointmentDate, 
        string clinicName,
        string serviceName,
        string veterinarianName);
    
    Task<bool> SendAppointmentCancellationAsync(
        string toEmail, 
        string userName, 
        DateTime appointmentDate, 
        string clinicName);
}