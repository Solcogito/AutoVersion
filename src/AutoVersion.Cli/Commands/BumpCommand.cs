// ============================================================================
// File:        BumpCommand.cs
// Project:     AutoVersion Lite
// Version:     0.6.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Implements the 'bump' subcommand. Supports bumping of major, minor,
//   patch, and prerelease identifiers. This Lite version intentionally
//   avoids Git integration, artifact processing, and CI responsibilities:
//   it only computes and writes the new semantic version.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Core.Config;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Cli.Commands
{
    public static class BumpCommand
    {
        /// <summary>
        /// Handles argument parsing and executes version bump logic.
        /// </summary>
        public static int Execute(string[] args)
        {
            // Expect at least: autoversion bump <type>
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run] [--force]");
                return 1;
            }

            var type = args[1].ToLowerInvariant();

            // ------------------------------------------------------------
            // 0. Validate known flags (Lite: only pre/dry-run/force)
            // ------------------------------------------------------------
            var validFlags = new HashSet<string>
            {
                "--pre",
                "--dry-run",
                "--force"
            };

            var unknownFlags = args
                .Where(a => a.StartsWith("--") && !validFlags.Contains(a))
                .ToList();

            if (unknownFlags.Any())
            {
                Logger.Error("Unknown option(s): " + string.Join(", ", unknownFlags));
                Console.WriteLine("Usage: autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run] [--force]");
                return 1;
            }

            // ------------------------------------------------------------
            // 1. Parse arguments
            // ------------------------------------------------------------
            var pre = args.FirstOrDefault(a => a == "--pre") != null
                ? args.SkipWhile(a => a != "--pre").Skip(1).FirstOrDefault()
                : null;

            var dryRun = args.Contains("--dry-run");
            var force = args.Contains("--force");

            Logger.DryRun = dryRun;

            try
            {
                // --------------------------------------------------------
                // 2. Load configuration (Lite: just to ensure file is valid)
                // --------------------------------------------------------
                var config = ConfigLoader.Load();
                Logger.Info("Loaded configuration.");

                // --------------------------------------------------------
                // 3. Load current version and compute new one
                // --------------------------------------------------------
                var oldVersion = VersionResolver.ResolveVersion();
                var newVersion = VersionBumper.Bump(oldVersion, type, pre);

                if (newVersion.Equals(default(VersionModel)))
                {
                    Logger.Error("New version resolved to default (0.0.0). Aborting.");
                    return 2;
                }

                // --------------------------------------------------------
                // 4. Resolve version file path
                // --------------------------------------------------------
                var versionFilePath = VersionResolver.ResolveVersionFilePath();
                Logger.Info($"Resolved version file path: '{versionFilePath}'");

                if (string.IsNullOrWhiteSpace(versionFilePath))
                {
                    Logger.Error("Version file path is empty. Aborting.");
                    return 2;
                }

                // --------------------------------------------------------
                // 5. Save version changes (unless dry-run)
                // --------------------------------------------------------
                if (dryRun)
                {
                    Logger.Info($"[DRY-RUN] Would update version: {oldVersion} -> {newVersion}");
                }
                else
                {
                    try
                    {
                        Logger.Info("Attempting to write version file...");
                        VersionFile.Write(versionFilePath, newVersion);
                        Logger.Info("Version file written successfully.");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error inside VersionFile.Write: {ex}");
                        return 2;
                    }
                }

                Logger.Action($"Version bump: {oldVersion} -> {newVersion}");

                // --------------------------------------------------------
                // 6. Final output
                // --------------------------------------------------------
                if (dryRun)
                    Logger.Info("Dry-run completed. No files were modified.");
                else
                    Logger.Success($"Version bump complete: {newVersion}");
            }
            catch (Exception ex)
            {
                Logger.Error("Bump failed: " + ex.Message);
                return 2;
            }

            return 0;
        }
    }
}
