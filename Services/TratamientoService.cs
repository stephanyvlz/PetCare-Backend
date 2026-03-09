using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class TratamientoService : ITratamientoService
{
    private readonly ITratamientoRepository _repo;
    private readonly IConsultaRepository _consultaRepo;

    public TratamientoService(ITratamientoRepository repo, IConsultaRepository consultaRepo)
    {
        _repo = repo;
        _consultaRepo = consultaRepo;
    }

    public async Task<TratamientoDto> CreateAsync(CreateTratamientoDto dto)
    {
        var consulta = await _consultaRepo.GetByIdAsync(dto.IdConsulta)
            ?? throw new Exception("Consulta no encontrada");

        var tratamiento = new Tratamiento
        {
            IdConsulta = consulta.IdConsulta,
            Medicamento = dto.Medicamento,
            Dosis = dto.Dosis,
            Plazo = dto.Plazo,
            Costo = dto.Costo
        };

        await _repo.AddAsync(tratamiento);
        await _repo.SaveChangesAsync();

        return new TratamientoDto(tratamiento.IdTratamiento, tratamiento.Medicamento,
            tratamiento.Dosis, tratamiento.Plazo, tratamiento.Costo);
    }

    public async Task<List<TratamientoDto>> GetByConsultaAsync(Guid idConsulta)
    {
        var tratamientos = await _repo.GetByConsultaAsync(idConsulta);
        return tratamientos.Select(t => new TratamientoDto(
            t.IdTratamiento, t.Medicamento, t.Dosis, t.Plazo, t.Costo)).ToList();
    }
}