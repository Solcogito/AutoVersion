using AutoVersion.Tests.TestUtils;

using FluentAssertions;

using Solcogito.AutoVersion.Cli;
using Solcogito.Common.ArgForge;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    public sealed class BumpSchemaTests
    {
        [Fact]
        public void Bump_Command_Should_Expose_Version_Component_Subcommands()
        {
            ArgSchema schema = ReflectionUtils.InvokePrivateStatic<ArgSchema>(
                typeof(Program),
                "BuildSchema");

            string help = schema.GetHelpFor(new[] { "bump" });

            help.Should().Contain("patch");
            help.Should().Contain("minor");
            help.Should().Contain("major");
            help.Should().Contain("prerelease");
        }
    }
}
