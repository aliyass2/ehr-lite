namespace EhrLite.TimelineBff.Api.Contracts;

public sealed record EncounterDto(
    Guid Id,
    Guid PatientId,
    DateTimeOffset StartAt,
    string Type,
    string? Location);
