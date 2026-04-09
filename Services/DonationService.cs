using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class DonationService : IDonationService
{
    private readonly IDonationRepository _repo;
    private readonly IConfiguration _config;
    private readonly HttpClient _http;

    // URLs de PayPal Sandbox
    private const string PAYPAL_BASE = "https://api-m.sandbox.paypal.com";

    public DonationService(IDonationRepository repo, IConfiguration config, HttpClient http)
    {
        _repo = repo;
        _config = config;
        _http = http;
    }

    // ─── Paso 1: Obtener token de acceso de PayPal ───────────────────────
    private async Task<string> GetAccessTokenAsync()
    {
        var clientId = _config["PayPal:ClientId"];
        var clientSecret = _config["PayPal:ClientSecret"];

        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        var request = new HttpRequestMessage(HttpMethod.Post, $"{PAYPAL_BASE}/v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        request.Content = new StringContent("grant_type=client_credentials",
            Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _http.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("access_token").GetString()
            ?? throw new Exception("No se pudo obtener el token de PayPal");
    }

    // ─── Paso 2: Crear orden en PayPal y guardar registro ────────────────
    public async Task<DonationDto> CreateOrderAsync(CreateDonationDto dto)
    {
        var token = await GetAccessTokenAsync();

        var orderPayload = new
        {
            intent = "CAPTURE",
            purchase_units = new[]
            {
                new
                {
                    amount = new
                    {
                        currency_code = "MXN",
                        value = dto.amount.ToString("F2")
                    },
                    description = "Donación a PetCare"
                }
            },
            application_context = new
            {
                return_url = _config["PayPal:ReturnUrl"],
                cancel_url = _config["PayPal:CancelUrl"]
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, $"{PAYPAL_BASE}/v2/checkout/orders");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(
            JsonSerializer.Serialize(orderPayload),
            Encoding.UTF8, "application/json");

        var response = await _http.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        var orderId = doc.RootElement.GetProperty("id").GetString()
            ?? throw new Exception("PayPal no devolvió un ID de orden");

        // Obtener link de aprobación para redirigir al usuario
        var approvalUrl = doc.RootElement
            .GetProperty("links")
            .EnumerateArray()
            .First(l => l.GetProperty("rel").GetString() == "approve")
            .GetProperty("href").GetString()
            ?? throw new Exception("No se encontró el link de aprobación de PayPal");

        // Guardar registro en BD con status pendiente
        var donation = new Donation
        {
            paypal_order_id = orderId,
            amount = dto.amount,
            donor_name = string.IsNullOrWhiteSpace(dto.donor_name) ? "Anónimo" : dto.donor_name,
            donor_email = dto.donor_email,
            message = dto.message,
            status = "pendiente",
            created_at = DateTime.UtcNow
        };

        await _repo.AddAsync(donation);
        await _repo.SaveChangesAsync();

        return MapToDto(donation, approvalUrl);
    }

    // ─── Paso 3: Capturar pago confirmado por el usuario ─────────────────
    public async Task<DonationDto> CaptureOrderAsync(string paypal_order_id)
    {
        var token = await GetAccessTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Post,
            $"{PAYPAL_BASE}/v2/checkout/orders/{paypal_order_id}/capture");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

        var response = await _http.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        var status = doc.RootElement.GetProperty("status").GetString();

        // Actualizar registro en BD
        var donation = await _repo.GetByPaypalOrderIdAsync(paypal_order_id)
            ?? throw new Exception("Donación no encontrada");

        donation.status = status == "COMPLETED" ? "completada" : "fallida";
        await _repo.UpdateAsync(donation);
        await _repo.SaveChangesAsync();

        return MapToDto(donation, string.Empty);
    }

    // ─── Listar todas las donaciones (para el admin) ──────────────────────
    public async Task<List<DonationDto>> GetAllAsync()
    {
        var donations = await _repo.GetAllAsync();
        return donations.Select(d => MapToDto(d, string.Empty)).ToList();
    }

    private static DonationDto MapToDto(Donation d, string approvalUrl) => new(
        d.id_donation,
        d.paypal_order_id,
        approvalUrl,
        d.amount,
        d.status,
        d.donor_name,
        d.donor_email,
        d.message,
        d.created_at
    );
}