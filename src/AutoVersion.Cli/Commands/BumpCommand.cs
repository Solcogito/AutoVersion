// ============================================================================
// File:        BumpCommand.cs
// Project:     AutoVersion Lite
// Version:     0.5.3
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Implements the 'bump' subcommand. Supports bumping of major, minor,
//   patch, and prerelease identifiers, changelog generation, artifact
//   renaming, and Git tag integration. Ensures changelog never overwrites.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Core.Git;
using Solcogito.AutoVersion.Core.Config;
using Solcogito.AutoVersion.Core.Artifacts;
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
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run] [--force] [--allow-dirty]");
                return 1;
            }

            var type = args[1].ToLowerInvariant();

            // ------------------------------------------------------------
            // 0. Validate known flags
            // ------------------------------------------------------------
            var validFlags = new HashSet<string>
            {
                "--pre",
                "--dry-run",
                "--force",
                "--allow-dirty"
            };

            var unknownFlags = args
                .Where(a => a.StartsWith("--") && !validFlags.Contains(a))
                .ToList();

            if (unknownFlags.Any())
            {
                Logger.Error("Unknown option(s): " + string.Join(", ", unknownFlags));
                Console.WriteLine("Usage: autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run] [--force] [--allow-dirty]");
                return 2;
            }

            // ------------------------------------------------------------
            // 1. Parse arguments
            // ------------------------------------------------------------
            var pre = args.FirstOrDefault(a => a == "--pre") != null
                ? args.SkipWhile(a => a != "--pre").Skip(1).FirstOrDefault()
                : null;

            var dryRun = args.Contains("--dry-run");
            var force = args.Contains("--force");
            var allowDirty = args.Contains("--allow-dirty");
            Logger.DryRun = dryRun;

            try
            {
                // ------------------------------------------------------------
                // 2. Load configuration
                // ------------------------------------------------------------
                var config = ConfigLoader.Load();
                Logger.Info("Loaded configuration.");

                // ------------------------------------------------------------
                // 3. Load current version and compute new one
                // ------------------------------------------------------------
                var oldVersion = VersionResolver.ResolveVersion();
                var newVersion = Solcogito.AutoVersion.Core.VersionBumper.Bump(oldVersion, args[1]);

                // ------------------------------------------------------------
                // 4. Git Tag Integration
                // ------------------------------------------------------------
                if (config.Git != null && !string.IsNullOrEmpty(config.Git.TagPrefix))
                {
                    var tagName = config.Git.TagPrefix + newVersion;

                    if (!GitService.IsClean() && !config.Git.AllowDirty && !allowDirty)
                    {
                        Logger.Warn("Repository is not clean. Use --allow-dirty to override.");
                        return 3;
                    }
                    else
                    {
                        GitService.CreateTag(tagName, $"AutoVersion {newVersion} release");
                        if (config.Git.Push)
                            GitService.PushTag(tagName);
                    }
                }

                // ------------------------------------------------------------
                // 5. Save version changes (unless dry-run)
                // ------------------------------------------------------------
                if (!dryRun)
                    VersionFile.Write(VersionResolver.ResolveVersionFilePath(), newVersion);

                Logger.Action($"Version bump: {oldVersion} -> {newVersion}");

                // ------------------------------------------------------------
                // 6. Process artifacts
                // ------------------------------------------------------------
                if (config.Artifacts != null && config.Artifacts.Any())
                {
                    Logger.Info("Processing artifacts...");
                    try
                    {
                        var artifactRules = config.Artifacts.Select(a => new ArtifactRule
                        {
                            Path = a.Path,
                            Rename = a.Rename,
                            Overwrite = a.Overwrite
                        });

                        ArtifactManager.ProcessArtifacts(
                            artifactRules,
                            newVersion.ToString(),
                            dryRun: dryRun,
                            force: force
                        );
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Artifact processing failed: " + ex.Message);
                    }
                }

                // ------------------------------------------------------------
                // 7. Final output
                // ------------------------------------------------------------
                if (dryRun)
                    Logger.Info("Dry-run completed. No files were modified.");
                else
                    Logger.Success($"Version bump complete: {newVersion}");
            }
            catch (Exception ex)
            {
                Logger.Error("Bump failed: " + ex.Message);
            }
            return 0;
        }
    }
}
