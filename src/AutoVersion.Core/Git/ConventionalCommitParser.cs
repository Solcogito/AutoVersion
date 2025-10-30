// ============================================================================
// File:        ConventionalCommitParser.cs
// Project:     AutoVersion Lite
// Version:     0.3.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Parses Conventional Commit messages into structured commit data.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solcogito.AutoVersion.Core.Git
{
    public class ParsedCommit
    {
        public string Type { get; set; } = "chore";
        public string? Scope { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
    }

    public static class ConventionalCommitParser
    {
        private static readonly Regex Pattern = new(
            @"^(?<type>[a-zA-Z]+)(\((?<scope>[^)]+)\))?: (?<desc>.+)$",
            RegexOptions.Compiled
        );

        public static IEnumerable<ParsedCommit> Parse(IEnumerable<GitCommit> commits)
        {
            var list = new List<ParsedCommit>();

            foreach (var c in commits)
            {
                var match = Pattern.Match(c.Subject);
                if (!match.Success)
                {
                    list.Add(new ParsedCommit { Type = "chore", Description = c.Subject, Hash = c.Hash });
                    continue;
                }

                list.Add(new ParsedCommit
                {
                    Type = match.Groups["type"].Value.ToLowerInvariant(),
                    Scope = match.Groups["scope"].Success ? match.Groups["scope"].Value : null,
                    Description = match.Groups["desc"].Value,
                    Hash = c.Hash
                });
            }

            return list;
        }
    }
}
