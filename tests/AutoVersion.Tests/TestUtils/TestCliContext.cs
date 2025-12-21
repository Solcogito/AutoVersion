// ============================================================================
// File:        TestCliContext.cs
// Project:     AutoVersion.Tests
// Author:      Solcogito S.E.N.C.
// ============================================================================
// Description:
//     Test implementation of ICliContext.
//     Provides an in-memory output sink for deterministic assertions.
// ============================================================================
// License:     MIT
// ============================================================================

using Solcogito.AutoVersion.Cli;
using Solcogito.Common.ArgForge;
using Solcogito.Common.IOKit;

namespace AutoVersion.Tests
{
    internal sealed class TestCliContext : ICliContext
    {
        public ArgResult Args { get; }
        public ArgSchema Schema { get; }
        public HelpFormatter Help { get; }

        // MUST be public to satisfy ICliContext
        public ITextSink Output { get; }

        public TestCliContext(ArgResult args, ArgSchema schema)
        {
            Args = args;
            Schema = schema;
            Help = new HelpFormatter();

            // Deterministic, in-memory output capture
            Output = new BufferTextSink();
        }
    }
}
