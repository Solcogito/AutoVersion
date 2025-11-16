// ============================================================================
// File:        ArtifactManager.cs
// Project:     AutoVersion Lite
// Version:     0.4.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Handles artifact renaming and version stamping according to configuration.
//   Supports dry-run mode, backups, and safe overwrite logic.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;

namespace Solcogito.AutoVersion.Core.ArtifactsPipeline
{
    /// <summary>
    /// Handles file renaming operations for versioned artifacts.
    /// </summary>
    public static class ArtifactManager
    {
        /// <summary>
        /// Processes all artifact rules, safely renaming or previewing changes.
        /// </summary>
        /// <param name="rules">List of artifact rename rules.</param>
        /// <param name="version">Version string to inject into rename pattern.</param>
        /// <param name="dryRun">If true, prints planned operations only.</param>
        /// <param name="force">If true, overwrites existing target files.</param>
        public static void ProcessArtifacts(
            IEnumerable<ArtifactRule> rules,
            string version,
            bool dryRun = false,
            bool force = false)
        {
            foreach (var rule in rules)
            {
                if (string.IsNullOrWhiteSpace(rule.Path) || string.IsNullOrWhiteSpace(rule.Rename))
                    continue;

                var src = rule.Path!;
                if (!File.Exists(src))
                {
                    Console.WriteLine($"[WARN] Missing source file: {src}");
                    continue;
                }

                // Build destination path safely
                var dir = Path.GetDirectoryName(src) ?? ".";
                var fileName = rule.Rename!.Replace("{version}", version, StringComparison.OrdinalIgnoreCase);
                var dest = Path.IsPathRooted(fileName) ? fileName : Path.Combine(dir, fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(dest)!);

                // Handle existing destination
                if (File.Exists(dest))
                {
                    if (!force && !rule.Overwrite)
                    {
                        Console.WriteLine($"[SKIP] File exists and overwrite disabled: {dest}");
                        continue;
                    }

                    if (!dryRun)
                        File.Delete(dest);

                    Console.WriteLine($"[OVERWRITE] {dest}");
                }

                // Perform or simulate the rename
                if (dryRun)
                {
                    Console.WriteLine($"[DRY-RUN] {Path.GetFileName(src)} -> {Path.GetFileName(dest)}");
                }
                else
                {
                    File.Move(src, dest);
                    Console.WriteLine($"[RENAMED] {Path.GetFileName(src)} -> {Path.GetFileName(dest)}");
                }
            }
        }
    }
}
