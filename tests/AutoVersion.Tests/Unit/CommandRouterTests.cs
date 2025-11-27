using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

using Solcogito.AutoVersion.Cli;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.Versioning;
using Solcogito.Common.LogScribe;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class CommandRouterTests
    {
        private static ArgResult Make(params string[] segments)
        {
            var r = new ArgResult();
            foreach (var s in segments)
                r.CommandPath.Add(s);
            return r;
        }

        private static VersionResolutionResult MakeResult(
            int major, int minor, int patch,
            string? path = "version.txt", bool success = true)
        {
            return new VersionResolutionResult
            {
                Version = new VersionModel(major, minor, patch),
                Source = path ?? "test",
                FilePath = path,
                Success = success
            };
        }

        private static Logger MakeLogger(out TestLogSink sink)
        {
            sink = new TestLogSink();
            return new Logger().WithSink(sink);
        }

        private static ArgSchema BuildSchema()
        {
            var schema = ArgSchema.Create("autoversion", "Semantic versioning tool");

            schema.Flag("dry-run", null, "--dry-run", "Dry-run mode");

            schema.Command("current");
            schema.Command("set", "Set version", cmd =>
            {
                cmd.Positional("version", 0, "Version", required: true);
            });

            schema.Command("bump", "Increment version", bump =>
            {
                bump.Command("patch");
                bump.Command("minor");
                bump.Command("major");
                bump.Command("prerelease", null, pre =>
                {
                    pre.Option("pre", "-p", "--pre", "Prerelease tag", false);
                });
            });

            return schema;
        }

        // -------------------------------------------------------------
        // bump patch
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Invoke_BumpCommand_For_Patch()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult(1, 2, 3));

            var logger = MakeLogger(out var sink);

            var args = Make("autoversion", "bump", "patch");
            var schema = BuildSchema();

            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(0);
            sink.Messages.Any(m => m.Text.Contains("Version bump complete")).Should().BeTrue();
        }

        // -------------------------------------------------------------
        // bump missing type
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Fail_When_BumpType_Is_Missing()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = MakeLogger(out var sink);

            var args = Make("autoversion", "bump");
            var schema = BuildSchema();

            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(1);
            sink.Messages.Any(m => m.Text.Contains("Usage")).Should().BeTrue();
        }

        // -------------------------------------------------------------
        // current
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Invoke_CurrentCommand()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult(9, 9, 9));

            var logger = MakeLogger(out var sink);

            var args = Make("autoversion", "current");
            var schema = BuildSchema();

            var code = CommandRouter.Run(args, schema, env.Object, logger);

            // Look for "9.9.9" in the logs
            sink.Messages.Any(m => m.Text.Contains("9.9.9")).Should().BeTrue();

            code.Should().Be(0);
        }

        // -------------------------------------------------------------
        // set 1.2.3
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Invoke_SetCommand()
        {
            var env = new Mock<IVersionEnvironment>();
            env.Setup(e => e.GetCurrentVersion())
               .Returns(MakeResult(0, 0, 0));

            VersionModel? written = null;
            env.Setup(e => e.WriteVersion(It.IsAny<VersionModel>()))
               .Callback<VersionModel>(v => written = v);

            var logger = MakeLogger(out var sink);

            var args = Make("autoversion", "set");
            args.AddPositional("version", "1.2.3");

            var schema = BuildSchema();

            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(0);
            written!.ToString().Should().Be("1.2.3");
        }

        // -------------------------------------------------------------
        // unknown command
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Fail_On_Unknown_Command()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = MakeLogger(out var sink);

            var args = Make("autoversion", "potato");
            var schema = BuildSchema();

            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(1);

            sink.Messages.Any(m => m.Text.Contains("Unknown command")).Should().BeTrue();
        }

        // -------------------------------------------------------------
        // root help
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Show_Root_Help_When_No_Command()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = MakeLogger(out var sink);

            var args = Make("autoversion");
            var schema = BuildSchema();

            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(1);

            sink.Messages.Any(m => m.Text.Contains("Usage")).Should().BeTrue();
        }
    }
}
