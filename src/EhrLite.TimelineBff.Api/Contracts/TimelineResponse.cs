namespace EhrLite.TimelineBff.Api.Contracts;

public sealed record TimelineResponse(
    PatientDto? Patient,
    IReadOnlyList<TimelineItem> Items);
