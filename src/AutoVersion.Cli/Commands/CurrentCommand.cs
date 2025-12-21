// ============================================================================
// File:        CurrentCommand.cs
// Project:     AutoVersion
// ============================================================================

using System;

using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Errors;
using Solcogito.Common.Errors;
using Solcogito.Common.IOKit;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class CurrentCommand
    {
        public static int Execute(IVersionEnvironment env, ICliContext cli)
        {
            string? explicitPath = null;

            if (cli.Args.Options.TryGetValue("path", out var raw) &&
                !string.IsNullOrWhiteSpace(raw))
            {
                try
                {
                    explicitPath = PathUtils.ToAbsolutePath(raw);
                }
                catch (Exception ex)
                {
                    env.Logger.Error(ErrorInfo.From(
                        AutoVersionErrors.InvalidPath,
                        "Invalid --path argument",
                        ("path", raw),
                        ("error", ex.Message)));
                    return 1;
                }
            }

            try
            {
                VersionResolveResult res = env.GetCurrentVersions(explicitPath);

                if (!res.HasFinal)
                {
                    env.Logger.Error(ErrorInfo.From(
                        AutoVersionErrors.NoFinalVersion,
                        "No version could be resolved.",
                        ("path", explicitPath ?? "<defaults>")));
                    return 2;
                }

                string output = res.Final!.Value.ToString();

                // USER-VISIBLE CLI OUTPUT (captured by tests)
                cli.Output.WriteLine(output);

                return 0;
            }
            catch (Exception ex)
            {
                env.Logger.Error(ErrorInfo.Unexpected(
                    AutoVersionErrors.CliFailure,
                    ex));
                return 2;
            }
        }
    }
}
