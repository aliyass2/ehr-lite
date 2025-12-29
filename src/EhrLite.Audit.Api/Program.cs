using EhrLite.Audit.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<AuditDbContext>("auditdb");

var app = builder.Build();

app.MapOpenApi();
app.MapDefaultEndpoints();
app.UseHttpsRedirection();


app.Run();