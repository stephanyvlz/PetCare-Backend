using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("Pets")]
public class Pet
{
    [Key]
    public Guid id_pet { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string breed { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? species { get; set; }

    public decimal weight { get; set; }
    public int age { get; set; }

    public Guid id_user { get; set; }

    [ForeignKey(nameof(id_user))]
    public User user { get; set; } = null!;

    public ICollection<Appointment> Appointments { get; set; } = [];
}