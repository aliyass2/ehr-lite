namespace EhrLite.TimelineBff.Api.Contracts;

public sealed record TimelineItem(
    string Kind,
    DateTimeOffset At,
    string Title,
    string? Summary,
    object Data);
