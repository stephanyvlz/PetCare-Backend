using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("roles")]
public class Rol
{
    [Key]
    public int IdRol { get; set; }

    [Required, MaxLength(50)]
    public string NombreRol { get; set; } = string.Empty;

    public ICollection<Usuario> Usuarios { get; set; } = [];
}