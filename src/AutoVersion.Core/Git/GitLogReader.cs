// ============================================================================
// File:        GitLogReader.cs
// Project:     AutoVersion Lite
// Version:     0.3.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Reads commit history from Git and returns structured commit records.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solcogito.AutoVersion.Core.Git
{
    public class GitCommit
    {
        public string Hash { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public static class GitLogReader
    {
        public static IEnumerable<GitCommit> ReadCommits(string? sinceTag = null)
        {
            var args = $"log --pretty=format:\"%H%n%s%n%b%n==END==\" --no-merges";
            if (!string.IsNullOrEmpty(sinceTag))
                args += $" {sinceTag}..HEAD";

            var psi = new ProcessStartInfo("git", args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start git process");
            using var reader = proc.StandardOutput;

            var output = reader.ReadToEnd();
            proc.WaitForExit();

            return ParseOutput(output);
        }

        private static IEnumerable<GitCommit> ParseOutput(string output)
        {
            var commits = new List<GitCommit>();
            var sections = output.Split("==END==", StringSplitOptions.RemoveEmptyEntries);

            foreach (var block in sections)
            {
                var lines = block.Trim().Split('\n');
                if (lines.Length < 2) continue;
                var hash = lines[0].Trim();
                var subject = lines[1].Trim();
                var body = string.Join('\n', lines[2..]).Trim();

                commits.Add(new GitCommit { Hash = hash, Subject = subject, Body = body });
            }

            return commits;
        }
    }
}
