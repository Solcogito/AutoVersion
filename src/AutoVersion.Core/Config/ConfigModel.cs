// ============================================================================
// File:        ConfigModel.cs
// Project:     AutoVersion Lite
// Version:     0.2.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Defines the primary configuration model used by AutoVersion to drive
//   version propagation and artifact operations.
//   Maps directly to autoversion.json schema and supports nullable reference
//   types for optional properties.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Collections.Generic;

namespace Solcogito.AutoVersion.Core.Config
{
    /// <summary>
    /// Root configuration model loaded from autoversion.json.
    /// </summary>
    public class ConfigModel
    {
        /// <summary>Primary version source file.</summary>
        public string VersionFile { get; set; } = string.Empty;

        /// <summary>List of files to update with version changes.</summary>
        public List<FileTarget> Files { get; set; } = new();

        /// <summary>List of build artifacts to rename after release.</summary>
        public List<ArtifactTarget> Artifacts { get; set; } = new();

        /// <summary>Git integration options (tag, push, commit message).</summary>
        public GitOptions Git { get; set; } = new();

        /// <summary>Simulate without modifying files.</summary>
        public bool DryRun { get; set; } = false;

        /// <summary>Controls verbosity and console color.</summary>
        public LoggingOptions Logging { get; set; } = new();
    }

    // ------------------------------------------------------------------------

    /// <summary>Represents a single file to update.</summary>
    public class FileTarget
    {
        public string Path { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;   // json, xml, regex, text

        // Optional depending on file type
        public string? Key { get; set; }       // for JSON
        public string? Pattern { get; set; }   // for regex
        public string? XPath { get; set; }     // for XML
        public string? Token { get; set; }     // for text
        public string? Encoding { get; set; }  // utf-8, ascii, etc.
    }

    /// <summary>Represents a build artifact to rename using the version number.</summary>
    public class ArtifactTarget
    {
        public string Path { get; set; } = string.Empty;
        public string Rename { get; set; } = string.Empty;
        public bool Overwrite { get; set; } = false;
    }

    // ------------------------------------------------------------------------

    /// <summary>Git integration options for tagging and pushing.</summary>
    public class GitOptions
    {
        public string TagPrefix { get; set; } = "v";
        public bool Push { get; set; } = true;
        public bool AllowDirty { get; set; } = false;
        public string CommitMessage { get; set; } =
            "chore(release): bump version to {version}";
    }

    /// <summary>Controls verbosity and color output for CLI logs.</summary>
    public class LoggingOptions
    {
        public bool Verbose { get; set; } = false;
        public bool Color { get; set; } = true;
        public string Level { get; set; } = "info";
    }
}
