// ============================================================================
// File:        ChangelogBuilder.cs
// Project:     AutoVersion Lite
// Version:     0.3.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Builds a markdown changelog from parsed Conventional Commits.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solcogito.AutoVersion.Core.Git;

namespace Solcogito.AutoVersion.Core.Changelog
{
    public static class ChangelogBuilder
    {
        private static readonly Dictionary<string, string> SectionMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["feat"] = "Added",
            ["fix"] = "Fixed",
            ["docs"] = "Documentation",
            ["refactor"] = "Changed",
            ["chore"] = "Maintenance",
            ["perf"] = "Performance"
        };

        /// <summary>
        /// Builds the formatted markdown changelog.
        /// </summary>
        public static string Build(IEnumerable<ParsedCommit> commits, string version, string date)
        {
            var list = commits.ToList();
            var sb = new StringBuilder();

            sb.AppendLine($"## [{version}] - {date}");

            if (list.Count == 0)
                return sb.ToString();

            // Group commits by section (Added, Fixed, etc.)
            var grouped = list
                .GroupBy(c => SectionMap.ContainsKey(c.Type) ? SectionMap[c.Type] : "Other")
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                sb.AppendLine($"### {group.Key}");
                foreach (var commit in group)
                {
                    var scope = string.IsNullOrWhiteSpace(commit.Scope) ? "" : $"**{commit.Scope}**: ";
                    var desc = commit.Description?.Trim() ?? "(no description)";
                    var hash = string.IsNullOrEmpty(commit.Hash) ? "" : $" ({commit.Hash[..Math.Min(commit.Hash.Length, 7)]})";
                    sb.AppendLine($"- {scope}{desc}{hash}");
                }
                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }
    }
}
