param(
  [string]$Tag = "dev",
  [string]$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
)

$ErrorActionPreference = "Stop"

Set-Location $Root

Write-Host "Publishing Aspire compose to .\publish ..."
aspire publish -o ./publish

Write-Host "Bringing stack up..."
& (Join-Path $PSScriptRoot "compose-up.ps1") -Tag $Tag -Root $Root
