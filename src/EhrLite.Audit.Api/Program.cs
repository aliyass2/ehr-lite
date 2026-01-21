using EhrLite.Audit.Api.Data;
using EhrLite.Audit.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<AuditDbContext>("auditdb");

var app = builder.Build();

app.MapOpenApi();
app.MapDefaultEndpoints();

// IMPORTANT: timeline-bff calls http://audit:8080, so do NOT force https redirects in containers
// app.UseHttpsRedirection();

app.MapGet("/ping", () => Results.Ok(new
{
    service = "audit",
    status = "ok",
    at = DateTimeOffset.UtcNow
}));

// MUST match AuditClient: "/api/patients/{patientId}/audit"
app.MapGet("/api/patients/{patientId:guid}/audit", (Guid patientId) =>
{
    var now = DateTimeOffset.UtcNow;

    IReadOnlyList<AuditDto> items =
    [
        new AuditDto(Guid.NewGuid(), patientId, now.AddMinutes(-5),  "TimelineViewed", "demo-user"),
        new AuditDto(Guid.NewGuid(), patientId, now.AddHours(-3),    "NoteCreated",    "doctor-a"),
        new AuditDto(Guid.NewGuid(), patientId, now.AddDays(-1),     "EncounterAdded", "nurse-b"),
    ];

    return Results.Ok(items);
});

app.Run();
