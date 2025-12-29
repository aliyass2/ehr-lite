
var builder = DistributedApplication.CreateBuilder(args);

// Compose env (no BuildContainerImages property in your version)
builder.AddDockerComposeEnvironment("compose");

// Infra
var postgres = builder.AddPostgres("pg").WithPgAdmin();
var rabbit = builder.AddRabbitMQ("rabbit");

// One DB per service
var identityDb = postgres.AddDatabase("identitydb");
var patientDb = postgres.AddDatabase("patientdb");
var encounterDb = postgres.AddDatabase("encounterdb");
var clinicalDb = postgres.AddDatabase("clinicaldocsdb");
var medsDb = postgres.AddDatabase("medsdb");
var labsDb = postgres.AddDatabase("labsdocsdb");
var auditDb = postgres.AddDatabase("auditdb");

// Services
builder.AddProject<Projects.EhrLite_Identity_Api>("identity")
    .WithReference(identityDb)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.EhrLite_PatientRegistry_Api>("patient-registry")
    .WithReference(patientDb)
    .WithReference(rabbit)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.EhrLite_Encounters_Api>("encounters")
    .WithReference(encounterDb)
    .WithReference(rabbit)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.EhrLite_ClinicalDocs_Api>("clinical-docs")
    .WithReference(clinicalDb)
    .WithReference(rabbit)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.EhrLite_Medications_Api>("medications")
    .WithReference(medsDb)
    .WithReference(rabbit)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.EhrLite_LabsDocs_Api>("labs-docs")
    .WithReference(labsDb)
    .WithReference(rabbit)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.EhrLite_Audit_Api>("audit")
    .WithReference(auditDb)
    .WithExternalHttpEndpoints();

// Timeline BFF
builder.AddProject<Projects.EhrLite_TimelineBff_Api>("timeline-bff")
    .WithExternalHttpEndpoints();

builder.Build().Run();
