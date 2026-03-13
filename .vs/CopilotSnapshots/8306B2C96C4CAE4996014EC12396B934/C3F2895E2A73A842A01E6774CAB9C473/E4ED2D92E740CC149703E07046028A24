using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("Users")]
public class User
{
    [Key]
    public Guid id_user { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string name { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public int id_role { get; set; }

    [ForeignKey(nameof(id_role))]
    public Role role { get; set; } = null!;

    public ICollection<Pet> pets { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
}