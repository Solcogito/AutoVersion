// ============================================================================
// File:        AllTests.cs
// Project:     AutoVersion Lite (Unified Test Suite)
// Version:     0.6.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Unified and cleaned test suite covering core AutoVersion Lite
//   functionality. This Lite version focuses on semantic versioning,
//   basic configuration loading, and version file handling. Git
//   integration and artifact pipeline tests have been intentionally
//   removed to keep the tool focused and CI-friendly.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;
using Xunit;
using Solcogito.AutoVersion.Core;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Tests
{
    // ------------------------------------------------------------------------
    // 1. Semantic Versioning Tests
    // ------------------------------------------------------------------------
    public class SemVerTests
    {
        [Fact]
        public void Compare_OrdersProperly()
        {
            var a = VersionModel.Parse("1.0.0");
            var b = VersionModel.Parse("1.1.0");
            Assert.True(a.CompareTo(b) < 0);
        }

        [Theory]
        [InlineData("1.0.0", "minor", "1.1.0")]
        [InlineData("1.2.3", "patch", "1.2.4")]
        [InlineData("1.2.3", "major", "2.0.0")]
        [InlineData("1.2.3", "prerelease", "1.2.3-alpha.1")]
        public void VersionBumper_BumpsCorrectly(string input, string type, string expected)
        {
            var current = VersionModel.Parse(input);
            var bumped = VersionBumper.Bump(current, type, null);
            Assert.Equal(expected, bumped.ToString());
        }
    }

    // ------------------------------------------------------------------------
    // 2. Config & Version File Tests (Lite)
    // ------------------------------------------------------------------------
    public class ConfigAndVersionFileTests
    {
        [Fact]
        public void VersionFile_WriteAndReadRoundTrip()
        {
            var dir = Path.Combine(Path.GetTempPath(), "AutoVersion_VersionFile_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, "version.txt");

            var version = VersionModel.Parse("1.2.3");
            VersionFile.Write(path, version);

            var readBack = VersionFile.TryRead(path);
            Assert.NotNull(readBack);
            Assert.Equal(version.ToString(), readBack!.ToString());
        }
    }
}
