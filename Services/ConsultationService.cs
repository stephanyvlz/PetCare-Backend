using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class ConsultationService : IConsultationService
{
    private readonly IConsultationRepository _repo;
    private readonly IAppointmentRepository _AppointmentRepo;

    public ConsultationService(IConsultationRepository repo, IAppointmentRepository AppointmentRepo)
    {
        _repo = repo;
        _AppointmentRepo = AppointmentRepo;
    }

    public async Task<ConsultationDto> CreateAsync(CreateConsultationDto dto)
    {
        var cita = await _AppointmentRepo.GetByIdAsync(dto.id_appointment)
            ?? throw new Exception("Cita no encontrada");

        if (cita.status != "atendida")
            throw new Exception("Solo se puede crear una consulta para citas atendidas");

        var existente = await _repo.GetByCitaAsync(dto.id_consultation);
        if (existente is not null)
            throw new Exception("Esta cita ya tiene una consulta registrada");

        var consulta = new Consultation
        {
            id_consultation = dto.id_consultation,
            diagnosis = dto.diagnosis,
            observation = dto.observations,
            consultation_date = DateTime.UtcNow
        };

        await _repo.AddAsync(consulta);
        await _repo.SaveChangesAsync();

        return new ConsultationDto(consulta.id_consultation, consulta.id_appointment,
            consulta.diagnosis, consulta.observation, consulta.consultation_date);
    }

    public async Task<ConsultationDto?> GetByAppointmentAsync(Guid id_appointment)
    {
        var consultation = await _repo.GetByAppointmentAsync(id_appointment);
        if (consultation is null) return null;

        return new ConsultationDto(consultation.id_consultation, consultation.id_appointment,
            consultation.diagnosis, consultation.observation, consultation.consultation_date);
    }
}