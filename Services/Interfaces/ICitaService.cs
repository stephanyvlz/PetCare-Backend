using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface ICitaService
{
    Task<List<CitaDto>> GetAllAsync();
    Task<List<CitaDto>> GetByUsuarioAsync(Guid idUsuario);
    Task<CitaDto> CreateAsync(CreateCitaDto dto);
    Task<CitaDto> CambiarEstadoAsync(Guid idCita, string estado);
    Task<List<CitaDto>> GetByVeterinarioAsync(Guid idVeterinario);
}