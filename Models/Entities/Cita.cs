using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("citas")]
public class Cita
{
    [Key]
    public Guid IdCita { get; set; } = Guid.NewGuid();

    public Guid IdUsuario { get; set; }
    [ForeignKey(nameof(IdUsuario))]
    public Usuario Usuario { get; set; } = null!;

    public Guid IdMascota { get; set; }
    [ForeignKey(nameof(IdMascota))]
    public Mascota Mascota { get; set; } = null!;

    public Guid IdClinica { get; set; }
    [ForeignKey(nameof(IdClinica))]
    public Clinica Clinica { get; set; } = null!;

    public Guid IdVeterinario { get; set; }
    [ForeignKey(nameof(IdVeterinario))]
    public Usuario Veterinario { get; set; } = null!;

    public DateTime Fecha { get; set; }

    [MaxLength(100)]
    public string Servicio { get; set; } = string.Empty;

    public decimal Costo { get; set; }

    [MaxLength(20)]
    public string Estado { get; set; } = "pendiente";

    public Consulta? Consulta { get; set; }
}