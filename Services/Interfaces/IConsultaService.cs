using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface IConsultaService
{
    Task<ConsultaDto> CreateAsync(CreateConsultaDto dto);
    Task<ConsultaDto?> GetByCitaAsync(Guid idCita);
}