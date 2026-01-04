using EhrLite.Medications.Api.Data;
using Medications.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<MedicationsDbContext>("medsdb");

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapGet("/ping", () => Results.Ok(new { service = "medications", status = "ok", at = DateTimeOffset.UtcNow }));

app.MapGet("/api/patients/{patientId:guid}/medications", (Guid patientId) =>
{
    var now = DateTimeOffset.UtcNow;

    IReadOnlyList<MedicationDto> meds =
    [
        new MedicationDto(Guid.NewGuid(), patientId, now.AddDays(-14), "Amoxicillin", "500mg"),
        new MedicationDto(Guid.NewGuid(), patientId, now.AddDays(-3),  "Ibuprofen",   "400mg"),
    ];

    return Results.Ok(meds);
});

app.Run();