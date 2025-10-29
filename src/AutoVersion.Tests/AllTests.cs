// ============================================================================
// File:        AllTests.cs
// Project:     AutoVersion Lite (Unified Test Suite)
// Version:     0.4.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Unified and cleaned test suite covering core AutoVersion functionality.
//   Includes tests for semantic versioning, configuration handling,
//   changelog generation, artifact renaming, and CLI bump integration.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Solcogito.AutoVersion.Core;
using Solcogito.AutoVersion.Core.Config;
using Solcogito.AutoVersion.Core.Artifacts;
using Solcogito.AutoVersion.Core.Changelog;
using Solcogito.AutoVersion.Core.Git;

namespace Solcogito.AutoVersion.Tests
{
    // ------------------------------------------------------------------------
    // 1. Semantic Versioning Tests
    // ------------------------------------------------------------------------
    public class SemVerTests
    {
        [Fact]
        public void Parse_ValidStrings()
        {
            var v = VersionModel.Parse("1.2.3-alpha+build42");
            Assert.Equal(1, v.Major);
            Assert.Equal(2, v.Minor);
            Assert.Equal(3, v.Patch);
            Assert.Equal("alpha", v.Prerelease);
            Assert.Equal("build42", v.Build);
        }

        [Fact]
        public void Bump_IncrementsCorrectly()
        {
            var v = VersionModel.Parse("1.2.3");
            Assert.Equal("2.0.0", v.Bump("major").ToString());
            Assert.Equal("1.3.0", v.Bump("minor").ToString());
            Assert.Equal("1.2.4", v.Bump("patch").ToString());
        }

        [Fact]
        public void Compare_OrdersProperly()
        {
            var a = VersionModel.Parse("1.0.0");
            var b = VersionModel.Parse("1.1.0");
            Assert.True(a.CompareTo(b) < 0);
        }
    }

    // ------------------------------------------------------------------------
    // 2. Changelog Builder Tests
    // ------------------------------------------------------------------------
    public class ChangelogTests
    {
        private readonly ConfigModel _config = new()
        {
            Changelog = new()
            {
                Path = "CHANGELOG_TEST.md",
                Sections = new()
                {
                    ["feat"] = "Added",
                    ["fix"] = "Fixed"
                }
            }
        };

        [Fact]
        public void Build_GeneratesMarkdown()
        {
            var builder = new ChangelogBuilder(_config);
            var commits = new[]
            {
                new ParsedCommit { Type = "feat", Description = "Add feature X", Hash = "abc123" },
                new ParsedCommit { Type = "fix", Description = "Fix bug Y", Hash = "def456" }
            };

            var markdown = builder.Build(commits, "1.0.0", "2025-10-29");

            Assert.Contains("## [1.0.0] - 2025-10-29", markdown);
            Assert.Contains("Add feature X", markdown);
            Assert.Contains("Fix bug Y", markdown);
        }

        [Fact]
        public void Update_WritesToFile()
        {
            var builder = new ChangelogBuilder(_config);
            var path = _config.Changelog.Path!;
            if (File.Exists(path)) File.Delete(path);

            builder.Update("1.0.0", dryRun: false);

            Assert.True(File.Exists(path));
            var text = File.ReadAllText(path);
            Assert.Contains("1.0.0", text);
        }
    }

    // ------------------------------------------------------------------------
    // 3. Artifact Handling Tests
    // ------------------------------------------------------------------------
    public class ArtifactTests
    {
        private const string Dir = "BuildTest";
        private const string FileName = "sample.txt";

        public ArtifactTests()
        {
            if (Directory.Exists(Dir)) Directory.Delete(Dir, true);
            Directory.CreateDirectory(Dir);
            File.WriteAllText(Path.Combine(Dir, FileName), "data");
        }

        [Fact]
        public void ProcessArtifacts_RenamesFile()
        {
            var src = Path.Combine(Dir, FileName);
            var destPattern = "sample_{version}.txt";

            var rules = new List<ArtifactRule>
            {
                new ArtifactRule
                {
                    Path = src,
                    Rename = destPattern,
                    Overwrite = true
                }
            };

            ArtifactManager.ProcessArtifacts(rules, "1.2.3", dryRun: false, force: true);

            var dest = Path.Combine(Dir, "sample_1.2.3.txt");
            Assert.True(File.Exists(dest));
        }

        [Fact]
        public void ProcessArtifacts_DryRunDoesNotModify()
        {
            var src = Path.Combine(Dir, FileName);
            var destPattern = "sample_{version}.txt";

            var rules = new List<ArtifactRule>
            {
                new ArtifactRule { Path = src, Rename = destPattern, Overwrite = false }
            };

            ArtifactManager.ProcessArtifacts(rules, "9.9.9", dryRun: true, force: false);

            var dest = Path.Combine(Dir, "sample_9.9.9.txt");
            Assert.False(File.Exists(dest));
        }
    }

    // ------------------------------------------------------------------------
    // 4. Integrated Pipeline Tests
    // ------------------------------------------------------------------------
    public class PipelineIntegrationTests
    {
        private readonly string Sandbox = "PipelineSandbox";

        public PipelineIntegrationTests()
        {
            if (Directory.Exists(Sandbox)) Directory.Delete(Sandbox, true);
            Directory.CreateDirectory(Sandbox);
        }

        [Fact]
        public void FullPipeline_CreatesChangelogAndArtifact()
        {
            var config = new ConfigModel
            {
                Changelog = new() { Path = Path.Combine(Sandbox, "CHANGELOG.md") },
                Artifacts = new()
                {
                    new() { Path = Path.Combine(Sandbox, "demo.txt"), Rename = "demo_{version}.txt", Overwrite = true }
                },
                Git = new() { TagPrefix = "v" }
            };

            File.WriteAllText(config.Artifacts[0].Path!, "test-data");

            var builder = new ChangelogBuilder(config);
            var commits = new[]
            {
                new ParsedCommit { Type = "feat", Description = "Pipeline feature", Hash = "abcd123" }
            };

            var markdown = builder.Build(commits, "1.0.0", "2025-10-29");
            File.WriteAllText(config.Changelog.Path!, markdown);

            ArtifactManager.ProcessArtifacts(
                config.Artifacts.Select(a => new ArtifactRule
                {
                    Path = a.Path,
                    Rename = a.Rename,
                    Overwrite = a.Overwrite
                }),
                "1.0.0",
                dryRun: false,
                force: true
            );

            Assert.True(File.Exists(config.Changelog.Path!));
            Assert.True(File.Exists(Path.Combine(Sandbox, "demo_1.0.0.txt")));
        }
    }
}
