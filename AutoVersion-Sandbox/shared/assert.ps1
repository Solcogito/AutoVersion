# ============================================================================
# File:        assert.ps1
# Project:     AutoVersion-Sandbox
# ----------------------------------------------------------------------------
# Description:
#   Minimal assertion helpers for sandbox execution.
#
#   Design principles:
#   - Dependency-free
#   - Deterministic
#   - Explicit failures
#   - LogScribe-aware (timestamp normalization)
#   - CLI-semantics-first (not byte-for-byte)
# ----------------------------------------------------------------------------
# License:     MIT
# ============================================================================

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# ---------------------------------------------------------------------------
# Basic assertions
# ---------------------------------------------------------------------------

function Assert-Equal {
    param (
        [Parameter(Mandatory)]
        [string]$Actual,

        [Parameter(Mandatory)]
        [string]$Expected,

        [Parameter(Mandatory)]
        [string]$Label
    )

    if ($Actual -ne $Expected) {
throw @"
ASSERT FAILED [$Label]

EXPECTED:
$Expected

ACTUAL:
$Actual
"@
    }
}

function Assert-File-Exists {
    param (
        [Parameter(Mandatory)]
        [string]$Path
    )

    if (-not (Test-Path $Path)) {
        throw "ASSERT FAILED: expected file to exist: $Path"
    }
}

function Assert-File-NotExists {
    param (
        [Parameter(Mandatory)]
        [string]$Path
    )

    if (Test-Path $Path) {
        throw "ASSERT FAILED: expected file to NOT exist: $Path"
    }
}

# ---------------------------------------------------------------------------
# Output normalization
# ---------------------------------------------------------------------------

function Normalize-Stdout {
    param (
        [AllowNull()]
        [string]$Text
    )

    if ($null -eq $Text) {
        return ""
    }

    $lines =
        $Text -split "`r?`n" |
        Where-Object { $_.Trim() -ne "" } |
        ForEach-Object {
            # Strip leading LogScribe timestamp:
            # [2025-12-20 23:05:43.753Z]
            $_ -replace '^\[[^\]]+\]\s*', ''
        }

    return ($lines -join "`n").Trim()
}

# ---------------------------------------------------------------------------
# File content assertions (CLI semantics aware)
# ---------------------------------------------------------------------------

function Assert-File-Content {
    param (
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [string]$Expected
    )

    if (-not (Test-Path $Path)) {
        throw "ASSERT FAILED: file does not exist: $Path"
    }

    $actualRaw   = Get-Content $Path -Raw
    $expectedRaw = $Expected

    $actual   = Normalize-Stdout $actualRaw
    $expected = Normalize-Stdout $expectedRaw

    if ($actual -ne $expected) {
throw @"
ASSERT FAILED: content mismatch

ACTUAL FILE:
$Path

NORMALIZED ACTUAL:
$actual

EXPECTED:
$expected
"@
    }
}
