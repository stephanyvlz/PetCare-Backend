using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config) => _config = config;

    public async Task SendAppointmentConfirmedAsync(
        string toEmail,
        string clientName,
        string petName,
        string veterinarianName,
        string clinicName,
        DateTime appointmentDate,
        string service)
    {
        var smtpHost     = _config["Email:SmtpHost"]!;
        var smtpPort     = int.Parse(_config["Email:SmtpPort"]!);
        var fromEmail    = _config["Email:FromEmail"]!;
        var fromName     = _config["Email:FromName"]!;
        var appPassword  = _config["Email:AppPassword"]!;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress(clientName, toEmail));
        message.Subject = $"✅ Cita confirmada — {petName}";

        var fecha = appointmentDate.ToString("dddd d 'de' MMMM yyyy",
                        new System.Globalization.CultureInfo("es-MX"));
        var hora  = appointmentDate.ToString("HH:mm");

        message.Body = new TextPart("html")
        {
            Text = $"""
            <!DOCTYPE html>
            <html lang="es">
            <head><meta charset="UTF-8"></head>
            <body style="margin:0;padding:0;background:#FDF6EE;font-family:'Segoe UI',sans-serif;">
              <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                  <td align="center" style="padding:32px 16px;">
                    <table width="560" cellpadding="0" cellspacing="0"
                           style="background:#fff;border-radius:16px;overflow:hidden;
                                  box-shadow:0 4px 24px rgba(0,0,0,.08);">

                      <!-- Header -->
                      <tr>
                        <td style="background:#5C7A4E;padding:28px 32px;text-align:center;">
                          <p style="margin:0;font-size:28px;">🐾</p>
                          <h1 style="margin:8px 0 0;color:#fff;font-size:22px;font-weight:700;">
                            ¡Tu cita ha sido confirmada!
                          </h1>
                        </td>
                      </tr>

                      <!-- Body -->
                      <tr>
                        <td style="padding:32px;">
                          <p style="margin:0 0 20px;font-size:15px;color:#4A3728;">
                            Hola <strong>{clientName}</strong>, te informamos que la cita
                            de <strong>{petName}</strong> ha sido <strong>confirmada</strong>.
                          </p>

                          <!-- Info card -->
                          <table width="100%" cellpadding="0" cellspacing="0"
                                 style="background:#FDF6EE;border-radius:12px;
                                        border:1px solid #E8D5B7;">
                            <tr>
                              <td style="padding:20px 24px;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                  <tr>
                                    <td style="padding:6px 0;font-size:13px;color:#9E9E9E;width:40%;">🐾 Mascota</td>
                                    <td style="padding:6px 0;font-size:14px;font-weight:600;color:#4A3728;">{petName}</td>
                                  </tr>
                                  <tr>
                                    <td style="padding:6px 0;font-size:13px;color:#9E9E9E;">🩺 Veterinario</td>
                                    <td style="padding:6px 0;font-size:14px;font-weight:600;color:#4A3728;">{veterinarianName}</td>
                                  </tr>
                                  <tr>
                                    <td style="padding:6px 0;font-size:13px;color:#9E9E9E;">🏥 Clínica</td>
                                    <td style="padding:6px 0;font-size:14px;font-weight:600;color:#4A3728;">{clinicName}</td>
                                  </tr>
                                  <tr>
                                    <td style="padding:6px 0;font-size:13px;color:#9E9E9E;">📋 Servicio</td>
                                    <td style="padding:6px 0;font-size:14px;font-weight:600;color:#4A3728;">{service}</td>
                                  </tr>
                                  <tr>
                                    <td style="padding:6px 0;font-size:13px;color:#9E9E9E;">📅 Fecha</td>
                                    <td style="padding:6px 0;font-size:14px;font-weight:600;color:#4A3728;">{fecha}</td>
                                  </tr>
                                  <tr>
                                    <td style="padding:6px 0;font-size:13px;color:#9E9E9E;">🕐 Hora</td>
                                    <td style="padding:6px 0;font-size:14px;font-weight:600;color:#4A3728;">{hora} hrs</td>
                                  </tr>
                                </table>
                              </td>
                            </tr>
                          </table>

                          <p style="margin:24px 0 0;font-size:13px;color:#9E9E9E;text-align:center;">
                            Si tienes alguna duda, no dudes en contactarnos.
                          </p>
                        </td>
                      </tr>

                      <!-- Footer -->
                      <tr>
                        <td style="background:#F5F5F5;padding:16px 32px;text-align:center;">
                          <p style="margin:0;font-size:12px;color:#9E9E9E;">
                            PetCare · Cuidamos a quienes más quieres 🐾
                          </p>
                        </td>
                      </tr>

                    </table>
                  </td>
                </tr>
              </table>
            </body>
            </html>
            """
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(fromEmail, appPassword);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }



    public async Task SendPasswordResetAsync(
    string toEmail,
    string userName,
    string resetLink)
{
    var smtpHost     = _config["Email:SmtpHost"]!;
    var smtpPort     = int.Parse(_config["Email:SmtpPort"]!);
    var fromEmail    = _config["Email:FromEmail"]!;
    var fromName     = _config["Email:FromName"]!;
    var appPassword  = _config["Email:AppPassword"]!;

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress(fromName, fromEmail));
    message.To.Add(new MailboxAddress(userName, toEmail));
    message.Subject = "🔐 Restablece tu contraseña";

    message.Body = new TextPart("html")
    {
        Text = $"""
        <!DOCTYPE html>
        <html lang="es">
        <head><meta charset="UTF-8"></head>
        <body style="margin:0;padding:0;background:#FDF6EE;font-family:'Segoe UI',sans-serif;">
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
              <td align="center" style="padding:32px 16px;">
                <table width="560" cellpadding="0" cellspacing="0"
                       style="background:#fff;border-radius:16px;overflow:hidden;
                              box-shadow:0 4px 24px rgba(0,0,0,.08);">

                  <!-- Header -->
                  <tr>
                    <td style="background:#C0392B;padding:28px 32px;text-align:center;">
                      <h1 style="margin:0;color:#fff;font-size:22px;">
                        Restablecer contraseña
                      </h1>
                    </td>
                  </tr>

                  <!-- Body -->
                  <tr>
                    <td style="padding:32px;">
                      <p style="font-size:15px;color:#4A3728;">
                        Hola <strong>{userName}</strong>,
                      </p>

                      <p style="font-size:14px;color:#4A3728;">
                        Recibimos una solicitud para restablecer tu contraseña.
                      </p>

                      <p style="font-size:14px;color:#4A3728;">
                        Haz clic en el siguiente botón:
                      </p>

                      <div style="text-align:center;margin:24px 0;">
                        <a href="{resetLink}"
                           style="background:#5C7A4E;color:#fff;padding:12px 24px;
                                  border-radius:8px;text-decoration:none;
                                  font-weight:600;">
                          Restablecer contraseña
                        </a>
                      </div>

                      <p style="font-size:12px;color:#9E9E9E;">
                        Este enlace expirará en 30 minutos.
                      </p>

                      <p style="font-size:12px;color:#9E9E9E;">
                        Si no solicitaste esto, puedes ignorar este mensaje.
                      </p>
                    </td>
                  </tr>

                  <!-- Footer -->
                  <tr>
                    <td style="background:#F5F5F5;padding:16px;text-align:center;">
                      <p style="font-size:12px;color:#9E9E9E;">
                        PetCare 🐾
                      </p>
                    </td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """
    };

    using var smtp = new SmtpClient();
    await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
    await smtp.AuthenticateAsync(fromEmail, appPassword);
    await smtp.SendAsync(message);
    await smtp.DisconnectAsync(true);
}
}