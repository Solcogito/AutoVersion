// ============================================================================
// File:        CommandRouter.cs
// Project:     AutoVersion Lite
// Version:     0.3.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   CLI router that interprets arguments and dispatches to subcommands.
//   Supports: 'current', 'bump', and 'changelog' with optional flags.
//   Provides contextual help and graceful error output.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Cli.Commands;

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

            try
            {
                switch (command)
                {
                    case "current":
                        CurrentCommand.Execute();
                        break;

                    case "bump":
                        BumpCommand.Execute(args);
                        break;

                    case "changelog":
                        RunChangelog(args);
                        break;

                    case "--help":
                    case "-h":
                    default:
                        PrintHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                Environment.ExitCode = 1;
            }
        }

        // --------------------------------------------------------------------
        // Command helpers
        // --------------------------------------------------------------------

        private static void RunChangelog(string[] args)
        {
            string? sinceTag = null;
            bool dryRun = false;

            foreach (var arg in args)
            {
                if (arg.StartsWith("--since-tag"))
                {
                    var parts = arg.Split('=');
                    if (parts.Length == 2)
                        sinceTag = parts[1];
                }
                else if (arg == "--dry-run" || arg == "--preview")
                {
                    dryRun = true;
                }
            }

            ChangelogCommand.Run(sinceTag, dryRun);
        }

        private static void PrintHelp()
        {
            Console.WriteLine("AutoVersion Lite 0.3.0");
            Console.WriteLine("Usage:");
            Console.WriteLine("  autoversion current");
            Console.WriteLine("  autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run]");
            Console.WriteLine("  autoversion changelog [--since-tag <tag>] [--dry-run|--preview]");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  autoversion changelog --since-tag v0.2.0");
            Console.WriteLine("  autoversion changelog --dry-run");
        }
    }
}
