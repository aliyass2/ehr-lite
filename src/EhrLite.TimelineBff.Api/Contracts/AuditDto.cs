namespace EhrLite.TimelineBff.Api.Contracts;

public sealed record AuditDto(
    Guid Id,
    Guid PatientId,
    DateTimeOffset At,
    string Action,
    string? Actor);
