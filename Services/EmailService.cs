// PetCare.API/Services/EmailService.cs
using SendGrid;
using SendGrid.Helpers.Mail;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class EmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly ILogger<EmailService> _logger;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") 
                  ?? configuration["SendGrid:ApiKey"];
        _logger = logger;
        
        // ✅ Asignar el remitente temporal de SendGrid (NO necesita verificación)
        _fromEmail = "sg7f3a421cc144500c8e15e4f2eb4935fa@sendgrid.net";
        _fromName = "PetCare Veterinaria";
        
        if (string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogWarning("⚠️ SENDGRID_API_KEY no está configurada");
        }
        else
        {
            _logger.LogInformation("✅ SendGrid configurado correctamente. Remitente: {FromEmail}", _fromEmail);
        }
    }

    public async Task<bool> SendAppointmentConfirmationAsync(
        string toEmail, 
        string userName, 
        DateTime appointmentDate, 
        string clinicName,
        string serviceName,
        string veterinarianName)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogWarning("No se puede enviar email: API Key no configurada");
            return false;
        }

        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var subject = "✅ Tu cita ha sido confirmada - PetCare";
            var to = new EmailAddress(toEmail, userName);
            
            var htmlContent = $@"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'></head>
<body style='font-family: Arial, sans-serif; line-height: 1.6;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <div style='background: #5C7A4E; color: white; padding: 20px; text-align: center; border-radius: 10px;'>
            <h2>🐾 PetCare Veterinaria</h2>
        </div>
        <div style='padding: 20px; background: #f9f9f9;'>
            <h3>¡Hola {userName}!</h3>
            <p>Tu cita ha sido <strong>confirmada exitosamente</strong>.</p>
            
            <div style='background: white; padding: 15px; border-radius: 8px; margin: 15px 0;'>
                <p>📅 <strong>Fecha:</strong> {appointmentDate:dddd, dd/MM/yyyy}</p>
                <p>🕐 <strong>Hora:</strong> {appointmentDate:HH:mm} hrs</p>
                <p>🏥 <strong>Clínica:</strong> {clinicName}</p>
                <p>🩺 <strong>Servicio:</strong> {serviceName}</p>
                <p>👨‍⚕️ <strong>Veterinario:</strong> {veterinarianName}</p>
            </div>
            
            <p style='text-align: center; margin-top: 20px;'>
                ¡Te esperamos puntual!
            </p>
        </div>
        <div style='background: #f0f0f0; padding: 15px; text-align: center; font-size: 12px; border-radius: 10px;'>
            <p>PetCare Veterinaria - Cuidando a tu mejor amigo</p>
        </div>
    </div>
</body>
</html>";
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                _logger.LogInformation("✅ Email enviado exitosamente a {Email}", toEmail);
                return true;
            }
            
            var errorBody = await response.Body.ReadAsStringAsync();
            _logger.LogWarning("❌ Error enviando email a {Email}. Status: {StatusCode}. Error: {Error}", 
                toEmail, response.StatusCode, errorBody);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al enviar email de confirmación a {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendAppointmentCancellationAsync(
        string toEmail, 
        string userName, 
        DateTime appointmentDate, 
        string clinicName)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogWarning("No se puede enviar email: API Key no configurada");
            return false;
        }

        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var subject = "❌ Tu cita ha sido cancelada - PetCare";
            var to = new EmailAddress(toEmail, userName);
            
            var htmlContent = $@"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'></head>
<body style='font-family: Arial, sans-serif;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <div style='background: #ef4444; color: white; padding: 20px; text-align: center; border-radius: 10px;'>
            <h2>🐾 PetCare Veterinaria</h2>
        </div>
        <div style='padding: 20px;'>
            <h3>Hola {userName}</h3>
            <p>Tu cita ha sido <strong>cancelada</strong>.</p>
            <div style='background: #f3f4f6; padding: 15px; border-radius: 8px;'>
                <p>📅 <strong>Fecha:</strong> {appointmentDate:dddd, dd/MM/yyyy}</p>
                <p>🕐 <strong>Hora:</strong> {appointmentDate:HH:mm} hrs</p>
                <p>🏥 <strong>Clínica:</strong> {clinicName}</p>
            </div>
            <p>Si no solicitaste esta cancelación, por favor contáctanos.</p>
        </div>
    </div>
</body>
</html>";
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                _logger.LogInformation("✅ Email de cancelación enviado a {Email}", toEmail);
                return true;
            }
            
            var errorBody = await response.Body.ReadAsStringAsync();
            _logger.LogWarning("❌ Error enviando email de cancelación a {Email}. Status: {StatusCode}. Error: {Error}", 
                toEmail, response.StatusCode, errorBody);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al enviar email de cancelación a {Email}", toEmail);
            return false;
        }
    }
}