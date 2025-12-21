// ============================================================================
// File:        DefaultCliContext.cs
// Project:     AutoVersion Lite
// Author:      Solcogito S.E.N.C.
// ============================================================================
// Description:
//     Default runtime implementation of ICliContext for CLI execution.
//     Wires parsed arguments, schema, help surface, and output sink.
// ============================================================================
// License:     MIT
// ============================================================================

using Solcogito.AutoVersion.Cli;
using Solcogito.Common.ArgForge;
using Solcogito.Common.IOKit;

namespace Solcogito.AutoVersion.Cli
{
    internal sealed class DefaultCliContext : ICliContext
    {
        public ArgResult Args { get; }
        public ArgSchema Schema { get; }
        public HelpFormatter Help { get; }

        public ITextSink Output { get; }

        public DefaultCliContext(
            ArgResult args,
            ArgSchema schema,
            HelpFormatter help,
            ITextSink output)
        {
            Args = args;
            Schema = schema;
            Help = help;
            Output = output;
        }
    }
}
