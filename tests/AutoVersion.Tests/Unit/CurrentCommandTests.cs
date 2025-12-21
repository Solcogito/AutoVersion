// ============================================================================
// File:        CurrentCommandTests.cs
// Project:     AutoVersion.Tests
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;

using AutoVersion.Tests;

using FluentAssertions;

using Moq;

using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.Errors;
using Solcogito.Common.IOKit;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public sealed class CurrentCommandTests
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
        public void Current_PrintsVersion_Returns0()
        {
            var (logger, logSink) = MakeLogger();
            var env = VersionEnvironmentMock.Create(logger);

            env.Setup(e => e.GetCurrentVersions(null))
               .Returns(new VersionResolveResult(
                   checkedSources: new[] { "test" },
                   successfulVersions: new[] { VersionModel.Parse("1.2.3") },
                   errors: Array.Empty<ErrorInfo>(),
                   final: VersionModel.Parse("1.2.3")
               ));

            var schema = TestSchemaFactory.Build();
            var args = new ArgParser().Parse(schema, new[] { "current" });
            var cli = new TestCliContext(args, schema);

            int code = CurrentCommand.Execute(env.Object, cli);

            code.Should().Be(0);

            var outputSink = (BufferTextSink)cli.Output;
            outputSink.Content.Should().Contain("1.2.3");
        }

        [Fact]
        public void Current_Returns2_OnException()
        {
            var (logger, sink) = MakeLogger();
            var env = VersionEnvironmentMock.Create(logger);

            env.Setup(e => e.GetCurrentVersions(null))
               .Throws(new Exception("boom"));

            var schema = TestSchemaFactory.Build();
            var args = new ArgParser().Parse(schema, new[] { "current" });
            var cli = new TestCliContext(args, schema);

            int code = CurrentCommand.Execute(env.Object, cli);

            code.Should().Be(2);
            sink.Content.Should().NotBeEmpty();
        }
    }
}
