// ============================================================================
// File:        ChangelogBuilder.cs
// Project:     AutoVersion Lite
// Version:     0.5.2
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Builds and safely updates the markdown changelog from parsed Conventional
//   Commits. Prepends new entries at the top of the file, never overwriting.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Solcogito.AutoVersion.Core.Git;
using Solcogito.AutoVersion.Core.Config;

namespace Solcogito.AutoVersion.Core.Changelog
{
    public class ChangelogBuilder
    {
        private readonly ConfigModel _config;
        private readonly Dictionary<string, string> _sectionMap;

        public ChangelogBuilder(ConfigModel config)
        {
            _config = config;
            _sectionMap = new(StringComparer.OrdinalIgnoreCase)
            {
                ["feat"] = "Added",
                ["fix"] = "Fixed",
                ["docs"] = "Documentation",
                ["refactor"] = "Changed",
                ["chore"] = "Maintenance",
                ["perf"] = "Performance"
            };

            if (config.Changelog?.Sections != null)
            {
                foreach (var kvp in config.Changelog.Sections)
                    _sectionMap[kvp.Key] = kvp.Value;
            }
        }

        // --------------------------------------------------------------------
        // Public API
        // --------------------------------------------------------------------

        /// <summary>
        /// Builds and safely writes the changelog for a given version.
        /// </summary>
        public void Update(string version, bool dryRun = false)
        {
            var path = _config.Changelog?.Path ?? "CHANGELOG.md";
            var sinceTag = $"{_config.Git?.TagPrefix}{version}";
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd");

            var commits = GitLogReader.ReadCommits(sinceTag);
            var parsed = ConventionalCommitParser.Parse(commits);
            var markdown = Build(parsed, version, date);

            if (dryRun)
            {
                Console.WriteLine("\n=== CHANGELOG PREVIEW ===");
                Console.WriteLine(markdown);
                return;
            }

            // Safe prepend logic (preserves all prior entries)
            var existing = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
            var combined = markdown.TrimEnd() + Environment.NewLine + Environment.NewLine + existing.TrimStart();

            File.WriteAllText(path, combined);
            Console.WriteLine($"[INFO] Changelog updated: {path}");
        }

        /// <summary>
        /// Builds the formatted markdown changelog text.
        /// </summary>
        public string Build(IEnumerable<ParsedCommit> commits, string version, string date)
        {
            var list = commits.ToList();
            var sb = new StringBuilder();

            sb.AppendLine($"## [{version}] - {date}");

            if (list.Count == 0)
                return sb.ToString();

            var grouped = list
                .GroupBy(c => _sectionMap.ContainsKey(c.Type) ? _sectionMap[c.Type] : "Other")
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                sb.AppendLine($"### {group.Key}");
                foreach (var commit in group)
                {
                    var scope = string.IsNullOrWhiteSpace(commit.Scope) ? "" : $"**{commit.Scope}**: ";
                    var desc = commit.Description?.Trim() ?? "(no description)";
                    var hash = string.IsNullOrEmpty(commit.Hash)
                        ? ""
                        : $" ({commit.Hash[..Math.Min(commit.Hash.Length, 7)]})";
                    sb.AppendLine($"- {scope}{desc}{hash}");
                }
                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }
    }
}
