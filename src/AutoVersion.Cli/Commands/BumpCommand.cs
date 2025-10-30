// ============================================================================
// File:        BumpCommand.cs
// Project:     AutoVersion Lite
// Version:     0.5.2
// Author:      Recursive Architect (Solcogito S.E.N.C.)
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
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Core.Git;
using Solcogito.AutoVersion.Core.Config;
using Solcogito.AutoVersion.Core.Artifacts;
using Solcogito.AutoVersion.Core.Changelog;

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
                Console.WriteLine("Usage: autoversion bump <major|minor|patch|prerelease> [--pre alpha.1] [--dry-run] [--force]");
                return;
            }

            var type = args[1].ToLowerInvariant();
            var pre = args.FirstOrDefault(a => a == "--pre") != null
                ? args.SkipWhile(a => a != "--pre").Skip(1).FirstOrDefault()
                : null;

            var dryRun = args.Contains("--dry-run");
            var force = args.Contains("--force");
            Logger.DryRun = dryRun;

            try
            {
                // ------------------------------------------------------------
                // 1. Load configuration
                // ------------------------------------------------------------
                var config = ConfigLoader.Load();
                Logger.Info("Loaded configuration.");

                // ------------------------------------------------------------
                // 2. Load current version and compute new one
                // ------------------------------------------------------------
                var oldVersion = VersionFile.Load();
                var newVersion = oldVersion.Bump(type, pre);
                Logger.Action($"Version bump: {oldVersion} -> {newVersion}");

                // ------------------------------------------------------------
                // 3. Save version changes (unless dry-run)
                // ------------------------------------------------------------
                if (!dryRun)
                    VersionFile.Save(newVersion);

                // ------------------------------------------------------------
                // 4. Update changelog (safe prepend mode)
                // ------------------------------------------------------------
                try
                {
                    Logger.Info("Updating changelog...");
                    var commits = GitLogReader.ReadCommits(config.Git?.TagPrefix + oldVersion);
                    var parsed = ConventionalCommitParser.Parse(commits);
                    var builder = new ChangelogBuilder(config);
                    var markdown = builder.Build(parsed, newVersion.ToString(), DateTime.UtcNow.ToString("yyyy-MM-dd"));

                    if (!dryRun)
                    {
                        var changelogPath = config.Changelog?.Path ?? "CHANGELOG.md";
                        var existing = File.Exists(changelogPath) ? File.ReadAllText(changelogPath) : string.Empty;
                        var combined = markdown.TrimEnd() + Environment.NewLine + Environment.NewLine + existing.TrimStart();
                        File.WriteAllText(changelogPath, combined);
                    }
                    else
                    {
                        Logger.Info("Dry-run: changelog preview generated only.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn("Changelog skipped or failed: " + ex.Message);
                }

                // ------------------------------------------------------------
                // 5. Process artifacts
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
                // 6. Git Tag Integration
                // ------------------------------------------------------------
                if (config.Git != null && !string.IsNullOrEmpty(config.Git.TagPrefix))
                {
                    var tagName = config.Git.TagPrefix + newVersion;

                    if (!GitService.IsClean() && !config.Git.AllowDirty)
                    {
                        Logger.Warn("Repository is not clean. Use --allow-dirty to override.");
                    }
                    else
                    {
                        GitService.CreateTag(tagName, $"AutoVersion {newVersion} release");
                        if (config.Git.Push)
                            GitService.PushTag(tagName);
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
        }
    }
}
