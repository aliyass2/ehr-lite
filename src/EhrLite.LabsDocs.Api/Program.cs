using EhrLite.LabsDocs.Api.Data;
using LabsDocs.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<LabsDocsDbContext>("labsdocsdb");

var app = builder.Build();

app.MapOpenApi();
app.MapGet("/ping", () => Results.Ok(new { service = "labs-docs", status = "ok", at = DateTimeOffset.UtcNow }));

app.MapGet("/api/patients/{patientId:guid}/labs", (Guid patientId) =>
{
    var now = DateTimeOffset.UtcNow;

    IReadOnlyList<LabResultDto> labs =
    [
        new LabResultDto(Guid.NewGuid(), patientId, now.AddDays(-7), "CBC",     "Normal", null),
        new LabResultDto(Guid.NewGuid(), patientId, now.AddDays(-1), "Glucose", "96",     "mg/dL"),
    ];

    return Results.Ok(labs);
});

app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.Run();
