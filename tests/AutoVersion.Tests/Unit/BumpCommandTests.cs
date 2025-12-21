// ============================================================================
// File:        BumpCommandTests.cs
// Project:     AutoVersion.Tests
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Moq;
using Xunit;

using AutoVersion.Tests;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.Errors;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public sealed class BumpCommandTests
    {
        private static (Logger logger, TestLogSink sink) MakeLogger()
        {
            var sink = new TestLogSink();
            var logger = new Logger()
                .WithMinimumLevel(LogLevel.Debug)
                .WithSink(sink);

            return (logger, sink);
        }

        [Fact]
        public void BumpPatch_DryRun_DoesNotWrite_Returns0()
        {
            var (logger, _) = MakeLogger();
            var env = VersionEnvironmentMock.Create(logger);

            env.Setup(e => e.GetCurrentVersions(It.IsAny<string>()))
               .Returns(new VersionResolveResult(
                   checkedSources: new[] { "version.txt" },
                   successfulVersions: new[] { VersionModel.Parse("1.2.3") },
                   errors: Array.Empty<ErrorInfo>(),
                   final: VersionModel.Parse("1.2.3")
               ));

            var schema = TestSchemaFactory.Build();
            var args = new ArgParser().Parse(
                schema,
                new[] { "bump", "patch", "--path", "version.txt", "--dry-run" });

            var cli = new TestCliContext(args, schema);

            int code = BumpCommand.Execute(env.Object, cli);

            Assert.Equal(0, code);
            env.Verify(
                e => e.WriteVersion(It.IsAny<VersionModel>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public void BumpPatch_Writes_Returns0()
        {
            var (logger, _) = MakeLogger();
            var env = VersionEnvironmentMock.Create(logger);

            env.Setup(e => e.GetCurrentVersions(It.IsAny<string>()))
               .Returns(new VersionResolveResult(
                   checkedSources: new[] { "version.txt" },
                   successfulVersions: new[] { VersionModel.Parse("1.2.3") },
                   errors: Array.Empty<ErrorInfo>(),
                   final: VersionModel.Parse("1.2.3")
               ));

            env.Setup(e =>
                e.WriteVersion(
                    It.Is<VersionModel>(v => v.ToString() == "1.2.4"),
                    It.Is<string>(p => p.EndsWith("version.txt"))
                ));

            var schema = TestSchemaFactory.Build();
            var args = new ArgParser().Parse(
                schema,
                new[] { "bump", "patch", "--path", "version.txt" });

            var cli = new TestCliContext(args, schema);

            int code = BumpCommand.Execute(env.Object, cli);

            Assert.Equal(0, code);

            env.Verify(
                e => e.WriteVersion(
                    It.Is<VersionModel>(v => v.ToString() == "1.2.4"),
                    It.Is<string>(p => p.EndsWith("version.txt"))
                ),
                Times.Once);
        }
    }
}
