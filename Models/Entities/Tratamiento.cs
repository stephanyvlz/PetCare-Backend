using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("tratamientos")]
public class Tratamiento
{
    [Key]
    public Guid IdTratamiento { get; set; } = Guid.NewGuid();

    public Guid IdConsulta { get; set; }
    [ForeignKey(nameof(IdConsulta))]
    public Consulta Consulta { get; set; } = null!;

    [Required, MaxLength(150)]
    public string Medicamento { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Dosis { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Plazo { get; set; } = string.Empty;

    public decimal Costo { get; set; }
}