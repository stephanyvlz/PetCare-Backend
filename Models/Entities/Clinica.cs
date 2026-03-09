using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("clinicas")]
public class Clinica
{
    [Key]
    public Guid IdClinica { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Ubicacion { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Horario { get; set; } = string.Empty;

    public ICollection<Cita> Citas { get; set; } = [];
}