using EhrLite.ClinicalDocs.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ClinicalDocsDbContext>("clinicaldocsdb");

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.Run();
