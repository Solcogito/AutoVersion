// ============================================================================
// File:        GlobalTestConfiguration.cs
// Project:     AutoVersion Lite (Unit Tests)
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Global test configuration for the AutoVersion test suite.
//   Forces invariant culture and predictable environment behavior so tests are
//   deterministic across machines and CI.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Globalization;

using Xunit;

namespace Solcogito.AutoVersion.Tests
{
    [CollectionDefinition(Name)]
    public sealed class GlobalTestCollection : ICollectionFixture<GlobalTestConfiguration>
    {
        public const string Name = "GlobalTestCollection";
    }

    public sealed class GlobalTestConfiguration
    {
        public GlobalTestConfiguration()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
