# ============================================================================
# File:        build.ps1
# Project:     AutoVersion Lite
# Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
# ----------------------------------------------------------------------------
# Description:
#   Local developer build script
#   - Restores dependencies
#   - Builds solution
#   - Runs unit tests
#   - Publishes CLI executable under ./Builds/
# ============================================================================

param(
    [switch]$Release = $false,
    [switch]$NoTests = $false
)

Write-Host "=== AutoVersion Build Script ===" -ForegroundColor Cyan

# --------------------------------------------------------------------------
# Configuration
# --------------------------------------------------------------------------
$Configuration = if ($Release) { "Release" } else { "Debug" }
$RootDir  = Split-Path -Parent $PSScriptRoot
$CliProj  = Join-Path $RootDir "src/AutoVersion.Cli/AutoVersion.Cli.csproj"
$BuildDir = Join-Path $RootDir "Builds"

# --------------------------------------------------------------------------
# Ensure .NET SDK
# --------------------------------------------------------------------------
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Host "❌ .NET SDK not found. Please install .NET 8.0 or newer." -ForegroundColor Red
    exit 1
}

# --------------------------------------------------------------------------
# 1. Restore
# --------------------------------------------------------------------------
Write-Host "`n[1/4] Restoring dependencies..." -ForegroundColor Yellow
dotnet restore "$RootDir/AutoVersion.sln" | Out-Host

# --------------------------------------------------------------------------
# 2. Build
# --------------------------------------------------------------------------
Write-Host "`n[2/4] Building solution ($Configuration)..." -ForegroundColor Yellow
dotnet build "$RootDir/AutoVersion.sln" --configuration $Configuration --no-restore | Out-Host

# --------------------------------------------------------------------------
# 3. Test
# --------------------------------------------------------------------------
if (-not $NoTests) {
    Write-Host "`n[3/4] Running tests..." -ForegroundColor Yellow
    dotnet test "$RootDir/src/AutoVersion.Tests" --configuration $Configuration --no-build `
        --logger "trx;LogFileName=test_results.trx" | Out-Host
}

# --------------------------------------------------------------------------
# 4. Publish CLI
# --------------------------------------------------------------------------
Write-Host "`n[4/4] Publishing CLI..." -ForegroundColor Yellow

# Define build directory (absolute path)
$RootDir  = Split-Path -Parent $PSScriptRoot
$BuildDir = Join-Path $RootDir "Builds"

# Ensure it exists
if (-not (Test-Path $BuildDir)) {
    New-Item -ItemType Directory -Path $BuildDir | Out-Null
}

# ✅ Use absolute path to prevent nested Builds/Builds confusion
$publishOut = (Resolve-Path $BuildDir).Path

dotnet publish $CliProj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeAllContentForSelfExtract=true `
    /p:PublishTrimmed=false `
    -o "$publishOut" | Out-Host

# Clean rename with overwrite
$cliExe   = Join-Path $BuildDir "AutoVersion.Cli.exe"
$finalExe = Join-Path $BuildDir "autoversion.exe"

if (Test-Path $finalExe) { Remove-Item $finalExe -Force }

if (Test-Path $cliExe) {
    Rename-Item $cliExe $finalExe -Force
    Write-Host "✅ CLI packaged: $finalExe" -ForegroundColor Green
} else {
    Write-Host "⚠️ CLI build did not produce an executable!" -ForegroundColor Yellow
}

Write-Host "`n✅ Build completed successfully." -ForegroundColor Green
Write-Host "Artifacts available in: $BuildDir`n"
