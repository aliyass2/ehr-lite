namespace EhrLite.TimelineBff.Api.Contracts;

public sealed record ClinicalNoteDto(
    Guid Id,
    Guid PatientId,
    DateTimeOffset CreatedAt,
    string Title,
    string Body);
