// ============================================================================
// File:        CurrentCommand.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Implements the 'current' subcommand, displaying the current version
//   as stored in version.txt (or default if none exists).
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class CurrentCommand
    {
        /// <summary>
        /// Displays the current project version.
        /// </summary>
        public static void Execute()
        {
            var version = VersionResolver.ResolveVersion();
            Console.WriteLine(version);
        }
    }
}
