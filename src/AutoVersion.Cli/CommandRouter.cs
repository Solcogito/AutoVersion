// ============================================================================
// File:        CommandRouter.cs
// Project:     AutoVersion Lite
// Version:     0.9.1
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   CLI router that interprets arguments and dispatches to subcommands.
//   Supports 'current' and 'bump' with optional flags.
//   Adds JSON output mode (--json) and force mode (--force) for CI integration.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Linq;
using System.Diagnostics;
using System.Text.Json;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.Common.Versioning;

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

            // ------------------------------------------------------------
            // Global flag handling (before routing)
            // ------------------------------------------------------------
            bool forceMode = args.Contains("--force");
            if (forceMode)
            {
                Environment.SetEnvironmentVariable("AUTOVERSION_FORCE_DEFAULT", "true");
                Console.WriteLine("[INFO] Force mode enabled (AUTOVERSION_FORCE_DEFAULT=true)");
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
                        {
                            Debug.WriteLine($"Executing 'bump' command with args: {string.Join(" ", args)}");
                            BumpCommand.Execute(args);
                        }
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
            var version = VersionResolver.ResolveVersion();
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
            var currentVersion = VersionResolver.ResolveVersion();

            var result = VersionBumper.Bump(currentVersion, type);
            var payload = new
            {
                command = "bump",
                oldVersion = currentVersion,
                newVersion = result,
                dryRun,
                status = "success"
            };

            Console.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
        }

        // --------------------------------------------------------------------
        // Text-mode help
        // -------------------------------------------------------------------- 

        private static void PrintHelp()
        {
            var versionNum = VersionResolver.ResolveVersion();
            Console.WriteLine($"AutoVersion Lite {versionNum}");
            Console.WriteLine("Usage:");
            Console.WriteLine("  autoversion current [--json] [--force]");
            Console.WriteLine("  autoversion bump <major|minor|patch|prerelease> [--dry-run] [--json] [--force]");
            Console.WriteLine();
            Console.WriteLine("Flags:");
            Console.WriteLine("  --json     Output in JSON format for CI integration");
            Console.WriteLine("  --dry-run  Simulate operation without modifying files");
            Console.WriteLine("  --force    Skip prompts and recreate default config automatically if invalid");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  autoversion bump minor --dry-run --json");
            Console.WriteLine("  autoversion bump patch --force");
        }
    }
}
