// ============================================================================
// File:        VersionManager.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Provides high-level helpers to read, bump, and persist versions on disk.
//   Wraps the VersionModel for CLI and CI usage.
//   Defaults to a local "version.txt" file when no config is provided.
//
//   Example:
//       var current = VersionManager.ReadCurrentVersion();
//       var next = VersionManager.Bump("patch");
//       Console.WriteLine($"Bumped to {next}");
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Provides utilities for managing version persistence and bumping logic.
    /// </summary>
    public static class VersionManager
    {
        private const string DefaultFile = "version.txt";

        // --------------------------------------------------------------------
        // ReadCurrentVersion
        // --------------------------------------------------------------------
        /// <summary>
        /// Reads the current version from a text file (default: version.txt).
        /// Returns 0.0.0 if no file exists.
        /// </summary>
        public static VersionModel ReadCurrentVersion(string? filePath = null)
        {
            filePath ??= DefaultFile;

            if (!File.Exists(filePath))
                return new VersionModel(0, 0, 0);

            var text = File.ReadAllText(filePath).Trim();
            return VersionModel.Parse(text);
        }

        // --------------------------------------------------------------------
        // WriteVersion
        // --------------------------------------------------------------------
        /// <summary>
        /// Writes the given version to the target file.
        /// </summary>
        public static void WriteVersion(VersionModel version, string? filePath = null)
        {
            filePath ??= DefaultFile;
            var content = version.ToString() + Environment.NewLine;
            File.WriteAllText(filePath, content);
        }

        // --------------------------------------------------------------------
        // Bump
        // --------------------------------------------------------------------
        /// <summary>
        /// Reads, bumps, and optionally writes the next version.
        /// </summary>
        /// <param name="type">The version component to bump (major, minor, patch, prerelease).</param>
        /// <param name="pre">Optional prerelease tag.</param>
        /// <param name="dryRun">If true, prints change but does not modify file.</param>
        public static VersionModel Bump(string type, string? pre = null, bool dryRun = false)
        {
            var current = ReadCurrentVersion();
            var next = current.Bump(type, pre);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Current: ");
            Console.ResetColor();
            Console.Write(current.ToString());
            Console.Write(" → ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(next.ToString());
            Console.ResetColor();

            if (!dryRun)
                WriteVersion(next);

            return next;
        }

        // --------------------------------------------------------------------
        // TryReadVersion
        // --------------------------------------------------------------------
        /// <summary>
        /// Attempts to read a version file gracefully.
        /// </summary>
        public static bool TryReadVersion(out VersionModel version, string? filePath = null)
        {
            try
            {
                version = ReadCurrentVersion(filePath);
                return true;
            }
            catch
            {
                version = new VersionModel(0, 0, 0);
                return false;
            }
        }
    }
}
