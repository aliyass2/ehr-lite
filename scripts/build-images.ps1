param(
  [string]$Tag = "dev",
  [string]$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
)

$ErrorActionPreference = "Stop"

Write-Host "Repo root: $Root"
Set-Location $Root

$services = @(
  @{ Name = "identity";         Project = "EhrLite.Identity.Api" },
  @{ Name = "patient-registry"; Project = "EhrLite.PatientRegistry.Api" },
  @{ Name = "encounters";       Project = "EhrLite.Encounters.Api" },
  @{ Name = "clinical-docs";    Project = "EhrLite.ClinicalDocs.Api" },
  @{ Name = "medications";      Project = "EhrLite.Medications.Api" },
  @{ Name = "labs-docs";        Project = "EhrLite.LabsDocs.Api" },
  @{ Name = "audit";            Project = "EhrLite.Audit.Api" },
  @{ Name = "timeline-bff";     Project = "EhrLite.TimelineBff.Api" }
)

foreach ($svc in $services) {
  $dockerfile = Join-Path $Root ("src\" + $svc.Project + "\Dockerfile")
  if (-not (Test-Path $dockerfile)) {
    throw "Missing Dockerfile: $dockerfile"
  }

  $image = "ehrlite-$($svc.Name):$Tag"
  Write-Host "Building $image ..."
  docker build -t $image -f $dockerfile $Root
}

Write-Host "All images built successfully."
