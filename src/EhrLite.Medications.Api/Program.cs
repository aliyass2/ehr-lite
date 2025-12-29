using EhrLite.Medications.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<MedicationsDbContext>("medsdb");

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.Run();