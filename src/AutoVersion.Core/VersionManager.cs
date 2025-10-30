// ============================================================================
// File:        VersionManager.cs
// Project:     AutoVersion Lite
// Version:     0.9.1
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Provides high-level helpers to read, bump, and persist versions on disk.
//   Wraps the VersionModel for CLI and CI usage.
//
//   Features:
//     • Graceful error recovery and rollback
//     • Dry-run safe (no file modification)
//     • Colorized console output for human mode
//     • Structured VersionResult for JSON output
//     • UTF-8 encoding and EOL preservation
//
//   Example:
//       var result = VersionManager.Bump("patch", dryRun: false);
//       Console.WriteLine($"Bumped {result.OldVersion} → {result.NewVersion}");
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Provides utilities for managing version persistence and bumping logic.
    /// </summary>
    public static class VersionManager
    {
        private const string DefaultFile = "version.txt";

        // --------------------------------------------------------------------
        // GetCurrentVersion
        // --------------------------------------------------------------------
        /// <summary>
        /// Reads the current version as a string, returning "0.0.0" if none exists.
        /// </summary>
        public static string GetCurrentVersion(string? filePath = null)
        {
            try
            {
                var version = ReadCurrentVersion(filePath);
                return version.ToString();
            }
            catch
            {
                return "0.0.0";
            }
        }

        // --------------------------------------------------------------------
        // ReadCurrentVersion
        // --------------------------------------------------------------------
        /// <summary>
        /// Reads and parses the current version from disk.
        /// </summary>
        public static VersionModel ReadCurrentVersion(string? filePath = null)
        {
            filePath ??= DefaultFile;

            if (!File.Exists(filePath))
                return new VersionModel(0, 0, 0);

            var text = File.ReadAllText(filePath, Encoding.UTF8).Trim();
            return VersionModel.Parse(text);
        }

        // --------------------------------------------------------------------
        // WriteVersion
        // --------------------------------------------------------------------
        /// <summary>
        /// Writes the given version to disk, preserving line endings and UTF-8 encoding.
        /// </summary>
        private static void WriteVersion(VersionModel version, string? filePath = null)
        {
            filePath ??= DefaultFile;

            string eol = DetectEol(filePath);
            string content = version + eol;

            // Backup before writing
            BackupFile(filePath);

            File.WriteAllText(filePath, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }

        // --------------------------------------------------------------------
        // Bump
        // --------------------------------------------------------------------
        /// <summary>
        /// Reads, bumps, and optionally writes the next version.
        /// Returns a structured VersionResult object.
        /// </summary>
        public static VersionResult Bump(string type, string? pre = null, bool dryRun = false)
        {
            var filesUpdated = new List<string>();
            var oldVersion = ReadCurrentVersion();
            var nextVersion = oldVersion.Bump(type, pre);

            // Colorized output (human mode)
            if (!Console.IsOutputRedirected)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Current: ");
                Console.ResetColor();
                Console.Write(oldVersion);
                Console.Write(" → ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(nextVersion);
                Console.ResetColor();
            }

            try
            {
                if (!dryRun)
                {
                    WriteVersion(nextVersion);
                    filesUpdated.Add(DefaultFile);
                }

                return new VersionResult(
                    OldVersion: oldVersion.ToString(),
                    NewVersion: nextVersion.ToString(),
                    FilesUpdated: filesUpdated,
                    TagCreated: false
                );
            }
            catch (Exception ex)
            {
                Rollback(DefaultFile);
                throw new InvalidOperationException($"Version bump failed: {ex.Message}", ex);
            }
        }

        // --------------------------------------------------------------------
        // Helpers
        // --------------------------------------------------------------------

        private static void BackupFile(string path)
        {
            if (!File.Exists(path)) return;
            var backup = $"{path}.bak";
            File.Copy(path, backup, overwrite: true);
        }

        private static void Rollback(string path)
        {
            var backup = $"{path}.bak";
            if (File.Exists(backup))
            {
                File.Copy(backup, path, overwrite: true);
                File.Delete(backup);
            }
        }

        private static string DetectEol(string? filePath)
        {
            if (filePath == null || !File.Exists(filePath))
                return Environment.NewLine;

            var content = File.ReadAllText(filePath);
            if (content.Contains("\r\n")) return "\r\n";
            if (content.Contains("\n")) return "\n";
            return Environment.NewLine;
        }

        // --------------------------------------------------------------------
        // TryReadVersion
        // --------------------------------------------------------------------
        /// <summary>
        /// Attempts to read the version gracefully.
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
