// ============================================================================
// File:        SetCommand.cs
// Project:     AutoVersion Lite
// Version:     0.2.1
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Implements the 'set <version>' command. Writes an explicit version to
//   the version file and reports success, supporting dry-run mode.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Solcogito.Common.Versioning;
using Solcogito.AutoVersion.Core.Config;
using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class SetCommand
    {
        /// <summary>
        /// Sets the version directly: autoversion set <version>.
        /// </summary>
        public static int Execute(ArgResult args)
        {
            bool dryRun = args.HasFlag("dry-run");

            // Get <version> positional
            if (!args.Positionals.TryGetValue("version", out var versionString) ||
                string.IsNullOrWhiteSpace(versionString))
            {
                Console.Error.WriteLine("Error: missing required version argument.");
                return 1;
            }

            VersionModel newVersion;

            // We must use Parse(), because VersionModel has NO TryParse()
            try
            {
                newVersion = VersionModel.Parse(versionString);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: '{versionString}' is not a valid semantic version. {ex.Message}");
                return 1;
            }

            try
            {
                var config = ConfigLoader.Load();
                var versionFilePath = VersionResolver.ResolveVersionFilePath();

                if (string.IsNullOrWhiteSpace(versionFilePath))
                {
                    Console.Error.WriteLine("Error: Version file path is empty or could not be resolved.");
                    return 2;
                }

                if (dryRun)
                {
                    Console.WriteLine($"[DRY-RUN] Would set version to: {newVersion}");
                }
                else
                {
                    VersionFile.Write(versionFilePath, newVersion);
                    Console.WriteLine($"Version set to: {newVersion}");
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error setting version: " + ex.Message);
                return 2;
            }
        }
    }
}
