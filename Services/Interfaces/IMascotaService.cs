using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IMascotaService
{
    Task<List<MascotaDto>> GetByUsuarioAsync(Guid idUsuario);
    Task<MascotaDto> CreateAsync(Guid idUsuario, CreateMascotaDto dto);
    Task DeleteAsync(Guid idMascota);
}