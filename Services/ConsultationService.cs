using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class ConsultationService : IConsultationService
{
    private readonly IConsultationRepository _repo;
    private readonly IAppointmentRepository _appointmentRepo;

    public ConsultationService(IConsultationRepository repo, IAppointmentRepository appointmentRepo)
    {
        _repo = repo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task<List<ConsultationDto>> GetAllAsync()
    {
        var consultations = await _repo.GetAllAsync();
        return consultations.Select(MapToDto).ToList();
    }

    public async Task<ConsultationDto> GetByIdAsync(Guid id_consultation)
    {
        var consultation = await _repo.GetByIdAsync(id_consultation)
            ?? throw new Exception("Consulta no encontrada");
        return MapToDto(consultation);
    }

    public async Task<ConsultationDto?> GetByAppointmentAsync(Guid id_appointment)
    {
        var consultation = await _repo.GetByAppointmentAsync(id_appointment);
        return consultation is null ? null : MapToDto(consultation);
    }

    public async Task<ConsultationDto> CreateAsync(CreateConsultationDto dto)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(dto.id_appointment)
            ?? throw new Exception("Cita no encontrada");

        if (appointment.status != "atendida")
            throw new Exception("Solo se puede crear una consulta para citas con estado 'atendida'");

        // Verificar que la cita no tenga ya una consulta
        var existente = await _repo.GetByAppointmentAsync(dto.id_appointment);
        if (existente is not null)
            throw new Exception("Esta cita ya tiene una consulta registrada");

        var consulta = new Consultation
        {
            id_consultation = Guid.NewGuid(),
            id_appointment = dto.id_appointment,
            diagnosis = dto.diagnosis,
            observation = dto.observations,
            consultation_date = DateTime.UtcNow
        };

        await _repo.AddAsync(consulta);
        await _repo.SaveChangesAsync();

        return MapToDto(consulta);
    }

    public async Task<ConsultationDto> UpdateAsync(Guid id_consultation, UpdateConsultationDto dto)
    {
        var consultation = await _repo.GetByIdAsync(id_consultation)
            ?? throw new Exception("Consulta no encontrada");

        consultation.diagnosis = dto.diagnosis;
        consultation.observation = dto.observations;

        await _repo.UpdateAsync(consultation);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdAsync(id_consultation)
            ?? throw new Exception("Error al actualizar la consulta");
        return MapToDto(updated);
    }

    public async Task DeleteAsync(Guid id_consultation)
    {
        var consultation = await _repo.GetByIdAsync(id_consultation)
            ?? throw new Exception("Consulta no encontrada");

        await _repo.DeleteAsync(consultation);
        await _repo.SaveChangesAsync();
    }

    private static ConsultationDto MapToDto(Consultation c) => new(
        c.id_consultation,
        c.id_appointment,
        c.diagnosis,
        c.observation,
        c.consultation_date
    );
}