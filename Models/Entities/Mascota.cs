using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("mascotas")]
public class Mascota
{
    [Key]
    public Guid IdMascota { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Raza { get; set; } = string.Empty;

    public decimal Peso { get; set; }
    public int Edad { get; set; }

    public Guid IdUsuario { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public Usuario Usuario { get; set; } = null!;

    public ICollection<Cita> Citas { get; set; } = [];
}