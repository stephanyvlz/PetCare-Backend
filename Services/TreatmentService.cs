using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<TreatmentDto> CreateAsync(CreateTreatmentDto dto)
    {
        var consulta = await _consultationRepo.GetByIdAsync(dto.id_consultation)
            ?? throw new Exception("Consulta no encontrada");

        var tratamiento = new Treatment
        {
            treatment_id = Guid.NewGuid(),
            id_consultation = consulta.id_consultation,
            medication = dto.medication,
            dosage = dto.dosage,
            duration = dto.duration,
            cost = dto.cost
        };

        await _repo.AddAsync(tratamiento);
        await _repo.SaveChangesAsync();

        return new TreatmentDto(tratamiento.treatment_id, tratamiento.medication,
            tratamiento.dosage, tratamiento.duration, tratamiento.cost);
    }

    public async Task<List<TreatmentDto>> GetByConsultationAsync(Guid id_consultation)
    {
        var treatments = await _repo.GetByConsultationAsync(id_consultation);

        // Convertir los elementos a Treatment antes de acceder a sus propiedades.
        // Uso Cast<Treatment>() para forzar el tipo y exponer errores si los elementos no son Treatment.
        return treatments.Cast<Treatment>()
            .Select(t => new TreatmentDto(
                t.treatment_id, t.medication, t.dosage, t.duration, t.cost))
            .ToList();
    }
}