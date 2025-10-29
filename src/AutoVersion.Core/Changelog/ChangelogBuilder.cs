// ============================================================================
// File:        ChangelogBuilder.cs
// Project:     AutoVersion Lite
// Version:     0.3.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Groups parsed commits into markdown-formatted changelog sections.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solcogito.AutoVersion.Core.Git;

namespace Solcogito.AutoVersion.Core.Changelog
{
    public static class ChangelogBuilder
    {
        private static readonly string[] Order = { "feat", "fix", "perf", "docs", "chore" };

        public static string Build(IEnumerable<ParsedCommit> commits, string version, string date)
        {
            var grouped = commits
                .GroupBy(c => c.Type)
                .OrderBy(g => System.Array.IndexOf(Order, g.Key));

            var sb = new StringBuilder();
            sb.AppendLine($"## [{version}] – {date}");

            foreach (var group in grouped)
            {
                sb.AppendLine($"### {Capitalize(group.Key)}");
                foreach (var c in group)
                {
                    var scope = c.Scope != null ? $"**{c.Scope}**: " : "";
                    sb.AppendLine($"- {scope}{c.Description} ({c.Hash[..7]})");
                }
                sb.AppendLine();
            }

            return sb.ToString().Trim();
        }

        private static string Capitalize(string input)
            => string.IsNullOrEmpty(input) ? input : char.ToUpper(input[0]) + input[1..];
    }
}
