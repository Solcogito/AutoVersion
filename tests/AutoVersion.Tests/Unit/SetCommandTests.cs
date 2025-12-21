// ============================================================================
// File:        SetCommandTests.cs
// Project:     AutoVersion.Tests
// ============================================================================

using Moq;
using Xunit;

using AutoVersion.Tests;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public sealed class SetCommandTests
    {
        private static Logger MakeLogger()
        {
            var sink = new TestLogSink();
            return new Logger()
                .WithMinimumLevel(LogLevel.Debug)
                .WithSink(sink);
        }

        [Fact]
        public void Set_DryRun_DoesNotWrite_Returns0()
        {
            var logger = MakeLogger();
            var env = VersionEnvironmentMock.Create(logger);

            var schema = TestSchemaFactory.Build();
            var args = new ArgParser().Parse(
                schema,
                new[] { "set", "1.2.3", "--path", "version.txt", "--dry-run" });

            var cli = new TestCliContext(args, schema);

            int code = SetCommand.Execute(env.Object, cli);

            Assert.Equal(0, code);
            env.Verify(
                e => e.WriteVersion(It.IsAny<VersionModel>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}
