// ============================================================================
// File:        BumpCommand.cs
// Project:     AutoVersion Lite
// Version:     0.7.1
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Implements the 'bump' command using hierarchical ArgForge. Each bump type
//   (patch, minor, major, prerelease) is exposed as a subcommand and routed
//   through a clean handler dictionary for maximum extensibility.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Solcogito.AutoVersion.Core;
using Solcogito.Common.Versioning;
using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Cli.Commands
{
    public static class BumpCommand
    {
        // --------------------------------------------------------------------
        // Handler dictionary — clean command dispatch
        // --------------------------------------------------------------------
        private static readonly Dictionary<string, Func<ArgResult, int>> _handlers =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "patch",      args => ExecuteBump(args, "patch") },
                { "minor",      args => ExecuteBump(args, "minor") },
                { "major",      args => ExecuteBump(args, "major") },
                { "prerelease", ExecutePrerelease }
            };

        /// <summary>
        /// Entry point for the 'bump' command.
        /// The hierarchical ArgForge parser puts the final subcommand name
        /// into args.CommandName, so routing is now trivial.
        /// </summary>
        public static int Execute(ArgResult args)
        {
            if (_handlers.TryGetValue(args.CommandName!, out var handler))
                return handler(args);

            Logger.Error("Unknown bump type: " + args.CommandName);
            return 1;
        }

        // ====================================================================
        // GENERIC BUMP LOGIC (major/minor/patch)
        // ====================================================================
        private static int ExecuteBump(ArgResult args, string type)
        {
            bool dryRun = args.HasFlag("dry-run");
            Logger.DryRun = dryRun;

            try
            {
                var oldVersion = VersionResolver.ResolveVersion();

                var newVersion = VersionBumper.Bump(oldVersion, type, preRelease: null);

                if (newVersion.Equals(default(VersionModel)))
                {
                    Logger.Error("New version resolved to default (0.0.0). Aborting.");
                    return 2;
                }

                var versionFilePath = VersionResolver.ResolveVersionFilePath();
                if (string.IsNullOrWhiteSpace(versionFilePath))
                {
                    Logger.Error("Version file path is empty. Aborting.");
                    return 2;
                }

                if (dryRun)
                {
                    Logger.Info($"[DRY-RUN] Would update version: {oldVersion} -> {newVersion}");
                }
                else
                {
                    Logger.Info("Attempting to write version file...");
                    VersionFile.Write(versionFilePath, newVersion);
                    Logger.Info("Version file written successfully.");
                }

                Logger.Action($"Version bump: {oldVersion} -> {newVersion}");

                if (!dryRun)
                    Logger.Success($"Version bump complete: {newVersion}");
                else
                    Logger.Info("Dry-run completed. No files were modified.");

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("Bump failed: " + ex.Message);
                return 2;
            }
        }

        // ====================================================================
        // PRERELEASE LOGIC (with prerelease validation)
        // ====================================================================
        private static int ExecutePrerelease(ArgResult args)
        {
            bool dryRun = args.HasFlag("dry-run");
            Logger.DryRun = dryRun;

            args.TryGetValue("pre", out var pre);

            // ----------------------------------------------------------------
            // Validate prerelease identifier BEFORE doing anything else
            // SemVer rule: only [0-9A-Za-z-] and dot separators
            // ----------------------------------------------------------------
            if (!string.IsNullOrWhiteSpace(pre))
            {
                // Regex per SemVer 2.0.0 prerelease component rules
                var semverPreRegex = new Regex(@"^[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*$");

                if (!semverPreRegex.IsMatch(pre))
                {
                    Logger.Error($"Invalid prerelease tag: '{pre}'.");
                    Logger.Error("Allowed characters: [A-Za-z0-9-], separated by dots.");
                    return 1;
                }
            }

            try
            {
                var oldVersion = VersionResolver.ResolveVersion();

                var newVersion = VersionBumper.Bump(oldVersion, "prerelease", pre);

                if (newVersion.Equals(default(VersionModel)))
                {
                    Logger.Error("New version resolved to default (0.0.0). Aborting.");
                    return 2;
                }

                var versionFilePath = VersionResolver.ResolveVersionFilePath();
                if (string.IsNullOrWhiteSpace(versionFilePath))
                {
                    Logger.Error("Version file path is empty. Aborting.");
                    return 2;
                }

                if (dryRun)
                {
                    Logger.Info($"[DRY-RUN] Would update version: {oldVersion} -> {newVersion}");
                }
                else
                {
                    Logger.Info("Attempting to write version file...");
                    VersionFile.Write(versionFilePath, newVersion);
                    Logger.Info("Version file written successfully.");
                }

                Logger.Action($"Version bump: {oldVersion} -> {newVersion}");

                if (!dryRun)
                    Logger.Success($"Version bump complete: {newVersion}");
                else
                    Logger.Info("Dry-run completed. No files were modified.");

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("Prerelease bump failed: " + ex.Message);
                return 2;
            }
        }
    }
}
