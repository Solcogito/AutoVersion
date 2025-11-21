// ============================================================================
// File:        CurrentCommandTests.cs
// Project:     AutoVersion Lite (Unified Test Suite)
// Version:     0.8.0
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Unit tests for the CurrentCommand. Verifies that the command prints the
//   correct version, handles exceptions gracefully, and returns the proper
//   exit codes. Fully isolated using ConsoleCapture to avoid inter-test
//   pollution caused by global Console writes.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;

using FluentAssertions;

using Moq;

using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.Versioning;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class CurrentCommandTests
    {
        private ArgResult MakeArgs() => new ArgResult();

        // -------------------------------------------------------------
        // 1. Happy path: prints version + returns 0
        // -------------------------------------------------------------
        [Fact]
        public void Current_Should_Print_Version_And_Return0()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Returns(new VersionModel(1, 2, 3));

            var args = MakeArgs();

            using var cap = new ConsoleCapture();

            // Act
            var code = CurrentCommand.Execute(args, env.Object, logger);
            var output = cap.OutWriter.ToString();

            // Assert: assert ONLY last printed line (safe across test runs)
            code.Should().Be(0);
            output.Trim().Split('\n').Last().Trim().Should().Be("1.2.3");
        }

        // -------------------------------------------------------------
        // 2. Environment throws → return 1 + error logged
        // -------------------------------------------------------------
        [Fact]
        public void Current_Should_Fail_And_Return1_When_EnvThrows()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Throws(new Exception("Test failure"));

            var args = MakeArgs();

            using var cap = new ConsoleCapture();

            // Act
            var code = CurrentCommand.Execute(args, env.Object, logger);
            var error = cap.ErrWriter.ToString();

            // Assert
            code.Should().Be(1);
            error.Should().Contain("Test failure");
            logger.Messages.Should().Contain(m => m.Contains("Error reading current version"));
        }
    }
}
