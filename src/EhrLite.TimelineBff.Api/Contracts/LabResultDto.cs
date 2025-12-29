namespace EhrLite.TimelineBff.Api.Contracts;

public sealed record LabResultDto(
    Guid Id,
    Guid PatientId,
    DateTimeOffset CollectedAt,
    string TestName,
    string? Value,
    string? Unit);
