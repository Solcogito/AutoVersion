// ============================================================================
// File:        ArtifactRule.cs
// Project:     AutoVersion Lite
// Version:     0.4.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Represents a single artifact rename rule as defined in the AutoVersion
//   configuration file. Each rule maps an existing file path to a new file
//   name pattern that includes the {version} placeholder.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;

namespace Solcogito.AutoVersion.Core.ArtifactsPipeline
{
    /// <summary>
    /// Defines a single artifact rename rule for post-build version stamping.
    /// </summary>
    public class ArtifactRule
    {
        /// <summary>
        /// Absolute or relative path to the artifact file.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// New file name pattern. Supports {version} placeholder.
        /// Example: "Product_{version}.zip"
        /// </summary>
        public string Rename { get; set; } = string.Empty;

        /// <summary>
        /// If true, overwrites existing files when renaming.
        /// </summary>
        public bool Overwrite { get; set; } = false;

        /// <summary>
        /// Resolves the full destination file path for a given version string.
        /// </summary>
        /// <param name="version">The version value to inject into the pattern.</param>
        /// <returns>Resolved absolute or relative target path.</returns>
        public string GetTargetName(string version)
        {
            if (string.IsNullOrWhiteSpace(Rename))
                return Path;

            var dir = System.IO.Path.GetDirectoryName(Path) ?? ".";
            var fileName = Rename.Replace("{version}", version, StringComparison.OrdinalIgnoreCase);
            return System.IO.Path.IsPathRooted(fileName)
                ? fileName
                : System.IO.Path.Combine(dir, fileName);
        }

        /// <summary>
        /// Ensures the destination directory exists.
        /// </summary>
        public void EnsureDirectory()
        {
            var dest = GetTargetName("preview");
            var directory = System.IO.Path.GetDirectoryName(dest);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}
