using PetCare.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("Treatments")]
public class Treatment
{
    [Key]
    public Guid treatment_id { get; set; } = Guid.NewGuid();

    public Guid id_consultation { get; set; }
    [ForeignKey(nameof(id_consultation))]
    public Consultation Consultations { get; set; } = null!;

    [Required, MaxLength(150)]
    public string medication { get; set; } = string.Empty;

    [MaxLength(100)]
    public string dosage { get; set; } = string.Empty;

    [MaxLength(100)]
    public string duration { get; set; } = string.Empty;

    public decimal cost { get; set; }
}