using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class MascotaService : IMascotaService
{
    private readonly IMascotaRepository _repo;
    public MascotaService(IMascotaRepository repo) => _repo = repo;

    public async Task<List<MascotaDto>> GetByUsuarioAsync(Guid idUsuario)
    {
        var mascotas = await _repo.GetByUsuarioAsync(idUsuario);
        return mascotas.Select(m => new MascotaDto(
            m.IdMascota, m.Nombre, m.Raza, m.Peso, m.Edad)).ToList();
    }

    public async Task<MascotaDto> CreateAsync(Guid idUsuario, CreateMascotaDto dto)
    {
        var mascota = new Mascota
        {
            Nombre = dto.Nombre,
            Raza = dto.Raza,
            Peso = dto.Peso,
            Edad = dto.Edad,
            IdUsuario = idUsuario
        };

        await _repo.AddAsync(mascota);
        await _repo.SaveChangesAsync();

        return new MascotaDto(mascota.IdMascota, mascota.Nombre,
            mascota.Raza, mascota.Peso, mascota.Edad);
    }

    public async Task DeleteAsync(Guid idMascota)
    {
        var mascota = await _repo.GetByIdAsync(idMascota)
            ?? throw new Exception("Mascota no encontrada");

        await _repo.DeleteAsync(mascota);
        await _repo.SaveChangesAsync();
    }
}