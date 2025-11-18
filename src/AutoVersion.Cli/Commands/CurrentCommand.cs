// ============================================================================
// File:        CurrentCommand.cs
// Project:     AutoVersion Lite
// Version:     0.2.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Implements the 'current' command, displaying the current version
//   as stored in version.txt (or default if none exists).
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Solcogito.Common.Versioning;
using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class CurrentCommand
    {
        /// <summary>
        /// Displays the current project version.
        /// </summary>
        public static int Execute(ArgResult args)
        {
            try
            {
                var version = VersionResolver.ResolveVersion();
                Console.WriteLine(version);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error reading current version: " + ex.Message);
                return 1;
            }
        }
    }
}
