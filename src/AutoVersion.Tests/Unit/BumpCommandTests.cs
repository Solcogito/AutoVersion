// ============================================================================
// File:        BumpCommandTests.cs
// Project:     AutoVersion Lite (Unit Tests)
// Author:      Solcogito S.E.N.C.
// -----------------------------------------------------------------------------
// Description:
//   Fully updated test suite for BumpCommand using real LogScribe.Logger +
//   TestLogSink. Covers: patch/minor/major bumps, prerelease bumps, dry-run,
//   invalid prerelease identifiers, missing file path, exceptions, and unknown
//   bump types. Ensures correct interaction with IVersionEnvironment and that
//   all log output passes through Logger and its sinks.
// ============================================================================

using System;
using FluentAssertions;
using Moq;
using Xunit;

using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.Common.Versioning;
using Solcogito.Common.ArgForge;
using Solcogito.Common.LogScribe;

using Solcogito.AutoVersion.Tests.TestUtils;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class BumpCommandTests
    {
        // ---------------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------------

        private static ArgResult MakeArgs(string bumpType, bool dryRun = false, string? pre = null)
        {
            var r = new ArgResult();
            r.CommandPath.Add("autoversion");
            r.CommandPath.Add("bump");
            r.CommandPath.Add(bumpType);

            if (dryRun)
                r.AddFlag("dry-run");

            if (!string.IsNullOrWhiteSpace(pre))
                r.AddOption("pre", pre);

            return r;
        }

        private static VersionResolutionResult MakeVersion(string content, string path = "version.txt")
        {
            return new VersionResolutionResult
            {
                Version = VersionModel.Parse(content),
                Success = true,
                FilePath = path,
                Source = path
            };
        }

        private Logger CreateLogger(out TestLogSink sink)
        {
            sink = new TestLogSink();
            return new Logger().WithSink(sink);
        }

        // ---------------------------------------------------------------------
        // PATCH / MINOR / MAJOR
        // ---------------------------------------------------------------------
        [Theory]
        [InlineData("patch", "1.2.3", "1.2.4")]
        [InlineData("minor", "1.2.3", "1.3.0")]
        [InlineData("major", "1.2.3", "2.0.0")]
        public void Bump_Should_UpdateVersion(string type, string start, string expected)
        {
            var args = MakeArgs(type);
            var logger = CreateLogger(out var sink);

            var env = new Mock<IVersionEnvironment>();
            env.Setup(x => x.GetCurrentVersion()).Returns(MakeVersion(start));

            int code = BumpCommand.Execute(args, env.Object, logger);

            code.Should().Be(0);
            env.Verify(x => x.WriteVersion(VersionModel.Parse(expected)), Times.Once);

            sink.Messages.Should().Contain(m => m.Text.Contains("Version bump complete"));
        }

        // ---------------------------------------------------------------------
        // DRY RUN
        // ---------------------------------------------------------------------
        [Fact]
        public void Bump_DryRun_Should_Not_WriteFile()
        {
            var args = MakeArgs("patch", dryRun: true);
            var logger = CreateLogger(out var sink);

            var env = new Mock<IVersionEnvironment>();
            env.Setup(x => x.GetCurrentVersion()).Returns(MakeVersion("1.0.0"));

            int code = BumpCommand.Execute(args, env.Object, logger);

            code.Should().Be(0);
            env.Verify(x => x.WriteVersion(It.IsAny<VersionModel>()), Times.Never);

            sink.Messages.Should().Contain(m => m.Text.Contains("[DRY-RUN]"));
        }

        // ---------------------------------------------------------------------
        // UNKNOWN BUMP TYPE
        // ---------------------------------------------------------------------
        [Fact]
        public void Bump_UnknownType_Should_Return1()
        {
            var args = MakeArgs("ketchup");
            args.CommandPath[2] = "ketchup";

            var logger = CreateLogger(out var sink);
            var env = new Mock<IVersionEnvironment>();

            int code = BumpCommand.Execute(args, env.Object, logger);

            code.Should().Be(1);

            sink.Messages.Should().Contain(m => m.Text.Contains("Unknown bump type"));
        }

        // ---------------------------------------------------------------------
        // FILE PATH MISSING
        // ---------------------------------------------------------------------
        [Fact]
        public void Bump_Should_Fail_When_FilePath_Is_Empty()
        {
            var args = MakeArgs("patch");
            var logger = CreateLogger(out var sink);

            var env = new Mock<IVersionEnvironment>();
            env.Setup(x => x.GetCurrentVersion()).Returns(
                new VersionResolutionResult
                {
                    Version = VersionModel.Parse("1.0.0"),
                    FilePath = "",
                    Success = true
                });

            int code = BumpCommand.Execute(args, env.Object, logger);

            code.Should().Be(2);
            sink.Messages.Should().Contain(m => m.Text.Contains("path is empty"));
        }

        // ---------------------------------------------------------------------
        // PRERELEASE BUMP
        // ---------------------------------------------------------------------
        [Fact]
        public void Prerelease_Should_Bump_With_Valid_Tag()
        {
            var args = MakeArgs("prerelease", pre: "alpha.5");
            var logger = CreateLogger(out var sink);

            var env = new Mock<IVersionEnvironment>();
            env.Setup(x => x.GetCurrentVersion()).Returns(MakeVersion("1.2.3"));

            int code = BumpCommand.Execute(args, env.Object, logger);

            code.Should().Be(0);

            env.Verify(x => x.WriteVersion(VersionModel.Parse("1.2.3-alpha.5")), Times.Once);
        }

        // ---------------------------------------------------------------------
        // INVALID PRERELEASE TAG
        // ---------------------------------------------------------------------
        [Fact]
        public void Prerelease_InvalidTag_Should_Return1()
        {
            var args = MakeArgs("prerelease", pre: "%%BAD%%");
            var logger = CreateLogger(out var sink);

            var env = new Mock<IVersionEnvironment>();

            int code = BumpCommand.Execute(args, env.Object, logger);

            code.Should().Be(1);

            sink.Messages.Should().Contain(m => m.Text.Contains("Invalid prerelease tag"));
        }

        // ---------------------------------------------------------------------
        // EXCEPTIONS during bumping
        // ---------------------------------------------------------------------
        [Fact]
        public void Bump_Should_Handle_Exceptions()
        {
            var args = MakeArgs("patch");
            var logger = CreateLogger(out var sink);

            var env = new Mock<IVersionEnvironment>();
            env.Setup(x => x.GetCurrentVersion()).Throws(new Exception("File locked"));

            int code = BumpCommand.Execute(args, env.Object, logger);

            code.Should().Be(2);

            sink.Messages.Should().Contain(m => m.Text.Contains("Bump failed"));
        }
    }
}
