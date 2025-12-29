using ClinicalDocs.Api.Dtos;
using EhrLite.ClinicalDocs.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ClinicalDocsDbContext>("clinicaldocsdb");

var app = builder.Build();
app.MapGet("/ping", () => Results.Ok(new
{
    service = "clinical-docs",
    status = "ok",
    at = DateTimeOffset.UtcNow
}))
.WithName("Ping");

app.MapGet("/api/patients/{patientId:guid}/notes", (Guid patientId) =>
{
    var items = new List<ClinicalNoteDto>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            PatientId: patientId,
            CreatedAt: DateTimeOffset.UtcNow.AddDays(-5),
            Title: "Triage Note",
            Body: "Patient reports headache and fatigue for 3 days. No known allergies. Vitals stable."
        ),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            PatientId: patientId,
            CreatedAt: DateTimeOffset.UtcNow.AddDays(-1),
            Title: "Progress Note",
            Body: "Symptoms improving. Recommended hydration and follow-up if worsening."
        )
    };

    return Results.Ok(items);
})
.WithName("GetNotesByPatient")
.Produces<List<ClinicalNoteDto>>(StatusCodes.Status200OK);

app.MapOpenApi();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.Run();
