// ============================================================================
// File:        CurrentCommand.cs
// Project:     AutoVersion Lite
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// -----------------------------------------------------------------------------
// Description:
//   Implements the `autoversion current` command.
//   Retrieves the current project version using the version environment,
//   prints it to stdout, and handles errors gracefully.
// ============================================================================

using System;
using Solcogito.Common.ArgForge;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class CurrentCommand
    {
        /// <summary>
        /// Displays the current project version.
        /// </summary>
        public static int Execute(ArgResult args, IVersionEnvironment env, ICliLogger logger)
        {
            try
            {
                var version = env.GetCurrentVersion().Version;
                Console.WriteLine(version);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error reading current version: " + ex.Message);
                logger.Error("Error reading current version: " + ex.Message);
                return 1;
            }
        }
    }
}
