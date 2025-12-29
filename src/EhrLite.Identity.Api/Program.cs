using EhrLite.Identity.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.AddNpgsqlDbContext<IdentityDbContext>("identitydb");

var app = builder.Build();

app.MapOpenApi();
app.MapDefaultEndpoints();
app.UseHttpsRedirection();

app.Run();
