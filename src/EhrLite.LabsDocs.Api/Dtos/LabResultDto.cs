namespace LabsDocs.Api.Dtos;

public sealed record LabResultDto(
    Guid Id,
    Guid PatientId,
    DateTimeOffset CollectedAt,
    string TestName,
    string? Value,
    string? Unit);
