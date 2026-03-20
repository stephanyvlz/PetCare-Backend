using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class TreatmentService : ITreatmentService
{
    private readonly ITreatmentRepository _repo;
    private readonly IConsultationRepository _consultationRepo;

    public TreatmentService(ITreatmentRepository repo, IConsultationRepository consultationRepo)
    {
        _repo = repo;
        _consultationRepo = consultationRepo;
    }
    public async Task<List<TreatmentDto>> GetAllAsync()
    {
        var treatments = await _repo.GetAllAsync();
        return treatments.Select(MapToDto).ToList();
    }
    public async Task<TreatmentDto> GetByIdAsync(Guid treatment_id)
    {
        var treatment = await _repo.GetByIdAsync(treatment_id)
            ?? throw new Exception("Tratamiento no encontrado");
        return MapToDto(treatment);
    }

    public async Task<List<TreatmentDto>> GetByConsultationAsync(Guid id_consultation)
    {
        var treatments = await _repo.GetByConsultationAsync(id_consultation);
        return treatments.Select(MapToDto).ToList();
    }

    public async Task<TreatmentDto> CreateAsync(CreateTreatmentDto dto)
    {
        var consultation = await _consultationRepo.GetByIdAsync(dto.id_consultation)
            ?? throw new Exception("Consulta no encontrada");

        var treatment = new Treatment
        {
            treatment_id = Guid.NewGuid(),
            id_consultation = consultation.id_consultation,
            medication = dto.medication,
            dosage = dto.dosage,
            duration = dto.duration,
            cost = dto.cost
        };

        await _repo.AddAsync(treatment);
        await _repo.SaveChangesAsync();

        return MapToDto(treatment);
    }

    public async Task<TreatmentDto> UpdateAsync(Guid treatment_id, UpdateTreatmentDto dto)
    {
        var treatment = await _repo.GetByIdAsync(treatment_id)
            ?? throw new Exception("Tratamiento no encontrado");

        treatment.medication = dto.medication;
        treatment.dosage = dto.dosage;
        treatment.duration = dto.duration;
        treatment.cost = dto.cost;

        await _repo.UpdateAsync(treatment);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdAsync(treatment_id)
            ?? throw new Exception("Error al actualizar el tratamiento");
        return MapToDto(updated);
    }

    public async Task DeleteAsync(Guid treatment_id)
    {
        var treatment = await _repo.GetByIdAsync(treatment_id)
            ?? throw new Exception("Tratamiento no encontrado");

        await _repo.DeleteAsync(treatment);
        await _repo.SaveChangesAsync();
    }

    private static TreatmentDto MapToDto(Treatment t) => new(
        t.treatment_id,
        t.medication,
        t.dosage,
        t.duration,
        t.cost
    );
}