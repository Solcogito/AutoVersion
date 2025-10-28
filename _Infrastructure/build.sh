#!/usr/bin/env bash
# ============================================================================
# File:        build.sh
# Project:     AutoVersion Lite
# Author:      Recursive Architect (Solcogito S.E.N.C.)
# ----------------------------------------------------------------------------
# Description:
#   Cross-platform build script for Linux/macOS developers and CI runners.
#   Restores, builds, tests, and outputs artifacts to ./Builds/
# ============================================================================

set -e

echo "=== AutoVersion Build Script (bash) ==="

CONFIG="Debug"
NO_TESTS=false

# Parse args
for arg in "$@"; do
  case $arg in
    -r|--release) CONFIG="Release" ;;
    -nt|--no-tests) NO_TESTS=true ;;
  esac
done

# Ensure .NET is installed
if ! command -v dotnet &> /dev/null; then
  echo "❌ .NET SDK not found. Please install .NET 8.0 or newer."
  exit 1
fi

echo ""
echo "[1/3] Restoring dependencies..."
dotnet restore src/AutoVersion.sln || dotnet restore

echo ""
echo "[2/3] Building ($CONFIG)..."
dotnet build src/AutoVersion.sln --configuration $CONFIG --no-restore

if [ "$NO_TESTS" = false ]; then
  echo ""
  echo "[3/3] Running tests..."
  dotnet test src/AutoVersion.Tests --configuration $CONFIG --no-build --logger "trx;LogFileName=test_results.trx"
fi

BUILD_DIR="$(dirname "$0")/../Builds"
mkdir -p "$BUILD_DIR"

echo ""
echo "✅ Build completed successfully."
echo "Artifacts are available in: $BUILD_DIR"
