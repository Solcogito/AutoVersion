// ============================================================================
// File:        BumpCommand.cs
// Project:     AutoVersion Lite
// Version:     0.8.0 (DI-enabled)
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Fully dependency-injected implementation of the "bump" command.
//   All version resolution, file write operations, and logging now go
//   through IVersionEnvironment and ICliLogger, making the command fully
//   unit-testable without filesystem access.
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
        // Handler dictionary — DI-aware command dispatch
        // --------------------------------------------------------------------
        private static readonly Dictionary<string, Func<ArgResult, IVersionEnvironment, ICliLogger, int>> _handlers =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "patch",      (args, env, logger) => ExecuteBump(args, "patch", env, logger) },
                { "minor",      (args, env, logger) => ExecuteBump(args, "minor", env, logger) },
                { "major",      (args, env, logger) => ExecuteBump(args, "major", env, logger) },
                { "prerelease", ExecutePrerelease }
            };

        // --------------------------------------------------------------------
        // Entry point (DI-enabled)
        // --------------------------------------------------------------------
        public static int Execute(ArgResult args, IVersionEnvironment env, ICliLogger logger)
        {
            if (args.CommandName != null &&
                _handlers.TryGetValue(args.CommandName, out var handler))
                return handler(args, env, logger);

            logger.Error($"Unknown bump type: {args.CommandName}");
            return 1;
        }

        // ====================================================================
        // GENERIC BUMP LOGIC (major/minor/patch)
        // ====================================================================
        private static int ExecuteBump(
            ArgResult args,
            string type,
            IVersionEnvironment env,
            ICliLogger logger)
        {
            bool dryRun = args.HasFlag("dry-run");
            logger.DryRun = dryRun;

            try
            {
                // Read version via DI
                var oldVersion = env.GetCurrentVersion();

                // Use your real VersionBumper
                var newVersion = VersionBumper.Bump(oldVersion, type, preRelease: null);

                if (newVersion.Equals(default(VersionModel)))
                {
                    logger.Error("New version resolved to default (0.0.0). Aborting.");
                    return 2;
                }

                var path = env.GetVersionFilePath();
                if (string.IsNullOrWhiteSpace(path))
                {
                    logger.Error("Version file path is empty. Aborting.");
                    return 2;
                }

                // Dry-run: no I/O
                if (dryRun)
                {
                    logger.Info($"[DRY-RUN] Would update version: {oldVersion} -> {newVersion}");
                }
                else
                {
                    logger.Info("Attempting to write version file...");
                    env.WriteVersion(newVersion);
                    logger.Info("Version file written successfully.");
                }

                logger.Action($"Version bump: {oldVersion} -> {newVersion}");

                if (!dryRun)
                    logger.Success($"Version bump complete: {newVersion}");
                else
                    logger.Info("Dry-run completed. No files were modified.");

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("Bump failed: " + ex.Message);
                return 2;
            }
        }

        // ====================================================================
        // PRERELEASE LOGIC (DI-enabled)
        // ====================================================================
        private static int ExecutePrerelease(
            ArgResult args,
            IVersionEnvironment env,
            ICliLogger logger)
        {
            bool dryRun = args.HasFlag("dry-run");
            logger.DryRun = dryRun;

            args.TryGetValue("pre", out var pre);

            // Validate prerelease identifier BEFORE doing anything else
            if (!string.IsNullOrWhiteSpace(pre))
            {
                var semverPreRegex = new Regex(@"^[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*$");

                if (!semverPreRegex.IsMatch(pre))
                {
                    logger.Error($"Invalid prerelease tag: '{pre}'.");
                    logger.Error("Allowed characters: [A-Za-z0-9-], separated by dots.");
                    return 1;
                }
            }

            try
            {
                var oldVersion = env.GetCurrentVersion();
                var newVersion = VersionBumper.Bump(oldVersion, "prerelease", pre);

                if (newVersion.Equals(default(VersionModel)))
                {
                    logger.Error("New version resolved to default (0.0.0). Aborting.");
                    return 2;
                }

                var path = env.GetVersionFilePath();
                if (string.IsNullOrWhiteSpace(path))
                {
                    logger.Error("Version file path is empty. Aborting.");
                    return 2;
                }

                if (dryRun)
                {
                    logger.Info($"[DRY-RUN] Would update version: {oldVersion} -> {newVersion}");
                }
                else
                {
                    logger.Info("Attempting to write version file...");
                    env.WriteVersion(newVersion);
                    logger.Info("Version file written successfully.");
                }

                logger.Action($"Version bump: {oldVersion} -> {newVersion}");

                if (!dryRun)
                    logger.Success($"Version bump complete: {newVersion}");
                else
                    logger.Info("Dry-run completed. No files were modified.");

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("Prerelease bump failed: " + ex.Message);
                return 2;
            }
        }
    }
}
