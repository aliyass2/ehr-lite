param(
  [string]$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
  [switch]$RemoveVolumes
)

$ErrorActionPreference = "Stop"

$publishDir = Join-Path $Root "publish"
$composeFile = Join-Path $publishDir "docker-compose.yaml"
$envFile = Join-Path $publishDir ".env"

if (-not (Test-Path $composeFile)) { throw "Missing: $composeFile" }
if (-not (Test-Path $envFile))     { throw "Missing: $envFile" }

Set-Location $publishDir

Write-Host "Stopping containers..."
if ($RemoveVolumes) {
  docker compose --env-file $envFile -f $composeFile down -v
} else {
  docker compose --env-file $envFile -f $composeFile down
}
