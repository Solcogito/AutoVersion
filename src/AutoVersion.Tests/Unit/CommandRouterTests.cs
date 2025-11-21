// ============================================================================
// File:        CommandRouterTests.cs
// Project:     AutoVersion Lite (Unified Test Suite)
// Version:     0.8.0
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Test suite validating the routing behavior of the AutoVersion CLI.
//   Ensures that command dispatching works correctly for 'bump', 'set',
//   and 'current', including error handling, missing arguments, unknown
//   commands, and root help. Fully isolated using mocked dependencies
//   to guarantee deterministic behavior.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Linq;

using FluentAssertions;

using Moq;

using Solcogito.AutoVersion.Cli;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Tests.TestUtils;
using Solcogito.Common.ArgForge;
using Solcogito.Common.Versioning;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class CommandRouterTests
    {
        // -------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------
        private static ArgResult Make(params string[] segments)
        {
            var r = new ArgResult();
            foreach (var s in segments)
                r.CommandPath.Add(s);
            return r;
        }

        // Copy of Program.BuildSchema() (private there)
        private static ArgSchema BuildSchema()
        {
            var schema = ArgSchema.Create("autoversion", "Semantic versioning tool");

            schema.Flag("dry-run", null, "--dry-run", "Simulate operation, do not write files");

            schema.Command("current", "Show the current version");

            schema.Command("set", "Set the version directly", cmd =>
            {
                cmd.Positional("version", 0, "Version to set (e.g., 1.2.3)", required: true);
            });

            schema.Command("bump", "Increment version components", bump =>
            {
                bump.Command("patch", "Increment patch version");
                bump.Command("minor", "Increment minor version");
                bump.Command("major", "Increment major version");

                bump.Command("prerelease", "Increment prerelease version", pre =>
                {
                    pre.Option("pre", "-p", "--pre",
                        "Tag name for prerelease bump (alpha.1, rc.2, etc.)",
                        requiredFlag: false);
                });
            });

            return schema;
        }

        // -------------------------------------------------------------
        // 1. bump patch
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Invoke_BumpCommand_For_Patch()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion()).Returns(new VersionModel(1, 2, 3));
            env.Setup(e => e.GetVersionFilePath()).Returns("version.txt");

            var args = Make("autoversion", "bump", "patch");

            var schema = BuildSchema();
            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(0);
            logger.Messages.Should().Contain(m => m.Contains("Version bump complete"));
        }

        // -------------------------------------------------------------
        // 2. bump missing type
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Fail_When_BumpType_Is_Missing()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            using var cap = new ConsoleCapture();

            var args = Make("autoversion", "bump");
            var schema = BuildSchema();
            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(1);
            cap.OutWriter.ToString().Should().Contain("Usage");
        }

        // -------------------------------------------------------------
        // 3. current
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Invoke_CurrentCommand()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion()).Returns(new VersionModel(9, 9, 9));

            var args = Make("autoversion", "current");

            using var cap = new ConsoleCapture();

            var schema = BuildSchema();
            var code = CommandRouter.Run(args, schema, env.Object, logger);

            var output = cap.OutWriter.ToString();

            code.Should().Be(0);
            output.Trim().Split('\n').Last().Trim().Should().Be("9.9.9");
        }

        // -------------------------------------------------------------
        // 4. set 1.2.3
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Invoke_SetCommand()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            using var cap = new ConsoleCapture();

            env.Setup(e => e.GetVersionFilePath()).Returns("version.txt");

            VersionModel? written = null;
            env.Setup(e => e.WriteVersion(It.IsAny<VersionModel>()))
               .Callback<VersionModel>(v => written = v);

            var args = Make("autoversion", "set");
            args.AddPositional("version", "1.2.3");

            var schema = BuildSchema();
            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(0);
            written!.ToString().Should().Be("1.2.3");
        }

        // -------------------------------------------------------------
        // 5. unknown command
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Fail_On_Unknown_Command()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            using var cap = new ConsoleCapture();

            var args = Make("autoversion", "potato");
            var schema = BuildSchema();
            var code = CommandRouter.Run(args, schema, env.Object, logger);

            code.Should().Be(1);
            cap.OutWriter.ToString().Should().Contain("Unknown command");
        }

        // -------------------------------------------------------------
        // 6. root help
        // -------------------------------------------------------------
        [Fact]
        public void Router_Should_Show_Root_Help_When_No_Command()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            using var cap = new ConsoleCapture();

            var args = Make("autoversion");
            var schema = BuildSchema();
            var code = CommandRouter.Run(args, schema, env.Object, logger);

            var output = cap.OutWriter.ToString();

            code.Should().Be(1);
            output.Should().NotBeNullOrWhiteSpace();
            output.Should().Contain("Usage");
        }
    }
}
