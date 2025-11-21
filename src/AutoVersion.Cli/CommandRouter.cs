// ============================================================================
// File: CommandRouter.cs
// Project:     AutoVersion Lite
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ------------------------------------------------------------------------------
// Description:
//   Routes parsed CLI arguments to the appropriate command handlers.
//   Handles root-level commands (current, set) and delegates bump-related
//   subcommands to BumpCommand. Provides unified error handling and help output.
// ============================================================================


using System;
using System.Collections.Generic;
using Solcogito.Common.ArgForge;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Cli
{
    internal static class CommandRouter
    {
        private static readonly Dictionary<string, Func<ArgResult, IVersionEnvironment, ICliLogger, int>> _rootHandlers =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "current", (args, env, logger) => CurrentCommand.Execute(args, env, logger) },
                { "set",     (args, env, logger) => SetCommand.Execute(args, env, logger)     },

                // "bump" handled via parent path logic below
            };

        /// <summary>
        /// Entrypoint for routing after arguments have been parsed.
        /// </summary>
        public static int Run(ArgResult args, ArgSchema schema, IVersionEnvironment env, ICliLogger logger)
        {
            // No command? → Show root help
            if (args.CommandName == null || args.CommandPath.Count <= 1)
            {
                Console.WriteLine(schema.GetHelp("autoversion"));
                return 1;
            }

            var command = args.CommandName;         // e.g. "patch" or "current"
            var parent  = args.CommandPath[^2];     // e.g. "bump" when inside bump

            // 1. If parent = "bump" → delegate to BumpCommand
            if (parent.Equals("bump", StringComparison.OrdinalIgnoreCase))
            {
                return BumpCommand.Execute(args, env, logger);
            }

            // 2. Root-level commands (current, set)
            if (_rootHandlers.TryGetValue(command, out var handler))
                return handler(args, env, logger);

            // 3. autoversion bump (no subcommand)
            if (command.Equals("bump", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(schema.GetHelp("autoversion", "bump"));
                return 1;
            }

            // 4. Unknown command → help
            Console.WriteLine(schema.GetHelp("autoversion"));
            Console.WriteLine();
            Console.WriteLine($"Unknown command: {command}");
            return 1;
        }
    }
}
