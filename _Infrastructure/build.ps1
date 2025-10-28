# ============================================================================
# File:        build.ps1
# Project:     AutoVersion Lite
# Author:      Recursive Architect (Solcogito S.E.N.C.)
# ----------------------------------------------------------------------------
# Description:
#   Local developer build script
#   - Restores dependencies
#   - Builds solution
#   - Runs unit tests
#   - Produces build artifacts under ./Builds/
# ============================================================================

param(
    [switch]$Release = $false,
    [switch]$NoTests = $false
)

Write-Host "=== AutoVersion Build Script ===" -ForegroundColor Cyan

$Configuration = if ($Release) { "Release" } else { "Debug" }

# Detect .NET SDK
$dotnet = (Get-Command dotnet -ErrorAction SilentlyContinue)
if (-not $dotnet) {
    Write-Host "❌ .NET SDK not found. Please install .NET 8.0 or newer." -ForegroundColor Red
    exit 1
}

# Restore
Write-Host "`n[1/3] Restoring dependencies..." -ForegroundColor Yellow
dotnet restore src/AutoVersion.sln | Out-Host

# Build
Write-Host "`n[2/3] Building ($Configuration)..." -ForegroundColor Yellow
dotnet build src/AutoVersion.sln --configuration $Configuration --no-restore | Out-Host

# Tests
if (-not $NoTests) {
    Write-Host "`n[3/3] Running tests..." -ForegroundColor Yellow
    dotnet test src/AutoVersion.Tests --configuration $Configuration --no-build --logger "trx;LogFileName=test_results.trx" | Out-Host
}

# Output path
$buildDir = Join-Path -Path $PSScriptRoot -ChildPath "../Builds"
if (-not (Test-Path $buildDir)) { New-Item -ItemType Directory -Path $buildDir | Out-Null }

Write-Host "`n✅ Build completed successfully." -ForegroundColor Green
Write-Host "Artifacts available in: $buildDir`n"
