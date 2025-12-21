// ============================================================================
// File:        ProgramSchemaTests.cs
// Project:     AutoVersion Lite (Unit Tests)
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Validates CLI schema presence by inspecting the help text emitted by
//   Program.BuildSchema().
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using AutoVersion.Tests.TestUtils;

using FluentAssertions;

using Solcogito.AutoVersion.Cli;
using Solcogito.Common.ArgForge;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    [Collection(GlobalTestCollection.Name)]
    public sealed class ProgramSchemaTests
    {
        [Fact]
        public void Program_BuildSchema_Should_Contain_Core_Commands_In_Help()
        {
            ArgSchema schema = ReflectionUtils.InvokePrivateStatic<ArgSchema>(
                typeof(Program),
                "BuildSchema");

            string help = schema.GetHelp();

            help.Should().Contain("autoversion");
            help.Should().Contain("current");
            help.Should().Contain("set");
            help.Should().Contain("bump");
            help.Should().Contain("--dry-run");
        }
    }
}
