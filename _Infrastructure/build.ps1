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
Write-Host "`n[1/4] Restoring dependencies..." -ForegroundColor Yellow
dotnet restore AutoVersion.sln | Out-Host

# Build
Write-Host "`n[2/4] Building ($Configuration)..." -ForegroundColor Yellow
dotnet build AutoVersion.sln --configuration $Configuration --no-restore | Out-Host

# Tests
if (-not $NoTests) {
    Write-Host "`n[3/4] Running tests..." -ForegroundColor Yellow
    dotnet test src/AutoVersion.Tests --configuration $Configuration --no-build --logger "trx;LogFileName=test_results.trx" | Out-Host
}

Write-Host "`n[4/4] Packaging CLI..." -ForegroundColor Yellow
$cliProject = "$PSScriptRoot\..\src\AutoVersion.Cli\AutoVersion.Cli.csproj"
$buildDir   = "$PSScriptRoot\..\Builds"

dotnet publish $cliProject `
    -c Release `
    -r win-x64 `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeAllContentForSelfExtract=true `
    /p:PublishTrimmed=false `
    -o $buildDir

if (Test-Path "$buildDir\AutoVersion.Cli.exe") {
    Rename-Item "$buildDir\AutoVersion.Cli.exe" "autoversion.exe" -Force
    Write-Host "✅ CLI packaged to: $buildDir\autoversion.exe" -ForegroundColor Green
} else {
    Write-Host "⚠️  CLI build did not produce an EXE!" -ForegroundColor Yellow
}

# Output path
$buildDir = Join-Path -Path $PSScriptRoot -ChildPath "../Builds"
if (-not (Test-Path $buildDir)) { New-Item -ItemType Directory -Path $buildDir | Out-Null }

Write-Host "`n✅ Build completed successfully." -ForegroundColor Green
Write-Host "Artifacts available in: $buildDir`n"
