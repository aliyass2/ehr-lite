namespace EhrLite.Audit.Api.Dtos;

public sealed record AuditDto(
    Guid Id,
    Guid PatientId,
    DateTimeOffset At,
    string Action,
    string? Actor);
