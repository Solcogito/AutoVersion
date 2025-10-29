// ============================================================================
// File:        VersionModel.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Core semantic version representation and manipulation logic.
//   Supports parsing, comparison, and controlled bumping of version fields
//   following the Semantic Versioning 2.0.0 specification.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Text.RegularExpressions;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Represents a semantic version (SemVer) compliant structure.
    /// Supports parsing, comparison, and version incrementation logic.
    /// </summary>
    public class VersionModel : IComparable<VersionModel>
    {
        // --------------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------------

        /// <summary>Major version number (breaking changes).</summary>
        public int Major { get; }

        /// <summary>Minor version number (feature additions).</summary>
        public int Minor { get; }

        /// <summary>Patch version number (bug fixes).</summary>
        public int Patch { get; }

        /// <summary>Optional prerelease tag (e.g. "alpha.1").</summary>
        public string Prerelease { get; }

        /// <summary>Optional build metadata (e.g. "build42").</summary>
        public string Build { get; }

        // --------------------------------------------------------------------
        // Internal Constants
        // --------------------------------------------------------------------

        private static readonly Regex SemVerRegex = new(
            @"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)" +
            @"(?:-(?<pre>[0-9A-Za-z-.]+))?(?:\+(?<build>[0-9A-Za-z-.]+))?$",
            RegexOptions.Compiled
        );

        // --------------------------------------------------------------------
        // Constructors
        // --------------------------------------------------------------------

        /// <summary>
        /// Creates a new <see cref="VersionModel"/> instance.
        /// </summary>
        public VersionModel(int major, int minor, int patch, string prerelease = null, string build = null)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Prerelease = prerelease;
            Build = build;
        }

        // --------------------------------------------------------------------
        // Static Factory Methods
        // --------------------------------------------------------------------

        /// <summary>
        /// Parses a string into a <see cref="VersionModel"/> instance.
        /// </summary>
        /// <exception cref="FormatException">If the input is not a valid SemVer.</exception>
        public static VersionModel Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var match = SemVerRegex.Match(input);
            if (!match.Success)
                throw new FormatException($"Invalid semantic version: '{input}'");

            return new VersionModel(
                int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                int.Parse(match.Groups["patch"].Value),
                match.Groups["pre"].Success ? match.Groups["pre"].Value : null,
                match.Groups["build"].Success ? match.Groups["build"].Value : null
            );
        }

        // --------------------------------------------------------------------
        // Public Methods
        // --------------------------------------------------------------------

        /// <summary>
        /// Returns a new version incremented according to the provided bump type.
        /// </summary>
        /// <param name="type">One of "major", "minor", "patch", or "prerelease".</param>
        /// <param name="prerelease">Optional prerelease tag override.</param>
        public VersionModel Bump(string type, string prerelease = null)
        {
            return type.ToLowerInvariant() switch
            {
                "major" => new VersionModel(Major + 1, 0, 0),
                "minor" => new VersionModel(Major, Minor + 1, 0),
                "patch" => new VersionModel(Major, Minor, Patch + 1),
                "prerelease" => new VersionModel(Major, Minor, Patch, prerelease ?? NextPrerelease(), Build),
                _ => throw new ArgumentException($"Unknown bump type: '{type}'")
            };
        }

        /// <summary>
        /// Converts the version object to its canonical string form.
        /// </summary>
        public override string ToString()
        {
            var baseVersion = $"{Major}.{Minor}.{Patch}";
            if (!string.IsNullOrEmpty(Prerelease)) baseVersion += $"-{Prerelease}";
            if (!string.IsNullOrEmpty(Build)) baseVersion += $"+{Build}";
            return baseVersion;
        }

        /// <summary>
        /// Compares this version with another SemVer instance.
        /// </summary>
        public int CompareTo(VersionModel other)
        {
            if (other == null) return 1;
            int result = Major.CompareTo(other.Major);
            if (result != 0) return result;
            result = Minor.CompareTo(other.Minor);
            if (result != 0) return result;
            result = Patch.CompareTo(other.Patch);
            if (result != 0) return result;
            return string.Compare(Prerelease ?? "", other.Prerelease ?? "", StringComparison.Ordinal);
        }

        // --------------------------------------------------------------------
        // Private Helpers
        // --------------------------------------------------------------------

        /// <summary>
        /// Generates the next prerelease identifier in sequence.
        /// </summary>
        private string NextPrerelease()
        {
            if (string.IsNullOrEmpty(Prerelease))
                return "alpha.1";

            var parts = Prerelease.Split('.');
            if (int.TryParse(parts[^1], out int num))
            {
                parts[^1] = (num + 1).ToString();
                return string.Join('.', parts);
            }
            return Prerelease + ".1";
        }
    }
}
