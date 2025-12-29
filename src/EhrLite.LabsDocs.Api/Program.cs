using EhrLite.LabsDocs.Api.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<LabsDocsDbContext>("labsdocsdb");

var app = builder.Build();

app.MapOpenApi();

//app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.Run();
