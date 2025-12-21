// ============================================================================
// File:        CommandRouterTests.cs
// Project:     AutoVersion.Tests
// ============================================================================

using AutoVersion.Tests;

using Moq;

using Solcogito.AutoVersion.Cli;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public sealed class CommandRouterTests
    {
        private static Logger MakeLogger()
        {
            var sink = new TestLogSink();
            return new Logger()
                .WithMinimumLevel(LogLevel.Debug)
                .WithSink(sink);
        }

        [Fact]
        public void Run_Current_RoutesToCurrent()
        {
            var logger = MakeLogger();
            var env = VersionEnvironmentMock.Create(logger);

            var version = VersionModel.Parse("1.2.3");

            env.Setup(e => e.GetCurrentVersions(null))
               .Returns(new VersionResolveResult(
                   new[] { "test" },
                   new[] { version },
                   Array.Empty<Solcogito.Common.Errors.ErrorInfo>(),
                   version
               ));

            var schema = TestSchemaFactory.Build();
            var args = new ArgParser().Parse(schema, new[] { "current" });
            var cli = new TestCliContext(args, schema);

            int code = CommandRouter.Run(env.Object, cli);

            Assert.Equal(0, code);
        }
    }
}
