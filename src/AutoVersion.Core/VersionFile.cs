// ============================================================================
// File:        VersionFile.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Handles persistent storage of the current project version. Provides
//   simple file-based read/write operations compatible with version.txt.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.IO;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Provides utilities to load and save version information to disk.
    /// </summary>
    public static class VersionFile
    {
        private const string VersionPath = "version.txt";

        /// <summary>
        /// Loads the version from the file, or returns a default if missing.
        /// </summary>
        public static VersionModel Load()
        {
            if (!File.Exists(VersionPath))
                return new VersionModel(0, 1, 0);

            var text = File.ReadAllText(VersionPath).Trim();
            return VersionModel.Parse(text);
        }

        /// <summary>
        /// Writes the specified version to the version file.
        /// </summary>
        public static void Save(VersionModel version)
        {
            File.WriteAllText(VersionPath, version.ToString());
        }
    }
}
