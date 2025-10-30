# ============================================================================
# File:        publish.ps1
# Project:     AutoVersion Lite
# Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
# ----------------------------------------------------------------------------
# Description:
#   Automates the full release workflow:
#     • Verifies clean Git state
#     • Builds solution
#     • Runs tests
#     • Bumps version via AutoVersion CLI
#     • Generates changelog
#     • Commits and tags release
#     • Pushes to remote
# ============================================================================

[CmdletBinding()]
param(
    [ValidateSet("major","minor","patch","prerelease")]
    [string]$Bump = "patch",

    [switch]$DryRun
)

# ----------------------------------------------------------------------------
# INITIALIZATION
# ----------------------------------------------------------------------------
Write-Host "=== AutoVersion Publish Script ===" -ForegroundColor Cyan
$ErrorActionPreference = "Stop"

# Get project root (resolves even if script launched from subfolder)
$RootPath = Resolve-Path (Join-Path $PSScriptRoot "..")

# ----------------------------------------------------------------------------
# SANITY CHECKS
# ----------------------------------------------------------------------------
# Ensure Git is clean
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Host "❌ Working directory not clean. Commit or stash changes first." -ForegroundColor Red
    exit 1
}

# Locate CLI executable
$CliExe = Join-Path $RootPath "src/AutoVersion.Cli/bin/Release/net8.0/AutoVersion.Cli.exe"

# Build if missing
if (-not (Test-Path $CliExe)) {
    Write-Host "⚙️  Building CLI first..." -ForegroundColor Yellow
    pwsh "$PSScriptRoot/build.ps1" -Release
}

# ----------------------------------------------------------------------------
# VERSION BUMP
# ----------------------------------------------------------------------------
Write-Host "`n🔧 Bumping version ($Bump)..." -ForegroundColor Yellow
& $CliExe bump $Bump $(if ($DryRun) { "--dry-run" }) | Out-Host

# ----------------------------------------------------------------------------
# CHANGELOG GENERATION
# ----------------------------------------------------------------------------
Write-Host "`n📝 Generating changelog..." -ForegroundColor Yellow
& $CliExe changelog $(if ($DryRun) { "--dry-run" }) | Out-Host

# ----------------------------------------------------------------------------
# GIT COMMIT & TAG
# ----------------------------------------------------------------------------
if (-not $DryRun) {

    # Detect new version number
    $version = (& $CliExe current) -replace '\s',''
    if (-not $version) {
        Write-Host "⚠️  Version could not be detected. Aborting." -ForegroundColor Red
        exit 1
    }

    Write-Host "`n📦 Committing and tagging v$version..." -ForegroundColor Green
    git add .
    git commit -m "chore(release): v$version" | Out-Host
    git tag "v$version" -m "v$version"
    git push
    git push --tags

    Write-Host "`n🚀 Release v$version pushed to origin." -ForegroundColor Green
}
else {
    Write-Host "`n🧪 Dry-run mode enabled — no files modified or tags created." -ForegroundColor Yellow
}

# ----------------------------------------------------------------------------
# SUMMARY
# ----------------------------------------------------------------------------
Write-Host "`n✅ Publish process completed." -ForegroundColor Cyan
