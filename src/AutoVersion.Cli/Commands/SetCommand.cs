// ============================================================================
// File:        SetCommand.cs
// Project:     AutoVersion Lite
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// -----------------------------------------------------------------------------
// Description:
//   Implements the `autoversion set <version>` command.
//   Validates the semantic version string, performs optional dry-run behavior,
//   resolves the version file location, and writes the new version using the
//   environment abstraction. Errors are logged and surfaced with exit codes.
// ============================================================================

using System;
using Solcogito.Common.Versioning;
using Solcogito.Common.ArgForge;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class SetCommand
    {
        public static int Execute(ArgResult args, IVersionEnvironment env, ICliLogger logger)
        {
            bool dryRun = args.HasFlag("dry-run");

            if (!args.Positionals.TryGetValue("version", out var versionString) ||
                string.IsNullOrWhiteSpace(versionString))
            {
                // TODO (Logging v2): replace all Console.WriteLine/Error.WriteLine with logger only. 
                // Current implementation left for backwards compatibility with initial CLI behavior.

                Console.Error.WriteLine("Error: missing required version argument.");
                logger.Error("Missing required version argument for 'set'.");
                return 1;
            }

            VersionModel newVersion;

            try
            {
                newVersion = VersionModel.Parse(versionString);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: '{versionString}' is not a valid semantic version. {ex.Message}");
                logger.Error($"Invalid version '{versionString}': {ex.Message}");
                return 1;
            }

            try
            {
                var cV = env.GetCurrentVersion();

                var versionFilePath = cV.FilePath;

                if (string.IsNullOrWhiteSpace(versionFilePath))
                {
                    Console.Error.WriteLine("Error: Version file path is empty or could not be resolved.");
                    logger.Error("Version file path is empty or could not be resolved.");
                    return 2;
                }

                if (dryRun)
                {
                    Console.WriteLine($"[DRY-RUN] Would set version to: {newVersion}");
                    logger.Info($"[DRY-RUN] Would set version to: {newVersion}");
                }
                else
                {
                    env.WriteVersion(newVersion);
                    Console.WriteLine($"Version set to: {newVersion}");
                    logger.Success($"Version set to: {newVersion}");
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error setting version: " + ex.Message);
                logger.Error("Error setting version: " + ex.Message);
                return 2;
            }
        }
    }
}
