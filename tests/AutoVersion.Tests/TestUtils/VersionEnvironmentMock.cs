// ============================================================================
// File:        VersionEnvironmentMock.cs
// Project:     AutoVersion.Tests
// ----------------------------------------------------------------------------
// Description:
//   Test helper for creating strict mocks of IVersionEnvironment.
//   This enforces the architectural rule that tests must never depend
//   on DefaultVersionEnvironment or any concrete implementation.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Moq;

using Solcogito.AutoVersion.Core;
using Solcogito.Common.LogScribe;

namespace Solcogito.AutoVersion.Tests.TestUtils
{
    internal static class VersionEnvironmentMock
    {
        /// <summary>
        /// Creates a strict mock of <see cref="IVersionEnvironment"/> with
        /// common properties preconfigured.
        /// </summary>
        /// <remarks>
        /// Architectural invariant:
        /// - Tests must mock interfaces only.
        /// - DefaultVersionEnvironment is sealed and must never be mocked.
        /// </remarks>
        public static Mock<IVersionEnvironment> Create(
            Logger logger,
            bool allowNormalize = true,
            string context = "test")
        {
            var env = new Mock<IVersionEnvironment>(MockBehavior.Strict);

            env.SetupGet(e => e.Logger).Returns(logger);
            env.SetupGet(e => e.AllowNormalize).Returns(allowNormalize);
            env.SetupGet(e => e.Context).Returns(context);

            return env;
        }
    }
}
