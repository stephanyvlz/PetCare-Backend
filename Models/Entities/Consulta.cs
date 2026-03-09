using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("consultas")]
public class Consulta
{
    [Key]
    public Guid IdConsulta { get; set; } = Guid.NewGuid();

    public Guid IdCita { get; set; }
    [ForeignKey(nameof(IdCita))]
    public Cita Cita { get; set; } = null!;

    [Required]
    public string Diagnostico { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public ICollection<Tratamiento> Tratamientos { get; set; } = [];
}