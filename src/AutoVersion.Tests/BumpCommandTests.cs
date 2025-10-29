// ============================================================================
// File:        BumpCommandTests.cs
// Project:     AutoVersion Lite (Test Suite)
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Basic behavioral tests for CLI bump command using in-memory stubs.
//   Confirms dry-run behavior and error handling.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Xunit;
using System.IO;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Tests
{
    public class BumpCommandTests
    {
        private const string VersionFileName = "version.txt";

        public BumpCommandTests()
        {
            if (File.Exists(VersionFileName))
                File.Delete(VersionFileName);
        }

        /// <summary>
        /// BumpCommand should increment patch version in dry-run mode.
        /// </summary>
        [Fact]
        public void Execute_Patch_DryRun()
        {
            File.WriteAllText(VersionFileName, "1.0.0");
            var args = new[] { "bump", "patch", "--dry-run" };

            BumpCommand.Execute(args);
            var content = File.ReadAllText(VersionFileName).Trim();

            // Should not modify file during dry-run
            Assert.Equal("1.0.0", content);
        }

        /// <summary>
        /// BumpCommand should update file when not dry-run.
        /// </summary>
        [Fact]
        public void Execute_Patch_SavesFile()
        {
            File.WriteAllText(VersionFileName, "1.0.0");
            var args = new[] { "bump", "patch" };

            BumpCommand.Execute(args);
            var content = File.ReadAllText(VersionFileName).Trim();

            Assert.Equal("1.0.1", content);
        }

        /// <summary>
        /// Invalid bump type should trigger error log.
        /// </summary>
        [Fact]
        public void Execute_InvalidType_LogsError()
        {
            File.WriteAllText(VersionFileName, "1.0.0");
            var args = new[] { "bump", "banana" };

            // Should not throw
            BumpCommand.Execute(args);
        }
    }
}
