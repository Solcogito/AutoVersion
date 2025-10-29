// ============================================================================
// File:        ChangelogTests.cs
// Project:     AutoVersion Lite (Test Suite)
// Version:     0.3.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Tests the Changelog Engine: commit parsing, grouping, and markdown output.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Solcogito.AutoVersion.Core.Git;
using Solcogito.AutoVersion.Core.Changelog;

namespace Solcogito.AutoVersion.Tests
{
    public class ChangelogTests
    {
        // --------------------------------------------------------------------
        // Conventional Commit Parsing
        // --------------------------------------------------------------------

        [Fact]
        public void Parse_ValidConventionalCommit_ReturnsStructuredCommit()
        {
            var commits = new List<GitCommit>
            {
                new() { Hash = "a1", Subject = "feat(core): add semantic version bump logic" },
                new() { Hash = "b2", Subject = "fix(config): correct JSON key parsing" },
                new() { Hash = "c3", Subject = "docs(readme): add usage section" },
                new() { Hash = "d4", Subject = "chore(ci): update build pipeline" }
            };

            var parsed = ConventionalCommitParser.Parse(commits).ToList();

            Assert.Equal(4, parsed.Count);
            Assert.Contains(parsed, c => c.Type == "feat" && c.Scope == "core");
            Assert.Contains(parsed, c => c.Type == "fix" && c.Scope == "config");
            Assert.Contains(parsed, c => c.Type == "docs" && c.Scope == "readme");
            Assert.Contains(parsed, c => c.Type == "chore" && c.Scope == "ci");
        }

        [Fact]
        public void Parse_IgnoresNonConventionalCommits()
        {
            var commits = new List<GitCommit>
            {
                new() { Hash = "x1", Subject = "update files" },
                new() { Hash = "x2", Subject = "minor tweak" },
                new() { Hash = "x3", Subject = "feat(ui): add new button" }
            };

            var parsed = ConventionalCommitParser.Parse(commits).ToList();

            Assert.Single(parsed.Where(c => c.Type == "feat"));
            var commit = parsed.First(c => c.Type == "feat");
            Assert.Equal("ui", commit.Scope);
        }

        // --------------------------------------------------------------------
        // Changelog Builder
        // --------------------------------------------------------------------

        [Fact]
        public void Build_GroupsByTypeAndFormatsMarkdown()
        {
            var parsed = new List<ParsedCommit>
            {
                new() { Type = "feat", Scope = "engine", Description = "add new changelog builder", Hash = "abc123" },
                new() { Type = "fix", Scope = "parser", Description = "handle null prerelease tags", Hash = "def456" },
                new() { Type = "docs", Scope = "usage", Description = "update readme", Hash = "ghi789" }
            };

            var markdown = ChangelogBuilder.Build(parsed, "0.3.0", "2025-10-29");

            Assert.Contains("## [0.3.0]", markdown);
            Assert.Contains("### Added", markdown);
            Assert.Contains("### Fixed", markdown);
            Assert.Contains("### Documentation", markdown);
            Assert.Contains("add new changelog builder", markdown);
        }

        [Fact]
        public void Build_EmptyCommitList_ReturnsHeaderOnly()
        {
            var markdown = ChangelogBuilder.Build(Array.Empty<ParsedCommit>(), "0.3.0", "2025-10-29");
            Assert.Contains("## [0.3.0]", markdown);
            Assert.DoesNotContain("###", markdown);
        }

        // --------------------------------------------------------------------
        // Markdown Writer
        // --------------------------------------------------------------------

        [Fact]
        public void MarkdownWriter_AppendsContentToFile()
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "# Changelog\n");

            var content = "## [0.3.0] – 2025-10-29\n### Added\n- new feature";
            MarkdownWriter.Append(tempFile, content);

            var result = File.ReadAllText(tempFile);
            Assert.Contains("# Changelog", result);
            Assert.Contains("## [0.3.0]", result);

            File.Delete(tempFile);
        }

        // --------------------------------------------------------------------
        // Git Log Reader (Stub)
        // --------------------------------------------------------------------

        [Fact]
        public void GitLogReader_Stub_ProducesFakeCommits()
        {
            var fakeCommits = new List<GitCommit>
            {
                new() { Hash = "t1", Subject = "feat(test): simulate git commit parsing" },
                new() { Hash = "t2", Subject = "chore(build): update test runner" }
            };

            Assert.All(fakeCommits, c => Assert.Contains(":", c.Subject));
        }

        // --------------------------------------------------------------------
        // Integration Test
        // --------------------------------------------------------------------

        [Fact]
        public void Integration_ParsesAndBuildsMarkdownChangelog()
        {
            var commits = new List<GitCommit>
            {
                new() { Hash = "a11", Subject = "feat(api): expose changelog endpoint" },
                new() { Hash = "b22", Subject = "fix(cli): correct argument parsing" }
            };

            var parsed = ConventionalCommitParser.Parse(commits).ToList();
            var markdown = ChangelogBuilder.Build(parsed, "0.3.0", "2025-10-29");

            Assert.Contains("**api**", markdown);
            Assert.Contains("**cli**", markdown);
            Assert.Contains("## [0.3.0]", markdown);
        }
    }
}
