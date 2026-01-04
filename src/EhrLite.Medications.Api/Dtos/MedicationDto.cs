namespace Medications.Api.Dtos;

public sealed record MedicationDto(
    Guid Id,
    Guid PatientId,
    DateTimeOffset StartedAt,
    string Name,
    string? Dose);
