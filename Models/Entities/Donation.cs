using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("donations")]
public class Donation
{
    [Key]
    public Guid id_donation { get; set; } = Guid.NewGuid();
    public string paypal_order_id { get; set; } = string.Empty;
    public decimal amount { get; set; }
    public string status { get; set; } = "pendiente";
    public string donor_name { get; set; } = "Anónimo";
    public string? donor_email { get; set; }
    public string? message { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
}