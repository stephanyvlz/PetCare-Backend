using PetCare.API.Models.DTOs;

namespace PetCare.API.Services.Interfaces;

public interface ITratamientoService
{
    Task<TratamientoDto> CreateAsync(CreateTratamientoDto dto);
    Task<List<TratamientoDto>> GetByConsultaAsync(Guid idConsulta);
}