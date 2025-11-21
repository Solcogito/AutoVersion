// ============================================================================
// File:        ProgramSchemaTests.cs
// Project:     AutoVersion Lite (Unified Test Suite)
// Version:     0.8.0
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Light-mode tests validating the structure of the CLI schema defined in
//   Program.BuildSchema(). Ensures that all commands, flags, positional
//   arguments, and options are registered correctly, and that help text is
//   generated successfully. No command execution occurs in this file.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Linq;
using FluentAssertions;
using Xunit;

using Solcogito.AutoVersion.Cli;
using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class ProgramSchemaTests
    {
        // -------------------------------------------------------------
        // Helper: access private Program.BuildSchema() via reflection
        // -------------------------------------------------------------
        private static ArgSchema GetSchema()
        {
            var method = typeof(Program)
                .GetMethod("BuildSchema",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static);

            method.Should().NotBeNull("Program.BuildSchema() must exist and remain private");

            return (ArgSchema)method!.Invoke(null, null)!;
        }

        // -------------------------------------------------------------
        // 1. Root command exists and has correct name/description
        // -------------------------------------------------------------
        [Fact]
        public void Schema_Should_Define_Root_Command()
        {
            var schema = GetSchema();
            var root = schema.Root;

            root.Name.Should().Be("autoversion");
            root.Description.Should().Contain("Semantic versioning tool");
        }

        // -------------------------------------------------------------
        // 2. All top-level commands are registered
        // -------------------------------------------------------------
        [Fact]
        public void Schema_Should_Contain_All_Commands()
        {
            var schema = GetSchema();
            var root = schema.Root;

            var commands = root.Subcommands.Values.Select(c => c.Name).ToList();

            commands.Should().Contain(new[] { "current", "set", "bump" });
        }

        // -------------------------------------------------------------
        // 3. "set" command has required positional <version>
        // -------------------------------------------------------------
        [Fact]
        public void Schema_Should_Define_Set_Command_Positional()
        {
            var schema = GetSchema();
            var root = schema.Root;

            var set = root.Subcommands.Values.FirstOrDefault(c => c.Name == "set");
            set.Should().NotBeNull();

            var versionPos = set!.Positionals.FirstOrDefault(p => p.Name == "version");
            versionPos.Should().NotBeNull();
            versionPos!.Required.Should().BeTrue();
        }

        // -------------------------------------------------------------
        // 4. bump command has all expected subcommands
        // -------------------------------------------------------------
        [Fact]
        public void Schema_Should_Define_Bump_Subcommands()
        {
            var schema = GetSchema();
            var root = schema.Root;

            var bump = root.Subcommands.Values.First(c => c.Name == "bump");

            var subs = bump.Subcommands.Values.Select(c => c.Name).ToList();

            subs.Should().Contain(new[] { "patch", "minor", "major", "prerelease" });
        }

        // -------------------------------------------------------------
        // 5. prerelease option (-p / --pre) exists
        // -------------------------------------------------------------
        [Fact]
        public void Schema_Should_Define_PreRelease_Option()
        {
            var schema = GetSchema();
            var root = schema.Root;

            var bump = root.Subcommands.Values.First(c => c.Name == "bump");
            var prerelease = bump.Subcommands.Values.First(c => c.Name == "prerelease");

            var option = prerelease.Options.FirstOrDefault(o => o.Name == "pre");
            option.Should().NotBeNull("prerelease should expose a --pre option");

            option!.Aliases.Should().Contain("-p");
            option.Aliases.Should().Contain("--pre");
        }

        // -------------------------------------------------------------
        // 6. Global --dry-run flag exists
        // -------------------------------------------------------------
        [Fact]
        public void Schema_Should_Define_DryRun_Flag()
        {
            var schema = GetSchema();
            var root = schema.Root;

            var flag = root.Flags.FirstOrDefault(f => f.Name == "dry-run");
            flag.Should().NotBeNull();

            flag!.Aliases.Should().Contain("--dry-run");
        }

        // -------------------------------------------------------------
        // 7. Help text is generated and contains "Usage"
        // -------------------------------------------------------------
        [Fact]
        public void Schema_Should_Generate_Help_Text()
        {
            var schema = GetSchema();

            var help = schema.GetHelp("autoversion");

            help.Should().NotBeNullOrWhiteSpace();
            help.Should().Contain("Usage");
            help.Should().Contain("current");
            help.Should().Contain("set");
            help.Should().Contain("bump");
        }
    }
}
