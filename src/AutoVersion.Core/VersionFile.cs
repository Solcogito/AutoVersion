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

using System.Diagnostics;
using System.IO;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Provides utilities to load and save version information to disk.
    /// </summary>
    public static class VersionFile
    {
        private const string AutoVersionFilePath = "autoversion.json";
        private static readonly string[] versionExt = { ".json", ".txt" };
        private static readonly List<string> defVersionFilePaths = versionExt
            .Select(ext => "version" + ext)
            .ToList();

        private static string VersionPath()
        {
            try
            {
                if (File.Exists(AutoVersionFilePath))
                {
                    // Load it from "VersionFile" key in autoversion.json
                    var jsonText = File.ReadAllText(AutoVersionFilePath).Trim();
                    var jsonDoc = System.Text.Json.JsonDocument.Parse(jsonText);

                    if (jsonDoc.RootElement.TryGetProperty("versionFile", out var versionFileElement))
                    {
                        string path = versionFileElement.GetString() ?? AutoVersionFilePath;
                        return path;
                    }
                    else
                    {
                        return AutoVersionFilePath;
                    }
                }

                foreach (var path in defVersionFilePaths)
                {
                    if (File.Exists(path))
                    {
                        string ver = File.ReadAllText(path).Trim();
                        return path;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Error determining version file path. Using default.");
            }
           return AutoVersionFilePath;
        }

        /// <summary>
        /// Loads the version from the file, or returns a default if missing.
        /// </summary>
        public static VersionModel Load()
        {
            // TODO: abstracted filename loader, so instead of checking for file,
            // we check for module presence and get relevant files
            // (peu importe, whatever file hase to come with it)
            if (!File.Exists(AutoVersionFilePath))
            {
                // Check for both defaults, json first
                foreach (var path in defVersionFilePaths)
                {
                    if (File.Exists(path))
                    {
                        string ver = File.ReadAllText(path).Trim();
                        return VersionModel.Parse(ver);
                    }
                }
            }
            else
            {
                // Load from autoversion.json
                var jsonText = File.ReadAllText(AutoVersionFilePath).Trim();
                var jsonDoc = System.Text.Json.JsonDocument.Parse(jsonText);
                if (jsonDoc.RootElement.TryGetProperty("version", out var versionElement))
                {
                    string ver = versionElement.GetString() ?? "0.1.0";
                    return VersionModel.Parse(ver);
                }
            }

            return new VersionModel(0, 1, 0);
        }

        /// <summary>
        /// Writes the specified version to the version file.
        /// </summary>
        public static void Save(VersionModel version)
        {
            File.WriteAllText(VersionPath(), version.ToString());
        }
    }
}
