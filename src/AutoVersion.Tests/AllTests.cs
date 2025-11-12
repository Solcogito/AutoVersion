// ============================================================================
// File:        AllTests.cs
// Project:     AutoVersion Lite (Unified Test Suite)
// Version:     0.4.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Unified and cleaned test suite covering core AutoVersion functionality.
//   Includes tests for semantic versioning, configuration handling,
//   artifact renaming, and CLI bump integration.
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
using Solcogito.AutoVersion.Core.Git;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Tests
{
    // ------------------------------------------------------------------------
    // 1. Semantic Versioning Tests
    // ------------------------------------------------------------------------
    public class SemVerTests
    {
        [Fact]
        public void Compare_OrdersProperly()
        {
            var a = VersionModel.Parse("1.0.0");
            var b = VersionModel.Parse("1.1.0");
            Assert.True(a.CompareTo(b) < 0);
        }
    }

    // ------------------------------------------------------------------------
    // 2. Artifact Handling Tests
    // ------------------------------------------------------------------------
        public class ArtifactTests
    {
        private readonly string Dir;
        private const string FileName = "sample.txt";

        public ArtifactTests()
        {
            Dir = Path.Combine(Path.GetTempPath(), "AutoVersion_ArtifactTest_" + Guid.NewGuid().ToString("N"));
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
            Assert.True(File.Exists(dest), $"Expected file not found: {dest}");
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
    // 3. Integrated Pipeline Tests
    // ------------------------------------------------------------------------
	public class PipelineIntegrationTests
    {
        private readonly string Sandbox;

        public PipelineIntegrationTests()
        {
            Sandbox = Path.Combine(Path.GetTempPath(), "AutoVersion_Pipeline_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Sandbox);
        }
    }

		
	// ------------------------------------------------------------------------
	// 4. Git Integration Tests (v0.5.0)
	// ------------------------------------------------------------------------
	[Trait("Category", "Integration")]
	public class GitIntegrationTests : IDisposable
	{
		private readonly string RepoDir;
		private readonly string OriginalDir;

		public GitIntegrationTests()
		{
			// Use unique temp directory for isolation
			RepoDir = Path.Combine(Path.GetTempPath(), "AutoVersion_GitTest_" + Guid.NewGuid().ToString("N"));
			OriginalDir = Directory.GetCurrentDirectory();

			Directory.CreateDirectory(RepoDir);
			Directory.SetCurrentDirectory(RepoDir);

			// Initialize a fake Git repo
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
			bool clean = false;
			for (int i = 0; i < 3; i++)
			{
				clean = GitService.IsClean();
				if (clean) break;
				System.Threading.Thread.Sleep(100); // short wait for Windows FS flush
			}

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

			// Try again — should not throw or duplicate
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

		// --------------------------------------------------------------------
		// Cleanup — robust for Windows file locks
		// --------------------------------------------------------------------
		public void Dispose()
		{
			try
			{
				Directory.SetCurrentDirectory(OriginalDir);
				if (Directory.Exists(RepoDir))
				{
					foreach (var file in Directory.GetFiles(RepoDir, "*", SearchOption.AllDirectories))
					{
						try { File.SetAttributes(file, FileAttributes.Normal); File.Delete(file); }
						catch { /* ignore locked files */ }
					}
					Directory.Delete(RepoDir, true);
				}
			}
			catch { /* swallow cleanup exceptions */ }
		}
	}
}
