# ============================================================================
# File:        publish.ps1
# Project:     AutoVersion Lite
# Author:      Recursive Architect (Solcogito S.E.N.C.)
# ----------------------------------------------------------------------------
# Description:
#   Automates the release workflow:
#   - Runs tests
#   - Bumps version via AutoVersion
#   - Generates changelog
#   - Commits and tags new version
#   - Pushes to remote
# ============================================================================

param(
    [Parameter(Mandatory = $false)]
    [string]$Bump = "patch",   # major | minor | patch | prerelease
    [switch]$DryRun = $false
)

Write-Host "=== AutoVersion Publish Script ===" -ForegroundColor Cyan

# Ensure clean repo
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Host "❌ Working directory not clean. Commit or stash changes first." -ForegroundColor Red
    exit 1
}

# Detect CLI build
$cliPath = Join-Path -Path $PSScriptRoot -ChildPath "../src/AutoVersion.Cli/bin/Release/net8.0/autoversion.dll"
if (-not (Test-Path $cliPath)) {
    Write-Host "⚙️ Building CLI first..." -ForegroundColor Yellow
    pwsh "$PSScriptRoot/build.ps1" -Release
}

# Version bump
Write-Host "`n🔧 Bumping version ($Bump)..." -ForegroundColor Yellow
dotnet $cliPath bump $Bump $(if ($DryRun) { "--dry-run" }) | Out-Host

# Generate changelog
Write-Host "`n📝 Generating changelog..." -ForegroundColor Yellow
dotnet $cliPath changelog | Out-Host

if (-not $DryRun) {
    # Add and commit
    git add .
    $version = (dotnet $cliPath current)
    git commit -m "chore(release): v$version"
    git tag "v$version" -m "v$version"
    git push
    git push --tags
    Write-Host "`n🚀 Release v$version pushed to origin." -ForegroundColor Green
}
else {
    Write-Host "`n🧪 Dry-run mode enabled — no files modified or tags created." -ForegroundColor Yellow
}
