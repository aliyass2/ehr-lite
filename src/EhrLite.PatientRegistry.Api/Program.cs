using PatientRegistry.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();
app.MapGet("/ping", () => Results.Ok(new
{
    service = "patient-registry",
    status = "ok",
    at = DateTimeOffset.UtcNow
}))
.WithName("Ping");

app.MapGet("/api/patients/{patientId:guid}", (Guid patientId) =>
{
    var dto = new PatientDto(
        Id: patientId,
        FullName: $"Demo Patient {patientId.ToString()[..8]}"
    );

    return Results.Ok(dto);
})
.WithName("GetPatientById")
.Produces<PatientDto>(StatusCodes.Status200OK);

app.MapOpenApi();
app.UseHttpsRedirection();

app.Run();
