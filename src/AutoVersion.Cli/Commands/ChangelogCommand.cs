// ============================================================================
// File:        ChangelogCommand.cs
// Project:     AutoVersion Lite
// Version:     0.3.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   CLI command for generating and previewing changelog entries.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Solcogito.AutoVersion.Core.Git;
using Solcogito.AutoVersion.Core.Changelog;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class ChangelogCommand
    {
        /// <summary>
        /// Executes the changelog generator.
        /// </summary>
        /// <param name="sinceTag">Optional tag name to start from.</param>
        /// <param name="dryRun">If true, prints the changelog instead of writing it.</param>
        public static void Run(string? sinceTag = null, bool dryRun = false)
        {
            var commits = GitLogReader.ReadCommits(sinceTag);
            var parsed = ConventionalCommitParser.Parse(commits);

            var version = sinceTag is null ? "Unreleased" : sinceTag.TrimStart('v');
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var builder = new ChangelogBuilder(new Core.Config.ConfigModel());
			var markdown = builder.Build(parsed, version, date);

            if (dryRun)
            {
                Console.WriteLine("=== CHANGELOG PREVIEW ===");
                Console.WriteLine(markdown);
                return;
            }

            MarkdownWriter.Append("CHANGELOG.md", markdown);
            Console.WriteLine($"Changelog updated for {version} ({date})");
        }
		
		public static List<object> RunJson(string? sinceTag, bool dryRun)
        {
            var entries = GenerateEntries(sinceTag);
            var structured = entries.Select(e => new
            {
                e.Type,
                e.Scope,
                e.Message,
                e.Hash
            }).Cast<object>().ToList();

            if (!dryRun)
                Console.WriteLine($"[INFO] Generated {structured.Count} changelog entries.");

            return structured;
        }

        // --------------------------------------------------------------------
        // Helper: Simulated changelog data source (replace with real git parser)
        // --------------------------------------------------------------------
        private static List<(string Type, string Scope, string Message, string Hash)> GenerateEntries(string? sinceTag)
        {
            // This is placeholder logic; your real implementation may parse Git commits.
            return new List<(string, string, string, string)>
            {
                ("feat", "cli", "Add JSON output mode", "abc123"),
                ("fix", "core", "Preserve UTF8/BOM encoding", "def456"),
                ("chore", "", "Polish help messages", "ghi789")
            };
        }
    }
}
