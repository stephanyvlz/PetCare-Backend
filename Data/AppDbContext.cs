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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Correo único
        modelBuilder.Entity<User>()
            .HasIndex(u => u.email)
            .IsUnique();

        // Cita → Usuario (cliente)
        modelBuilder.Entity<Appointment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(c => c.id_user)
            .OnDelete(DeleteBehavior.Restrict);

        // Cita → Veterinario (también es Usuario)
        modelBuilder.Entity<Appointment>()
            .HasOne(c => c.veterinarian)
            .WithMany()
            .HasForeignKey(c => c.id_veterinarian)
            .OnDelete(DeleteBehavior.Restrict);

        // Consulta → Cita (1 a 1)
        modelBuilder.Entity<Consultation>()
            .HasOne(c => c.Appointments)
            .WithOne(c => c.Consultations)
            .HasForeignKey<Consultation>(c => c.id_appointment);

        // Roles iniciales
        modelBuilder.Entity<Role>().HasData(
            new Role { id_role = 1, role_name = "admin" },
            new Role { id_role = 2, role_name = "veterinario" },
            new Role { id_role = 3, role_name = "cliente" }
        );
    }
}