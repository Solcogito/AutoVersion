using System.Linq;

using FluentAssertions;

using Moq;

using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class CurrentCommandTests
    {
        private Logger MakeLogger(out TestLogSink sink)
        {
            sink = new TestLogSink();
            return new Logger().WithSink(sink);
        }

        private static VersionResolutionResult MakeResult(int major, int minor, int patch)
        {
            return new VersionResolutionResult
            {
                Version = new VersionModel(major, minor, patch),
                FilePath = null,
                Source = "test",
                Success = true
            };
        }

        // -------------------------------------------------------------
        // SUCCESS PATH
        // -------------------------------------------------------------
        [Fact]
        public void Current_Should_Print_Version_And_Return0()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion()).Returns(MakeResult(1, 2, 3));

            var logger = MakeLogger(out var sink);

            var code = CurrentCommand.Execute(new ArgResult(), env.Object, logger);

            code.Should().Be(0);

            // Check logger output instead of console
            sink.Messages.Any(m => m.Text.Contains("1.2.3")).Should().BeTrue();
        }

        // -------------------------------------------------------------
        // ERROR PATH
        // -------------------------------------------------------------
        [Fact]
        public void Current_Should_Return1_On_Exception()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Throws(new System.Exception("boom"));

            var logger = MakeLogger(out var sink);

            var code = CurrentCommand.Execute(new ArgResult(), env.Object, logger);

            code.Should().Be(1);

            sink.Messages.Any(m => m.Text.Contains("Error")).Should().BeTrue();
        }
    }
}
