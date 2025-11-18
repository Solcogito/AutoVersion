// ============================================================================
// File:        CommandRouter.cs
// Project:     AutoVersion Lite
// Version:     0.7.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Unified command router for hierarchical ArgForge commands. This class
//   decides which high-level command to execute (current, bump, set, etc.)
//   based solely on ArgResult.CommandName and ArgResult.CommandPath.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using Solcogito.Common.ArgForge;
using Solcogito.AutoVersion.Cli.Commands;

namespace Solcogito.AutoVersion.Cli
{
    internal static class CommandRouter
    {
        // --------------------------------------------------------------------
        // High-level command handlers
        // --------------------------------------------------------------------
        private static readonly Dictionary<string, Func<ArgResult, int>> _rootHandlers =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "current",    CurrentCommand.Execute },
                { "set",        SetCommand.Execute     },

                // IMPORTANT:
                // "bump" is NOT executed here!
                // BumpCommand executes SUBCOMMANDS like patch/minor/etc.
            };

        /// <summary>
        /// Entrypoint for routing after arguments have been parsed.
        /// </summary>
        public static int Run(ArgResult args, ArgSchema schema)
        {
            // No command? → Show root help
            if (args.CommandName == null || args.CommandPath.Count <= 1)
            {
                Console.WriteLine(schema.GetHelp("autoversion"));
                return 1;
            }

            var command = args.CommandName;                       // e.g. "patch" or "current"
            var parent = args.CommandPath[^2];                  // e.g. "bump" when inside bump

            // ------------------------------------------------------------
            // 1. If parent command is "bump", then route to BumpCommand.
            //    (patch/minor/major/prerelease are handled INSIDE)
            // ------------------------------------------------------------
            if (parent.Equals("bump", StringComparison.OrdinalIgnoreCase))
            {
                return BumpCommand.Execute(args);
            }

            // ------------------------------------------------------------
            // 2. If the command itself is a root-level command
            // ------------------------------------------------------------
            if (_rootHandlers.TryGetValue(command, out var handler))
                return handler(args);

            // ------------------------------------------------------------
            // 3. If they typed: autoversion bump   (no subcommand)
            // ------------------------------------------------------------
            if (command.Equals("bump", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(schema.GetHelp("autoversion", "bump"));
                return 1;
            }

            // ------------------------------------------------------------
            // 4. Unknown command → show help
            // ------------------------------------------------------------
            Console.WriteLine(schema.GetHelp("autoversion"));
            Console.WriteLine();
            Console.WriteLine($"Unknown command: {command}");
            return 1;
        }
    }
}
