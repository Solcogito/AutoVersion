// ============================================================================
// File:        CommandRouter.cs
// Project:     AutoVersion Lite
// Version:     0.9.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   CLI router that interprets arguments and dispatches to subcommands.
//   Supports 'current', 'bump', and 'changelog' with optional flags.
//   Adds JSON output mode (--json) for CI integration.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Linq;
using System.Text.Json;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Cli.Commands;

namespace Solcogito.AutoVersion.Cli
{
    internal static class CommandRouter
    {
        public static void Run(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                PrintHelp();
                return;
            }

            var command = args[0].ToLowerInvariant();
            bool jsonMode = args.Contains("--json");

            try
            {
                switch (command)
                {
                    case "current":
                        if (jsonMode)
                            RunCurrentJson();
                        else
                            CurrentCommand.Execute();
                        break;

                    case "bump":
                        if (jsonMode)
                            RunBumpJson(args);
                        else
                            BumpCommand.Execute(args);
                        break;

                    case "--help":
                    case "-h":
                    default:
                        PrintHelp();
                        break;
                }

                Environment.ExitCode = 0;
            }
            catch (Exception ex)
            {
                if (jsonMode)
                {
                    var error = new
                    {
                        status = "error",
                        message = ex.Message,
                        stack = ex.StackTrace
                    };
                    Console.WriteLine(JsonSerializer.Serialize(error, new JsonSerializerOptions { WriteIndented = true }));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"Error: {ex.Message}");
                    Console.ResetColor();
                }

                Environment.ExitCode = 1;
            }
        }

        // --------------------------------------------------------------------
        // JSON handlers
        // --------------------------------------------------------------------

        private static void RunCurrentJson()
        {
            var version = VersionManager.GetCurrentVersion();
            var payload = new
            {
                command = "current",
                version,
                status = "success"
            };
            Console.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
        }

        private static void RunBumpJson(string[] args)
        {
            bool dryRun = args.Contains("--dry-run") || args.Contains("--preview");
            string type = args.Length > 1 ? args[1] : "patch";

            var result = VersionManager.Bump(type, pre: null, dryRun: dryRun);
            var payload = new
            {
                command = "bump",
                oldVersion = result.OldVersion,
                newVersion = result.NewVersion,
                dryRun,
                filesUpdated = result.FilesUpdated?.Count ?? 0,
                tagCreated = result.TagCreated,
                status = "success"
            };

            Console.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
        }

        // --------------------------------------------------------------------
        // Text-mode changelog + help
        // -------------------------------------------------------------------- 

        private static void PrintHelp()
        {	
			var versionNum = VersionFile.Load();
            Console.WriteLine($"AutoVersion Lite {versionNum}");
            Console.WriteLine("Usage:");
            Console.WriteLine("  autoversion current [--json]");
            Console.WriteLine("  autoversion bump <major|minor|patch|prerelease> [--dry-run] [--json]");
            Console.WriteLine("  autoversion changelog [--since-tag <tag>] [--dry-run|--preview] [--json]");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  autoversion bump minor --dry-run --json");
            Console.WriteLine("  autoversion changelog --since-tag v0.8.0");
        }
    }
}
