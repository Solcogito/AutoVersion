// ============================================================================
// File:        AutoVersionMenu.cs
// Project:     AutoVersion Unity Menu Demo
// Version:     0.6.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Adds Unity Editor menu items under Tools/AutoVersion/ for interacting
//   with the AutoVersion CLI (autoversion.exe) directly from the Editor.
//   Provides bump commands, changelog generation, and version display.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Solcogito.AutoVersion.Unity
{
    public static class AutoVersionMenu
    {
        private static readonly string CliPath =
            Path.Combine(Application.dataPath, "..", "autoversion.exe");

        private static readonly string VersionFile =
            Path.Combine(Application.dataPath, "..", "version.txt");

        // --------------------------------------------------------------------
        // Helper methods
        // --------------------------------------------------------------------

        private static bool EnsureCliExists()
        {
            if (!File.Exists(CliPath))
            {
                EditorUtility.DisplayDialog(
                    "AutoVersion Missing",
                    $"autoversion.exe not found at:\n\n{CliPath}\n\n" +
                    "Please copy it from your Builds/ folder into the Unity project root.",
                    "OK");
                return false;
            }
            return true;
        }

        private static void RunCommand(string args)
        {
            if (!EnsureCliExists())
                return;

            var psi = new ProcessStartInfo
            {
                FileName = CliPath,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            try
            {
                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                UnityEngine.Debug.Log($"<b>[AutoVersion]</b> {args}\n{output}");
                EditorUtility.DisplayDialog("AutoVersion", $"Command executed:\n{args}", "OK");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"AutoVersion CLI failed: {ex.Message}");
                EditorUtility.DisplayDialog("Error", ex.Message, "OK");
            }
        }

        private static void RefreshVersion()
        {
            if (File.Exists(VersionFile))
            {
                string version = File.ReadAllText(VersionFile).Trim();
                UnityEngine.Debug.Log($"<b>AutoVersion</b> current version: {version}");
            }
            else
            {
                UnityEngine.Debug.LogWarning("No version.txt found.");
            }
        }

        // --------------------------------------------------------------------
        // Menu Entries
        // --------------------------------------------------------------------

        [MenuItem("Tools/AutoVersion/Show Current Version", false, 0)]
        public static void ShowVersion()
        {
            RefreshVersion();
            if (File.Exists(VersionFile))
                EditorUtility.DisplayDialog("AutoVersion", $"Current version:\n{File.ReadAllText(VersionFile).Trim()}", "OK");
            else
                EditorUtility.DisplayDialog("AutoVersion", "version.txt not found.", "OK");
        }

        [MenuItem("Tools/AutoVersion/Bump Major", false, 10)]
        public static void BumpMajor() => RunCommand("bump major");

        [MenuItem("Tools/AutoVersion/Bump Minor", false, 11)]
        public static void BumpMinor() => RunCommand("bump minor");

        [MenuItem("Tools/AutoVersion/Bump Patch", false, 12)]
        public static void BumpPatch() => RunCommand("bump patch");

        [MenuItem("Tools/AutoVersion/Bump Prerelease", false, 13)]
        public static void BumpPrerelease() => RunCommand("bump prerelease --pre alpha.1");

        [MenuItem("Tools/AutoVersion/Generate Changelog", false, 20)]
        public static void GenerateChangelog() => RunCommand("changelog");

        [MenuItem("Tools/AutoVersion/Open Changelog", false, 21)]
        public static void OpenChangelog()
        {
            string changelogPath = Path.Combine(Application.dataPath, "AutoVersionDemo", "CHANGELOG.md");
            if (File.Exists(changelogPath))
                Process.Start(new ProcessStartInfo(changelogPath) { UseShellExecute = true });
            else
                EditorUtility.DisplayDialog("AutoVersion", "CHANGELOG.md not found.", "OK");
        }

        [MenuItem("Tools/AutoVersion/Run Dry-Run Preview", false, 30)]
        public static void DryRunPreview() => RunCommand("bump patch --dry-run");

        [MenuItem("Tools/AutoVersion/Help/Documentation", false, 50)]
        public static void OpenDocs() =>
            Application.OpenURL("https://github.com/Solcogito/AutoVersion");

        [MenuItem("Tools/AutoVersion/Help/About AutoVersion", false, 51)]
        public static void About() =>
            EditorUtility.DisplayDialog(
                "AutoVersion Lite",
                "Automated versioning and changelog generation.\n\n" +
                "© 2025 Solcogito S.E.N.C.\nRecursive Architect\n\n" +
                "https://github.com/Solcogito/AutoVersion",
                "OK");
    }
}
#endif
