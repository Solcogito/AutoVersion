// ============================================================================
// File:        BumpCommand.cs
// Project:     AutoVersion
// Author:      Solcogito S.E.N.C.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Errors;
using Solcogito.Common.Errors;
using Solcogito.Common.IOKit;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class BumpCommand
    {
        private static readonly Dictionary<string, Func<IVersionEnvironment, ICliContext, int>>
            Handlers = new(StringComparer.OrdinalIgnoreCase)
            {
                { "patch",      (e, c) => ExecuteBump("patch", null, e, c) },
                { "minor",      (e, c) => ExecuteBump("minor", null, e, c) },
                { "major",      (e, c) => ExecuteBump("major", null, e, c) },
                { "prerelease", ExecutePrerelease }
            };

        public static int Execute(IVersionEnvironment env, ICliContext cli)
        {
            var cmd = cli.Args.Command;

            if (cmd == null)
            {
                env.Logger.Error(ErrorInfo.From(
                    AutoVersionErrors.MissingCommand,
                    "No bump type provided"));
                return 1;
            }

            if (!Handlers.TryGetValue(cmd.Name, out var handler))
            {
                env.Logger.Error(ErrorInfo.From(
                    AutoVersionErrors.UnknownBumpType,
                    "Unknown bump type",
                    ("type", cmd.Name)));
                return 1;
            }

            return handler(env, cli);
        }

        private static int ExecuteBump(
            string type,
            string? prerelease,
            IVersionEnvironment env,
            ICliContext cli)
        {
            bool dryRun = cli.Args.Flags.GetValueOrDefault("dry-run");

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
                VersionResolveResult resolved =
                    env.GetCurrentVersions(explicitPath);

                if (!resolved.HasFinal)
                {
                    env.Logger.Error(ErrorInfo.From(
                        AutoVersionErrors.MissingVersion,
                        "No version found to bump",
                        ("path", explicitPath ?? "<defaults>")));
                    return 1;
                }

                VersionModel? current = resolved.Final!;
                VersionModel next = VersionBumper.Bump(current, type, prerelease);

                if (dryRun)
                {
                    env.Logger.Info(
                        "[DRY-RUN] Would bump " + type + ": " +
                        current + " -> " + next);
                    return 0;
                }

                foreach (string source in resolved.CheckedSources)
                {
                    env.WriteVersion(next, source);
                }

                env.Logger.Info(
                    "Version bump (" + type + "): " +
                    current + " -> " + next);

                return 0;
            }
            catch (Exception ex)
            {
                env.Logger.Error(
                    ErrorInfo.Unexpected(AutoVersionErrors.CliFailure, ex));
                return 2;
            }
        }

        private static int ExecutePrerelease(
            IVersionEnvironment env,
            ICliContext cli)
        {
            cli.Args.Options.TryGetValue("pre", out var pre);

            if (!string.IsNullOrWhiteSpace(pre))
            {
                var rx = new Regex(@"^[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*$");
                if (!rx.IsMatch(pre))
                {
                    env.Logger.Error(ErrorInfo.From(
                        AutoVersionErrors.InvalidPrerelease,
                        "Invalid prerelease tag",
                        ("value", pre)));
                    return 1;
                }
            }

            return ExecuteBump("prerelease", pre, env, cli);
        }
    }
}
