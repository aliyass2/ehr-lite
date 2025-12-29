param(
  [string]$Tag = "dev",
  [string]$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
)

$ErrorActionPreference = "Stop"

# Build images first
& (Join-Path $PSScriptRoot "build-images.ps1") -Tag $Tag -Root $Root

$publishDir = Join-Path $Root "publish"
$composeFile = Join-Path $publishDir "docker-compose.yaml"
$envFile = Join-Path $publishDir ".env"

if (-not (Test-Path $composeFile)) { throw "Missing: $composeFile (run: aspire publish -o ./publish)" }
if (-not (Test-Path $envFile))     { throw "Missing: $envFile" }

Set-Location $publishDir

Write-Host "Validating compose..."
docker compose --env-file $envFile -f $composeFile config | Out-Null

Write-Host "Starting containers..."
docker compose --env-file $envFile -f $composeFile up -d

Write-Host "Running containers:"
docker compose --env-file $envFile -f $composeFile ps
