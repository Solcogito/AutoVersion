// ============================================================================
// File:        SetCommand.cs
// Project:     AutoVersion
// Author:      Solcogito S.E.N.C.
// ============================================================================

using System;

using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Errors;
using Solcogito.Common.Errors;
using Solcogito.Common.IOKit;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class SetCommand
    {
        public static int Execute(IVersionEnvironment env, ICliContext cli)
        {
            bool dryRun = cli.Args.Flags.GetValueOrDefault("dry-run");

            if (cli.Args.Positionals.Count == 0 ||
                string.IsNullOrWhiteSpace(cli.Args.Positionals[0]))
            {
                env.Logger.Error(ErrorInfo.From(
                    AutoVersionErrors.MissingVersion,
                    "Missing required version argument"));
                return 1;
            }

            string rawVersion = cli.Args.Positionals[0];

            VersionModel newVersion;
            try
            {
                newVersion = VersionModel.Parse(rawVersion);
            }
            catch (Exception ex)
            {
                env.Logger.Error(ErrorInfo.From(
                    AutoVersionErrors.InvalidVersion,
                    "Invalid version string",
                    ("value", rawVersion),
                    ("error", ex.Message)));
                return 1;
            }

            string? explicitPath = null;

            if (cli.Args.Options.TryGetValue("path", out var rawPath) &&
                !string.IsNullOrWhiteSpace(rawPath))
            {
                try
                {
                    explicitPath = PathUtils.ToAbsolutePath(rawPath);
                }
                catch (Exception ex)
                {
                    env.Logger.Error(ErrorInfo.From(
                        AutoVersionErrors.InvalidPath,
                        "Invalid --path argument",
                        ("path", rawPath),
                        ("error", ex.Message)));
                    return 1;
                }
            }

            try
            {
                if (dryRun)
                {
                    env.Logger.Info("[DRY-RUN] Would set version to: " + newVersion);
                    return 0;
                }

                if (!string.IsNullOrWhiteSpace(explicitPath))
                {
                    env.WriteVersion(newVersion, explicitPath);
                    env.Logger.Info("Version set to: " + newVersion);
                    return 0;
                }

                VersionResolveResult resolved = env.GetCurrentVersions(null);

                if (!resolved.HasFinal)
                {
                    env.Logger.Error(ErrorInfo.From(
                        AutoVersionErrors.MissingPath,
                        "No default version files found. Use --path to specify a write target."));
                    return 1;
                }

                // IMPORTANT:
                // Writing applies to ALL discovered default sources.
                foreach (string source in resolved.CheckedSources)
                {
                    env.WriteVersion(newVersion, source);
                }

                env.Logger.Info("Version set to: " + newVersion);
                return 0;
            }
            catch (Exception ex)
            {
                env.Logger.Error(
                    ErrorInfo.Unexpected(AutoVersionErrors.WriteFailed, ex));
                return 2;
            }
        }
    }
}
