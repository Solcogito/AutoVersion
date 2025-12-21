// ============================================================================
// File:        ICliContext.cs
// Project:     AutoVersion Lite
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   CLI-only execution context.
//   - Parsed args (ArgResult)
//   - Schema (ArgSchema)
//   - Help formatting surface
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Solcogito.Common.ArgForge;
using Solcogito.Common.IOKit;

namespace Solcogito.AutoVersion.Cli
{
    /// <summary>
    /// CLI-only context required by command handlers that interpret user arguments.
    /// </summary>
    /// <remarks>
    /// This interface intentionally contains presentation and parsing concerns.
    /// It must never leak into the domain environment interface.
    ///
    /// Commands that accept <see cref="ICliContext"/> are CLI handlers,
    /// not pure domain commands.
    /// </remarks>
    internal interface ICliContext
    {
        ArgResult Args { get; }
        ArgSchema Schema { get; }
        HelpFormatter Help { get; }

        ITextSink Output { get; }
    }
}
