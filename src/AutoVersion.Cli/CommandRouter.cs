// ============================================================================
// File:        CommandRouter.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Defines and executes all CLI commands for AutoVersion Lite.
//   Commands:
//     - current     → Display current version
//     - bump        → Increment major/minor/patch/prerelease
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Cli
{
    /// <summary>
    /// Routes command-line invocations to core logic.
    /// </summary>
    public static class CommandRouter
    {
        public static void Run(string[] args)
        {
            var root = new RootCommand("AutoVersion Lite — Semantic Versioning & Changelog Automation");

            // ----------------------------------------------------------------
            // current
            // ----------------------------------------------------------------
            var currentCmd = new Command("current", "Displays the current version stored locally");
            currentCmd.SetHandler(() =>
            {
                var v = VersionManager.ReadCurrentVersion();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(v.ToString());
                Console.ResetColor();
            });
            root.AddCommand(currentCmd);

            // ----------------------------------------------------------------
			// bump
			// ----------------------------------------------------------------
			var bumpCmd = new Command("bump", "Increments version: major | minor | patch | prerelease");

			var typeArg = new Argument<string>("type", description: "major | minor | patch | prerelease");
			var preOpt = new Option<string>("--pre", "Optional prerelease tag (e.g. alpha.1)");
			var dryOpt = new Option<bool>("--dry-run", "Preview changes without writing to file");

			bumpCmd.AddArgument(typeArg);
			bumpCmd.AddOption(preOpt);
			bumpCmd.AddOption(dryOpt);

			bumpCmd.SetHandler(
				(Action<string, string, bool>)((type, pre, dryRun) =>
				{
					try
					{
						VersionManager.Bump(type, pre, dryRun);
					}
					catch (Exception ex)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Error.WriteLine($"Error: {ex.Message}");
						Console.ResetColor();
					}
				}),
				typeArg,
				preOpt,
				dryOpt
			);

			root.AddCommand(bumpCmd);
            // ----------------------------------------------------------------
            // help
            // ----------------------------------------------------------------
            var helpCmd = new Command("help", "Shows CLI usage information");
            helpCmd.SetHandler(() =>
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  autoversion current");
                Console.WriteLine("  autoversion bump [major|minor|patch|prerelease] [--pre alpha.1] [--dry-run]");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("  autoversion bump patch");
                Console.WriteLine("  autoversion bump prerelease --pre beta.2");
            });
            root.AddCommand(helpCmd);

            // ----------------------------------------------------------------
            // Execute
            // ----------------------------------------------------------------
            root.Invoke(args);
        }
    }
}
