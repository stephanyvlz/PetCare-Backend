using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetCare.API.Models.Entities;

public class PasswordResetToken
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public bool IsUsed { get; set; } = false;
}