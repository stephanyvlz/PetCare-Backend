using Microsoft.EntityFrameworkCore;
using PetCare.API.Models.Entities;

namespace PetCare.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Clinic> Clinic => Set<Clinic>();
    public DbSet<Appointment> Appointment => Set<Appointment>();
    public DbSet<Consultation> Consultation => Set<Consultation>();
    public DbSet<Treatment> Treatment => Set<Treatment>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>()
        .HasIndex(u => u.email)
        .IsUnique();

    //  User → Appointment (cliente)
    modelBuilder.Entity<Appointment>()
        .HasOne(a => a.User)
        .WithMany(u => u.Appointments)
        .HasForeignKey(a => a.id_user)
        .OnDelete(DeleteBehavior.Cascade);

    //  User → Appointment (veterinario)
    modelBuilder.Entity<Appointment>()
        .HasOne(a => a.veterinarian)
        .WithMany()
        .HasForeignKey(a => a.id_veterinarian)
        .OnDelete(DeleteBehavior.Cascade);

    //  Appointment → Consultation (1:1)
    modelBuilder.Entity<Consultation>()
        .HasOne(c => c.Appointments)
        .WithOne(a => a.Consultations)
        .HasForeignKey<Consultation>(c => c.id_appointment)
        .OnDelete(DeleteBehavior.Cascade);

    //  Consultation → Treatment (1:N)
    modelBuilder.Entity<Treatment>()
        .HasOne(t => t.Consultations)
        .WithMany(c => c.Treatments)
        .HasForeignKey(t => t.id_consultation)
        .OnDelete(DeleteBehavior.Cascade);

    // Roles iniciales
    modelBuilder.Entity<Role>().HasData(
        new Role { id_role = 1, role_name = "admin" },
        new Role { id_role = 2, role_name = "veterinario" },
        new Role { id_role = 3, role_name = "cliente" }
    );
}
}