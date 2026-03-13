using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.API.Models.Entities;

[Table("Appointments")]
public class Appointment
{
    [Key]
    public Guid id_appointment { get; set; } = Guid.NewGuid();

    public Guid id_user { get; set; }
    [ForeignKey(nameof(id_user))]
    public User User { get; set; } = null!;

    public Guid id_pet { get; set; }
    [ForeignKey(nameof(id_pet))]
    public Pet Pet { get; set; } = null!;

    public Guid id_clinic { get; set; }
    [ForeignKey(nameof(id_clinic))]
    public Clinic Clinic { get; set; } = null!;

    public Guid id_veterinarian { get; set; }
    [ForeignKey(nameof(id_veterinarian))]
    public User veterinarian { get; set; } = null!;

    public DateTime appointment_date { get; set; }

    [MaxLength(100)]
    public string service { get; set; } = string.Empty;

    public decimal cost { get; set; }

    [MaxLength(20)]
    public string status { get; set; } = "pendiente";

    public Consultation? Consultations { get; set; }
}