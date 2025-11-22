// ============================================================================
// File:        SetCommandTests.cs
// Project:     AutoVersion Lite (Unit Tests)
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// -----------------------------------------------------------------------------
// Description:
//   Unit tests for SetCommand. Covers successful version writes, dry-run
//   behavior, missing or invalid version inputs, empty version file paths, and
//   exception handling when write operations fail. Ensures that correct exit
//   codes and logger messages are produced and that environment methods are
//   invoked as expected.
// ============================================================================

using FluentAssertions;
using Moq;
using Xunit;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.Common.ArgForge;
using Solcogito.Common.Versioning;
using Solcogito.AutoVersion.Tests.TestUtils;
using System.IO;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class SetCommandTests
    {
        // ---------------------------------------------------------------------
        // Helper to build ArgResult for `autoversion set <version> [--dry-run]`
        // ---------------------------------------------------------------------
        private static ArgResult CreateArgs(string? version, bool dryRun = false)
        {
            var result = new ArgResult();

            // Command path: autoversion set
            result.CommandPath.Add("autoversion");
            result.CommandPath.Add("set");

            if (!string.IsNullOrWhiteSpace(version))
            {
                // Positional "version"
                result.AddPositional("version", version);
            }

            if (dryRun)
            {
                result.AddFlag("dry-run");
            }

            return result;
        }

        private static VersionResolutionResult MakeResult(string filePath)
        {
            return new VersionResolutionResult
            {
                Version = new VersionModel(0, 0, 0),
                Source = string.IsNullOrWhiteSpace(filePath) ? "default" : filePath,
                FilePath = filePath,
                Success = !string.IsNullOrWhiteSpace(filePath)
            };
        }

        // ---------------------------------------------------------------------
        // 1. Happy path: valid version, normal write
        // ---------------------------------------------------------------------
        [Fact]
        public void Set_WithValidVersion_Should_Write_And_Return0()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult("version.txt"));

            VersionModel? written = null;
            env.Setup(e => e.WriteVersion(It.IsAny<VersionModel>()))
               .Callback<VersionModel>(v => written = v);

            var args = CreateArgs("1.2.3");

            // Act
            var exitCode = SetCommand.Execute(args, env.Object, logger);

            // Assert
            exitCode.Should().Be(0);
            written.Should().NotBeNull();
            written!.ToString().Should().Be("1.2.3");

            env.Verify(e => e.GetCurrentVersion(), Times.Once);
            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Once);
        }

        // ---------------------------------------------------------------------
        // 2. Dry-run: no write, still success
        // ---------------------------------------------------------------------
        [Fact]
        public void Set_WithDryRun_Should_Not_Write_File()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult("version.txt"));

            var args = CreateArgs("1.2.3", dryRun: true);

            // Act
            var exitCode = SetCommand.Execute(args, env.Object, logger);

            // Assert
            exitCode.Should().Be(0);

            env.Verify(e => e.GetCurrentVersion(), Times.Once);
            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Never);

            // SetCommand itself doesn't toggle DryRun flag on logger
            logger.DryRun.Should().BeFalse();
            logger.Messages.Any(m => m.Contains("[DRY-RUN]") || m.Contains("DRY-RUN"))
                  .Should().BeTrue();
        }

        // ---------------------------------------------------------------------
        // 3. Missing version positional → error code 1, no env usage
        // ---------------------------------------------------------------------
        [Fact]
        public void Set_WithMissingVersion_Should_Return1_And_Not_Touch_Env()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>(MockBehavior.Strict);
            var logger = new FakeCliLogger();

            var args = CreateArgs(null);

            // Act
            var exitCode = SetCommand.Execute(args, env.Object, logger);

            // Assert
            exitCode.Should().Be(1);
            env.VerifyNoOtherCalls();
        }

        // ---------------------------------------------------------------------
        // 4. Invalid version string → error code 1, no env usage
        // ---------------------------------------------------------------------
        [Fact]
        public void Set_WithInvalidVersion_Should_Return1_And_Not_Touch_Env()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>(MockBehavior.Strict);
            var logger = new FakeCliLogger();

            var args = CreateArgs("not-a-version");

            // Act
            var exitCode = SetCommand.Execute(args, env.Object, logger);

            // Assert
            exitCode.Should().Be(1);
            env.VerifyNoOtherCalls();
        }

        // ---------------------------------------------------------------------
        // 5. Empty version file path → error code 2, no write
        // ---------------------------------------------------------------------
        [Fact]
        public void Set_WithEmptyVersionFilePath_Should_Return2_And_Not_Write()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult(string.Empty));

            var args = CreateArgs("1.2.3");

            // Act
            var exitCode = SetCommand.Execute(args, env.Object, logger);

            // Assert
            exitCode.Should().Be(2);
            env.Verify(e => e.GetCurrentVersion(), Times.Once);
            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Never);
        }

        // ---------------------------------------------------------------------
        // 6. WriteVersion throws → error code 2
        // ---------------------------------------------------------------------
        [Fact]
        public void Set_When_WriteVersion_Throws_Should_Return2()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult("version.txt"));

            env.Setup(e => e.WriteVersion(It.IsAny<VersionModel>()))
               .Throws(new IOException("disk full"));

            var args = CreateArgs("1.2.3");

            // Act
            var exitCode = SetCommand.Execute(args, env.Object, logger);

            // Assert
            exitCode.Should().Be(2);
            env.Verify(e => e.GetCurrentVersion(), Times.Once);
            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Once);

            logger.Messages.Should().Contain(m => m.Contains("Error setting version"));
        }
    }
}
