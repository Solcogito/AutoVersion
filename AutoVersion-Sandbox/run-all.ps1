# AutoVersion Sandbox Runner (PowerShell)
# Deterministic, alias-free, CI-safe

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$casesRoot = Join-Path $root "cases"
$shared = Join-Path $root "shared\assert.ps1"

. $shared

# ------------------------------------------------------------
# Resolve AutoVersion executable
# ------------------------------------------------------------

if ($env:AUTOVERSION_EXE -and (Test-Path $env:AUTOVERSION_EXE)) {
    $AutoVersionExe = $env:AUTOVERSION_EXE
}
else {
    $candidate = Join-Path $root "..\publish\AutoVersion.exe"
    if (Test-Path $candidate) {
        $AutoVersionExe = (Resolve-Path $candidate).Path
    }
    else {
        throw @"
AutoVersion executable not found.

Expected one of:
- AUTOVERSION_EXE environment variable
- ../publish/AutoVersion.exe

Set:
  `$env:AUTOVERSION_EXE = "C:\path\to\AutoVersion.exe"
"@
    }
}

Write-Host "Using AutoVersion executable:"
Write-Host "  $AutoVersionExe"
Write-Host ""

# ------------------------------------------------------------
# Run cases
# ------------------------------------------------------------

$cases = Get-ChildItem $casesRoot -Directory | Sort-Object Name

Write-Host "Running AutoVersion sandbox tests..."
Write-Host ""

foreach ($case in $cases) {
    Write-Host "==> $($case.Name)"

    $temp = Join-Path $env:TEMP ("autoversion-sandbox-" + $case.Name)
    if (Test-Path $temp) {
        Remove-Item $temp -Recurse -Force
    }

    New-Item -ItemType Directory -Path $temp | Out-Null

    $before = Join-Path $case.FullName "before"
    if (Test-Path $before) {
        Copy-Item "$before\*" $temp -Recurse -Force -ErrorAction SilentlyContinue
    }

    $command = Get-Content (Join-Path $case.FullName "command.txt") -Raw
    $command = $command.Trim()

    # Strip leading "autoversion"
    $args = $command -replace "^\s*autoversion\s*", ""

    Push-Location $temp

    $proc = Start-Process `
        -FilePath $AutoVersionExe `
        -ArgumentList $args `
        -NoNewWindow `
        -PassThru `
        -Wait `
        -RedirectStandardOutput stdout.txt `
        -RedirectStandardError stderr.txt

    Pop-Location

    $expected = Join-Path $case.FullName "expected"

    Assert-Equal `
        (Get-Content (Join-Path $expected "exitcode.txt") -Raw).Trim() `
        $proc.ExitCode `
        "exit code"

    if (Test-Path (Join-Path $expected "stdout.txt")) {
        Assert-File-Content `
            (Join-Path $temp "stdout.txt") `
            (Get-Content (Join-Path $expected "stdout.txt") -Raw)
    }

    if (Test-Path (Join-Path $expected "stderr.txt")) {
        Assert-File-Content `
            (Join-Path $temp "stderr.txt") `
            (Get-Content (Join-Path $expected "stderr.txt") -Raw)
    }

    Get-ChildItem $expected -File |
        Where-Object { $_.Name -notin @("exitcode.txt", "stdout.txt", "stderr.txt") } |
        ForEach-Object {
            Assert-File-Content `
                (Join-Path $temp $_.Name) `
                (Get-Content $_.FullName -Raw)
        }

    Write-Host "    OK"
    Write-Host ""
}

Write-Host "All sandbox tests passed."
