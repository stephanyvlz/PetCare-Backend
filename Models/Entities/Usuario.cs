using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("usuarios")]
public class Usuario
{
    [Key]
    public Guid IdUsuario { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public int RolId { get; set; }

    [ForeignKey(nameof(RolId))]
    public Rol Rol { get; set; } = null!;

    public ICollection<Mascota> Mascotas { get; set; } = [];
    public ICollection<Cita> Citas { get; set; } = [];
}