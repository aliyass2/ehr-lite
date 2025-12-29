using EhrLite.Encounters.Api.Data;
using Encounters.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<EncounterDbContext>("encounterdb");

var app = builder.Build();
app.MapGet("/api/patients/{patientId:guid}/encounters", (Guid patientId) =>
{
    // Stable IDs for demo (fine for Day 2)
    var items = new List<EncounterDto>
    {
        new(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            PatientId: patientId,
            StartAt: DateTimeOffset.UtcNow.AddDays(-10),
            Type: "Outpatient Visit",
            Location: "Baghdad General Hospital - Clinic A"
        ),
        new(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            PatientId: patientId,
            StartAt: DateTimeOffset.UtcNow.AddDays(-2),
            Type: "Emergency",
            Location: "Baghdad General Hospital - ED"
        )
    };

    return Results.Ok(items);
})
.WithName("GetEncountersByPatient")
.Produces<List<EncounterDto>>(StatusCodes.Status200OK);
app.MapGet("/ping", () => Results.Ok(new
{
    service = "encounters",
    status = "ok",
    at = DateTimeOffset.UtcNow
}))
.WithName("Ping");

app.MapOpenApi();
app.MapDefaultEndpoints();
//app.UseHttpsRedirection();
app.Run();
