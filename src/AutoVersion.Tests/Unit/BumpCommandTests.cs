using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.Common.Versioning;
using Solcogito.Common.ArgForge;
using Solcogito.AutoVersion.Tests.TestUtils;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class BumpCommandTests
    {
        // ---------------------------------------------------------------------
        // FIXED CreateArgs that matches the real ArgResult implementation
        // ---------------------------------------------------------------------
        private static ArgResult CreateArgs(
            string commandName,
            bool dryRun = false,
            string? pre = null)
        {
            var result = new ArgResult();

            result.CommandPath.Add("autoversion");
            result.CommandPath.Add("bump");
            result.CommandPath.Add(commandName);

            if (dryRun)
                result.AddFlag("dry-run");

            if (!string.IsNullOrWhiteSpace(pre))
                result.AddOption("pre", pre);

            return result;
        }

        // ---------------------------------------------------------------------

        [Fact]
        public void Patch_Bump_Should_Use_Environment_And_Write_New_Version()
        {
            var oldVersion = new VersionModel(1, 2, 3);
            var newVersion = new VersionModel(1, 2, 4);

            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion()).Returns(oldVersion);
            env.Setup(e => e.GetVersionFilePath()).Returns("version.txt");

            env.Setup(e => e.WriteVersion(It.IsAny<VersionModel>()))
               .Callback<VersionModel>(v => v.Should().Be(newVersion));

            var args = CreateArgs("patch");

            var exitCode = BumpCommand.Execute(args, env.Object, logger);

            exitCode.Should().Be(0);
            logger.Messages.Should().Contain(m => m.Contains("Version bump complete: 1.2.4"));

            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Once);
        }

        [Fact]
        public void Patch_Bump_DryRun_Should_Not_Write_File()
        {
            var oldVersion = new VersionModel(1, 2, 3);

            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion()).Returns(oldVersion);
            env.Setup(e => e.GetVersionFilePath()).Returns("version.txt");

            var args = CreateArgs("patch", dryRun: true);

            var exitCode = BumpCommand.Execute(args, env.Object, logger);

            exitCode.Should().Be(0);

            logger.DryRun.Should().BeTrue();
            logger.Messages.Should().Contain(m => m.Contains("[DRY-RUN] Would update version: 1.2.3 -> 1.2.4"));

            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Never);
        }

        [Fact]
        public void Prerelease_With_Custom_Tag_Should_Pass_Pre_To_Bumper()
        {
            var oldVersion = new VersionModel(2, 0, 0);

            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion()).Returns(oldVersion);
            env.Setup(e => e.GetVersionFilePath()).Returns("version.txt");

            var args = CreateArgs("prerelease", pre: "cornichon.5");

            var exitCode = BumpCommand.Execute(args, env.Object, logger);

            exitCode.Should().Be(0);
            logger.Messages.Should().Contain(m => m.Contains("cornichon.5"));

            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Once);
        }

        [Fact]
        public void Prerelease_With_Invalid_Pre_Tag_Should_Fail_Validation()
        {
            var oldVersion = new VersionModel(2, 0, 0);

            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion()).Returns(oldVersion);
            env.Setup(e => e.GetVersionFilePath()).Returns("version.txt");

            var args = CreateArgs("prerelease", pre: "!!bad tag!!");

            var exitCode = BumpCommand.Execute(args, env.Object, logger);

            exitCode.Should().Be(1);
            logger.Messages.Should().Contain(m => m.Contains("Invalid prerelease tag"));

            env.Verify(e => e.WriteVersion(It.IsAny<VersionModel>()), Times.Never);
        }

        [Fact]
        public void Unknown_Bump_Type_Should_Return_Error()
        {
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            var args = CreateArgs("weird");

            var exitCode = BumpCommand.Execute(args, env.Object, logger);

            exitCode.Should().Be(1);
            logger.Messages.Should().Contain(m => m.Contains("Unknown bump type"));
        }
    }
}
