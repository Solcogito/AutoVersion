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
		
		// ------------------------------------------------------------------------
		// 5. Git Integration Tests (v0.5.0)
		// ------------------------------------------------------------------------
		public class GitIntegrationTests
		{
			private readonly string RepoDir = "GitSandbox";

			public GitIntegrationTests()
			{
				if (Directory.Exists(RepoDir))
					Directory.Delete(RepoDir, true);
				Directory.CreateDirectory(RepoDir);
				Directory.SetCurrentDirectory(RepoDir);

				// Initialize a fake git repo
				RunGit("init");
				File.WriteAllText("README.md", "test");
				RunGit("add .");
				RunGit("commit -m \"initial commit\"");
			}

			private static string RunGit(string args)
			{
				var psi = new System.Diagnostics.ProcessStartInfo("git", args)
				{
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};
				using var proc = System.Diagnostics.Process.Start(psi)!;
				var output = proc.StandardOutput.ReadToEnd();
				proc.WaitForExit();
				return output;
			}

			[Fact]
			public void GitService_CreatesTagSuccessfully()
			{
				Assert.True(GitService.IsClean());

				var tag = "v0.5.0-test";
				GitService.CreateTag(tag, "Test tag creation");

				var output = RunGit("tag --list v0.5.0-test");
				Assert.Contains("v0.5.0-test", output);
			}

			[Fact]
			public void GitService_DetectsDirtyRepo()
			{
				File.WriteAllText("dirty.txt", "dirty");
				var isClean = GitService.IsClean();
				Assert.False(isClean);
			}

			[Fact]
			public void GitService_PreventsDuplicateTag()
			{
				GitService.CreateTag("v0.5.1-test", "First tag");
				var before = RunGit("tag --list v0.5.1-test").Trim();

				// Try again — should not throw
				GitService.CreateTag("v0.5.1-test", "Duplicate tag");
				var after = RunGit("tag --list v0.5.1-test").Trim();

				Assert.Equal(before, after);
			}

			[Fact]
			public void GitService_HasRemoteFalseByDefault()
			{
				var result = GitService.HasRemote();
				Assert.False(result);
			}
		}

    }
}
