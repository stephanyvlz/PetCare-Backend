using Microsoft.EntityFrameworkCore;
using PetCare.API.Models.Entities;

namespace PetCare.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Mascota> Mascotas => Set<Mascota>();
    public DbSet<Clinica> Clinicas => Set<Clinica>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Correo único
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Correo)
            .IsUnique();

        // Cita → Usuario (cliente)
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Citas)
            .HasForeignKey(c => c.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        // Cita → Veterinario (también es Usuario)
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Veterinario)
            .WithMany()
            .HasForeignKey(c => c.IdVeterinario)
            .OnDelete(DeleteBehavior.Restrict);

        // Consulta → Cita (1 a 1)
        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Cita)
            .WithOne(c => c.Consulta)
            .HasForeignKey<Consulta>(c => c.IdCita);

        // Roles iniciales
        modelBuilder.Entity<Rol>().HasData(
            new Rol { IdRol = 1, NombreRol = "admin" },
            new Rol { IdRol = 2, NombreRol = "veterinario" },
            new Rol { IdRol = 3, NombreRol = "cliente" }
        );
    }
}