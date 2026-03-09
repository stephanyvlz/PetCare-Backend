using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class CitaService : ICitaService
{
    private readonly ICitaRepository _repo;
    public CitaService(ICitaRepository repo) => _repo = repo;

    public async Task<List<CitaDto>> GetAllAsync()
    {
        var citas = await _repo.GetAllAsync();
        return citas.Select(MapToDto).ToList();
    }

    public async Task<List<CitaDto>> GetByUsuarioAsync(Guid idUsuario)
    {
        var citas = await _repo.GetByUsuarioAsync(idUsuario);
        return citas.Select(MapToDto).ToList();
    }

    public async Task<CitaDto> CreateAsync(CreateCitaDto dto)
    {
        var cita = new Cita
        {
            IdUsuario = dto.IdUsuario,
            IdMascota = dto.IdMascota,
            IdClinica = dto.IdClinica,
            IdVeterinario = dto.IdVeterinario,
            Fecha = dto.Fecha,
            Servicio = dto.Servicio,
            Costo = dto.Costo,
            Estado = "pendiente"
        };

        await _repo.AddAsync(cita);
        await _repo.SaveChangesAsync();

        var citaCreada = await _repo.GetByIdAsync(cita.IdCita)
            ?? throw new Exception("Error al crear la cita");

        return MapToDto(citaCreada);
    }

    public async Task<CitaDto> CambiarEstadoAsync(Guid idCita, string estado)
    {
        var estadosValidos = new[] { "pendiente", "atendida", "cancelada" };

        if (!estadosValidos.Contains(estado))
            throw new Exception("Estado inválido. Use: pendiente, atendida o cancelada");

        var cita = await _repo.GetByIdAsync(idCita)
            ?? throw new Exception("Cita no encontrada");

        cita.Estado = estado;
        await _repo.SaveChangesAsync();

        return MapToDto(cita);
    }
    public async Task<List<CitaDto>> GetByVeterinarioAsync(Guid idVeterinario)
    {
        var citas = await _repo.GetByVeterinarioAsync(idVeterinario);
        return citas.Select(MapToDto).ToList();
    }

    // Mapeo reutilizable
    private static CitaDto MapToDto(Cita c) => new(
        c.IdCita,
        c.Usuario.Nombre,
        c.Mascota.Nombre,
        c.Veterinario.Nombre,
        c.Fecha,
        c.Servicio,
        c.Costo,
        c.Estado
    );
}