// ============================================================================
// File:        AllTests.cs
// Project:     AutoVersion Lite (Unit Tests)
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Assembly-level smoke tests to ensure the test project is wired correctly.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using FluentAssertions;

using Xunit;

namespace Solcogito.AutoVersion.Tests.Unit
{
    [Collection(GlobalTestCollection.Name)]
    public sealed class AllTests
    {
        [Fact]
        public void Test_Project_Loads()
        {
            true.Should().BeTrue();
        }
    }
}
