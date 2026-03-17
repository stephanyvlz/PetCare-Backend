using PetCare.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("Clinics")]
public class Clinic
{
    [Key]
    public Guid id_clinic { get; set; } = Guid.NewGuid();
    [Required, MaxLength(150)]
    public string name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string location { get; set; } = string.Empty;

    [MaxLength(100)]
    public string schedule { get; set; } = string.Empty;

    public ICollection<Appointment> Appointment { get; set; } = [];
    public ICollection<User> Veterinarians { get; set; } = [];
}