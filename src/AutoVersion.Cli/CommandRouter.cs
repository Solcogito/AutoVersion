// ============================================================================
// File:        CommandRouter.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Minimal CLI router that interprets arguments and dispatches commands.
//   Supports subcommands: 'current' and 'bump' with optional flags.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Cli
{
    internal static class CommandRouter
    {
        /// <summary>
        /// Parses CLI arguments and executes the corresponding command.
        /// </summary>
        public static void Run(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                PrintHelp();
                return;
            }

            var command = args[0].ToLowerInvariant();
            switch (command)
            {
                case "current":
                    Commands.CurrentCommand.Execute();
                    break;

                case "bump":
                    Commands.BumpCommand.Execute(args);
                    break;

                case "--help":
                case "-h":
                default:
                    PrintHelp();
                    break;
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("AutoVersion Lite 0.1.0");
            Console.WriteLine("Usage:");
            Console.WriteLine("  autoversion current");
            Console.WriteLine("  autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run]");
        }
    }
}
