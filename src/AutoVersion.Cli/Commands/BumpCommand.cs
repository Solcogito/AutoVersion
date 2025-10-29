// ============================================================================
// File:        BumpCommand.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Implements the 'bump' subcommand. Supports bumping of major, minor,
//   patch, and prerelease identifiers with optional dry-run mode.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Linq;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Cli.Commands
{
    public static class BumpCommand
    {
        /// <summary>
        /// Handles argument parsing and executes version bump logic.
        /// </summary>
        public static void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run]");
                return;
            }

            var type = args[1].ToLowerInvariant();
            var pre = args.FirstOrDefault(a => a == "--pre") != null
                ? args.SkipWhile(a => a != "--pre").Skip(1).FirstOrDefault()
                : null;
            var dryRun = args.Contains("--dry-run");

            Logger.DryRun = dryRun;

            try
            {
                var oldVersion = VersionFile.Load();
                var newVersion = oldVersion.Bump(type, pre);

                Logger.Action($"{oldVersion} → {newVersion}");

                if (!dryRun)
                    VersionFile.Save(newVersion);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
