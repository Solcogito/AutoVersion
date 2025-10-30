// ============================================================================
// File:        AutoVersionIntegrationTests.cs
// Project:     AutoVersion Unity Menu
// Version:     0.6.1
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Verifies integration between the Unity Editor and the AutoVersion CLI.
//   Ensures CLI executes correctly, version.txt is accessible, and that
//   commands like "bump patch --dry-run" return expected output.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================
#if UNITY_EDITOR
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class AutoVersionIntegrationTests
{
    private string versionFile;
    private string cliPath;

    [SetUp]
    public void Setup()
    {
        versionFile = Path.Combine(Application.dataPath, "../version.txt");
        cliPath = Path.Combine(Application.dataPath, "../Builds/autoversion.exe");

        Assert.True(File.Exists(cliPath),
            $"CLI executable not found at: {cliPath}\nPlease run build.ps1 first.");
        Assert.True(File.Exists(versionFile),
            $"version.txt not found at: {versionFile}\nEnsure AutoVersion is initialized.");
    }

    [Test]
    public void RunCli_DryRunBump_WorksWithoutErrors()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = cliPath,
            Arguments = "bump patch --dry-run",
            WorkingDirectory = Path.GetDirectoryName(cliPath),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        Assert.NotNull(process, "Failed to start AutoVersion CLI process.");

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit(8000);

        UnityEngine.Debug.Log("[AutoVersion CLI Output]\n" + output);
        if (!string.IsNullOrEmpty(error))
            UnityEngine.Debug.LogWarning("[AutoVersion CLI Error]\n" + error);

        Assert.True(process.ExitCode == 0, $"CLI exited with code {process.ExitCode}");
        Assert.True(output.Contains("Version bump"), "CLI output missing expected 'Version bump' message.");
    }

    [Test]
    public void VersionFile_IsReadable_AndContainsValidSemVer()
    {
        var version = File.ReadAllText(versionFile).Trim();
        UnityEngine.Debug.Log($"[AutoVersion] Current version in file: {version}");
        Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(version, @"^\d+\.\d+\.\d+"),
            "Version format should follow SemVer (e.g. 1.2.3)");
    }
}
#endif
