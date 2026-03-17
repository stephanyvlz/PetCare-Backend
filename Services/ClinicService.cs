using PetCare.API.Models.DTOs;
using PetCare.API.Models.Entities;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services.Interfaces;

namespace PetCare.API.Services;

public class ClinicService : IClinicService
{
    private readonly IClinicRepository _clinicRepository;

    public ClinicService(IClinicRepository clinicRepository) =>
        _clinicRepository = clinicRepository;

    public async Task<List<ClinicDto>> GetAllAsync()
    {
        var clinics = await _clinicRepository.GetAllAsync();
        return clinics.Select(c => new ClinicDto(c.id_clinic, c.name, c.location, c.schedule)).ToList();
    }

    public async Task<ClinicDto> GetByIdAsync(Guid id)
    {
        var clinic = await _clinicRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Clínica no encontrada");
        return new ClinicDto(clinic.id_clinic, clinic.name, clinic.location, clinic.schedule);
    }

    public async Task<ClinicDto> CreateAsync(CreateClinicDto dto)
    {
        var clinic = new Clinic
        {
            name = dto.name,
            location = dto.location,
            schedule = dto.schedule
        };
        var created = await _clinicRepository.CreateAsync(clinic);
        return new ClinicDto(created.id_clinic, created.name, created.location, created.schedule);
    }

    public async Task<ClinicDto> UpdateAsync(Guid id, CreateClinicDto dto)
    {
        var clinic = await _clinicRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Clínica no encontrada");

        clinic.name = dto.name;
        clinic.location = dto.location;
        clinic.schedule = dto.schedule;

        var updated = await _clinicRepository.UpdateAsync(clinic);
        return new ClinicDto(updated.id_clinic, updated.name, updated.location, updated.schedule);
    }

    public async Task DeleteAsync(Guid id)
    {
        var clinic = await _clinicRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Clínica no encontrada");
        await _clinicRepository.DeleteAsync(clinic);
    }
}