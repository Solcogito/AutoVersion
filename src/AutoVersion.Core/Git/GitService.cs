// ============================================================================
// File:        GitService.cs
// Project:     AutoVersion Lite
// Version:     0.5.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Provides Git utility functions for tag creation, push, and repo safety
//   checks. Used by AutoVersion bump flow for release automation.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Diagnostics;
using System.IO;

namespace Solcogito.AutoVersion.Core.Git
{
    public static class GitService
    {
        private static string RunGit(string args)
        {
            var psi = new ProcessStartInfo("git", args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi)
                ?? throw new InvalidOperationException("Git process failed to start");

            var output = proc.StandardOutput.ReadToEnd().Trim();
            var error = proc.StandardError.ReadToEnd().Trim();

            proc.WaitForExit();

            if (proc.ExitCode != 0 && !string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException($"Git error: {error}");

            return output;
        }

        public static bool IsClean()
        {
            try
            {
                var result = RunGit("status --porcelain");
                return string.IsNullOrWhiteSpace(result);
            }
            catch
            {
                return false;
            }
        }

        public static bool TagExists(string tag)
        {
            try
            {
                var result = RunGit($"tag --list {tag}");
                return !string.IsNullOrWhiteSpace(result);
            }
            catch
            {
                return false;
            }
        }

        public static void CreateTag(string tag, string message)
        {
            if (TagExists(tag))
            {
                Console.WriteLine($"⚠️  Tag '{tag}' already exists, skipping creation.");
                return;
            }

            Console.WriteLine($"🔖  Creating tag {tag}");
            RunGit($"tag -a {tag} -m \"{message}\"");
        }

        public static void PushTag(string tag)
        {
            Console.WriteLine($"🚀  Pushing tag {tag} to origin...");
            RunGit($"push origin {tag}");
        }

        public static bool HasRemote()
        {
            try
            {
                var remotes = RunGit("remote");
                return !string.IsNullOrWhiteSpace(remotes);
            }
            catch
            {
                return false;
            }
        }
    }
}
