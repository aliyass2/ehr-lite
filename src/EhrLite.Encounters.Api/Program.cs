using EhrLite.Encounters.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<EncounterDbContext>("encounterdb");

var app = builder.Build();

app.MapOpenApi();
app.MapDefaultEndpoints();
app.UseHttpsRedirection();
app.Run();
