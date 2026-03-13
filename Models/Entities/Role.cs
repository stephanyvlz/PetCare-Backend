using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("Role")]
public class Role
{
    [Key]
    public int id_role { get; set; }

    [Required, MaxLength(50)]
    public string role_name { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = [];
}