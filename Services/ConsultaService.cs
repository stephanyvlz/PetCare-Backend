using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class ConsultaService : IConsultaService
{
    private readonly IConsultaRepository _repo;
    private readonly ICitaRepository _citaRepo;

    public ConsultaService(IConsultaRepository repo, ICitaRepository citaRepo)
    {
        _repo = repo;
        _citaRepo = citaRepo;
    }

    public async Task<ConsultaDto> CreateAsync(CreateConsultaDto dto)
    {
        var cita = await _citaRepo.GetByIdAsync(dto.IdCita)
            ?? throw new Exception("Cita no encontrada");

        if (cita.Estado != "atendida")
            throw new Exception("Solo se puede crear una consulta para citas atendidas");

        var existente = await _repo.GetByCitaAsync(dto.IdCita);
        if (existente is not null)
            throw new Exception("Esta cita ya tiene una consulta registrada");

        var consulta = new Consulta
        {
            IdCita = dto.IdCita,
            Diagnostico = dto.Diagnostico,
            Observaciones = dto.Observaciones,
            Fecha = DateTime.UtcNow
        };

        await _repo.AddAsync(consulta);
        await _repo.SaveChangesAsync();

        return new ConsultaDto(consulta.IdConsulta, consulta.IdCita,
            consulta.Diagnostico, consulta.Observaciones, consulta.Fecha);
    }

    public async Task<ConsultaDto?> GetByCitaAsync(Guid idCita)
    {
        var consulta = await _repo.GetByCitaAsync(idCita);
        if (consulta is null) return null;

        return new ConsultaDto(consulta.IdConsulta, consulta.IdCita,
            consulta.Diagnostico, consulta.Observaciones, consulta.Fecha);
    }
}