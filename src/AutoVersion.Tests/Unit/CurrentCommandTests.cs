using System;
using System.IO;
using FluentAssertions;
using Moq;
using Xunit;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.Common.ArgForge;
using Solcogito.Common.Versioning;
using Solcogito.AutoVersion.Tests.TestUtils;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public class CurrentCommandTests
    {
        // ---------------------------------------------------------------
        // Helper to create a minimal ArgResult for "autoversion current"
        // ---------------------------------------------------------------
        private static ArgResult CreateArgs()
        {
            var result = new ArgResult();
            result.CommandPath.Add("autoversion");
            result.CommandPath.Add("current");
            return result;
        }

        // ---------------------------------------------------------------
        // 1. Happy path: prints version to console
        // ---------------------------------------------------------------
        [Fact]
        public void Current_Should_Print_Version_And_Return0()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Returns(new VersionModel(1, 2, 3));

            var args = CreateArgs();

            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            var exitCode = CurrentCommand.Execute(args, env.Object, logger);
            var output = sw.ToString().Trim();

            // Assert
            exitCode.Should().Be(0);
            output.Should().Be("1.2.3");
        }

        // ---------------------------------------------------------------
        // 2. When env fails → prints to Console.Error and logs error
        // ---------------------------------------------------------------
        [Fact]
        public void Current_When_Env_Throws_Should_Return1_And_LogError()
        {
            // Arrange
            var env = new Mock<IVersionEnvironment>();
            var logger = new FakeCliLogger();

            env.Setup(e => e.GetCurrentVersion())
               .Throws(new Exception("disk exploded"));

            var args = CreateArgs();

            using var swOut = new StringWriter();
            using var swErr = new StringWriter();
            Console.SetOut(swOut);
            Console.SetError(swErr);

            // Act
            var exitCode = CurrentCommand.Execute(args, env.Object, logger);

            // Assert
            exitCode.Should().Be(1);

            swErr.ToString().Should().Contain("disk exploded");
            logger.Messages.Should().Contain(m => m.Contains("Error reading current version"));
        }
    }
}
