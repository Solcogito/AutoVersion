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
    public class SetCommandTests
    {
        private Logger MakeLogger(out TestLogSink sink)
        {
            sink = new TestLogSink();
            return new Logger().WithSink(sink);
        }

        private static VersionResolutionResult MakeResult(string path)
        {
            return new VersionResolutionResult
            {
                Version = new VersionModel(0, 0, 0),
                FilePath = path,
                Source = path,
                Success = !string.IsNullOrWhiteSpace(path)
            };
        }

        private static ArgResult Args(string? version, bool dryRun = false)
        {
            var r = new ArgResult();
            r.CommandPath.Add("autoversion");
            r.CommandPath.Add("set");

            if (version != null)
                r.AddPositional("version", version);

            if (dryRun)
                r.AddFlag("dry-run");

            return r;
        }

        [Fact]
        public void Set_Should_Write_Valid_Version()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult("version.txt"));

            VersionModel? written = null;
            env.Setup(e => e.WriteVersion(It.IsAny<VersionModel>()))
               .Callback<VersionModel>(v => written = v);

            var logger = MakeLogger(out _);

            var code = SetCommand.Execute(Args("1.2.3"), env.Object, logger);

            code.Should().Be(0);
            written!.ToString().Should().Be("1.2.3");
        }

        [Fact]
        public void Set_DryRun_Should_Not_Write()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult("version.txt"));

            var logger = MakeLogger(out var sink);

            var code = SetCommand.Execute(Args("1.2.3", true), env.Object, logger);

            code.Should().Be(0);
            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Never);

            sink.Messages.Should().Contain(m => m.Text.Contains("DRY-RUN"));
        }

        [Fact]
        public void Set_MissingVersion_Should_Return1()
        {
            var env = new Mock<IVersionEnvironment>(MockBehavior.Strict);
            var logger = MakeLogger(out _);

            var code = SetCommand.Execute(Args(null), env.Object, logger);

            code.Should().Be(1);
            env.VerifyNoOtherCalls();
        }

        [Fact]
        public void Set_InvalidVersion_Should_Return1()
        {
            var env = new Mock<IVersionEnvironment>(MockBehavior.Strict);
            var logger = MakeLogger(out _);

            var code = SetCommand.Execute(Args("nope"), env.Object, logger);

            code.Should().Be(1);
            env.VerifyNoOtherCalls();
        }

        [Fact]
        public void Set_EmptyPath_Should_Return2()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult(""));

            var logger = MakeLogger(out var sink);

            var code = SetCommand.Execute(Args("1.2.3"), env.Object, logger);

            code.Should().Be(2);
            sink.Messages.Should().Contain(m => m.Text.Contains("empty"));
        }

        [Fact]
        public void Set_WriteThrows_Should_Return2()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult("version.txt"));

            env.Setup(e => e.WriteVersion(It.IsAny<VersionModel>()))
               .Throws(new System.Exception("disk full"));

            var logger = MakeLogger(out var sink);

            var code = SetCommand.Execute(Args("1.2.3"), env.Object, logger);

            code.Should().Be(2);
            sink.Messages.Should().Contain(m => m.Text.Contains("Error setting version"));
        }
    }
}
