using EhrLite.TimelineBff.Api.Clients;
using EhrLite.TimelineBff.Api.Contracts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddServiceDefaults();

// Bind service base URLs from config (compose env vars)
builder.Services.Configure<ServiceOptions>("PatientRegistry", builder.Configuration.GetSection("Services:PatientRegistry"));
builder.Services.Configure<ServiceOptions>("Encounters", builder.Configuration.GetSection("Services:Encounters"));
builder.Services.Configure<ServiceOptions>("ClinicalDocs", builder.Configuration.GetSection("Services:ClinicalDocs"));
builder.Services.Configure<ServiceOptions>("Medications", builder.Configuration.GetSection("Services:Medications"));
builder.Services.Configure<ServiceOptions>("LabsDocs", builder.Configuration.GetSection("Services:LabsDocs"));
builder.Services.Configure<ServiceOptions>("Audit", builder.Configuration.GetSection("Services:Audit"));

// Typed HttpClients
builder.Services.AddHttpClient<PatientRegistryClient>((sp, http) =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<ServiceOptions>>().Get("PatientRegistry");
    http.BaseAddress = new Uri(opt.BaseUrl);
});

builder.Services.AddHttpClient<EncountersClient>((sp, http) =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<ServiceOptions>>().Get("Encounters");
    http.BaseAddress = new Uri(opt.BaseUrl);
});

builder.Services.AddHttpClient<ClinicalDocsClient>((sp, http) =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<ServiceOptions>>().Get("ClinicalDocs");
    http.BaseAddress = new Uri(opt.BaseUrl);
});

builder.Services.AddHttpClient<MedicationsClient>((sp, http) =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<ServiceOptions>>().Get("Medications");
    http.BaseAddress = new Uri(opt.BaseUrl);
});

builder.Services.AddHttpClient<LabsDocsClient>((sp, http) =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<ServiceOptions>>().Get("LabsDocs");
    http.BaseAddress = new Uri(opt.BaseUrl);
});

builder.Services.AddHttpClient<AuditClient>((sp, http) =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<ServiceOptions>>().Get("Audit");
    http.BaseAddress = new Uri(opt.BaseUrl);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/api/timeline/{patientId:guid}", async (
    Guid patientId,
    IConfiguration config,
    PatientRegistryClient patients,
    EncountersClient encounters,
    ClinicalDocsClient clinicalDocs,
    MedicationsClient meds,
    LabsDocsClient labs,
    AuditClient audit,
    CancellationToken ct) =>
{
    static async Task<T?> Safe<T>(Func<Task<T?>> action)
    {
        try { return await action(); }
        catch { return default; }
    }

    static async Task<IReadOnlyList<T>> SafeList<T>(Func<Task<IReadOnlyList<T>>> action)
    {
        try { return await action(); }
        catch { return []; }
    }

    // Day 2: false (only 3 services)
    // Day 3: true (enable meds/labs/audit)
    var includeOptional = config.GetValue("Timeline:IncludeOptional", false);

    var patientTask = Safe(() => patients.GetPatientAsync(patientId, ct));
    var encountersTask = SafeList(() => encounters.ListAsync(patientId, ct));
    var notesTask = SafeList(() => clinicalDocs.ListNotesAsync(patientId, ct));

    await Task.WhenAll(patientTask, encountersTask, notesTask);

    var items = new List<TimelineItem>();

    items.AddRange(encountersTask.Result.Select(e =>
        new TimelineItem("encounter", e.StartAt, $"{e.Type} encounter", e.Location, e)));

    items.AddRange(notesTask.Result.Select(n =>
        new TimelineItem("clinical-note", n.CreatedAt, n.Title,
            n.Body.Length > 120 ? n.Body[..120] + "..." : n.Body, n)));

    // IMPORTANT: optional calls are created only when enabled
    if (includeOptional)
    {
        var medsTask = SafeList(() => meds.ListAsync(patientId, ct));
        var labsTask = SafeList(() => labs.ListAsync(patientId, ct));
        var auditTask = SafeList(() => audit.ListAsync(patientId, ct));

        await Task.WhenAll(medsTask, labsTask, auditTask);

        items.AddRange(medsTask.Result.Select(m =>
            new TimelineItem("medication", m.StartedAt, m.Name, m.Dose, m)));

        items.AddRange(labsTask.Result.Select(l =>
            new TimelineItem("lab", l.CollectedAt, l.TestName,
                $"{l.Value} {l.Unit}".Trim(), l)));

        items.AddRange(auditTask.Result.Select(a =>
            new TimelineItem("audit", a.At, a.Action, a.Actor, a)));
    }

    var ordered = items.OrderByDescending(x => x.At).ToList();
    return Results.Ok(new TimelineResponse(patientTask.Result, ordered));
})
.WithName("GetPatientTimeline");


app.MapHealthChecks("/health");
app.MapHealthChecks("/alive", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live")
});
app.Run();
