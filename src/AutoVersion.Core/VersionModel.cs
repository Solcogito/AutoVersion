// ============================================================================
// File:        VersionModel.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Core semantic version representation for AutoVersion Lite.
//   Implements parsing, comparison, and increment (bump) logic.
//   Supports full SemVer 2.0.0 with optional prerelease and metadata parts.
//
//   Example:
//       var v = VersionModel.Parse("1.2.3-alpha+001");
//       var next = v.Bump("patch");
//       Console.WriteLine(next); // 1.2.4
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Text.RegularExpressions;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Represents a semantic version compliant with SemVer 2.0.0.
    /// </summary>
    public sealed class VersionModel : IComparable<VersionModel>
    {
        private static readonly Regex SemVerPattern = new(
            @"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(?:-(?<pre>[0-9A-Za-z\.-]+))?(?:\+(?<meta>[0-9A-Za-z\.-]+))?$",
            RegexOptions.Compiled
        );

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string? PreRelease { get; }
        public string? Metadata { get; }

        public VersionModel(int major, int minor, int patch, string? pre = null, string? meta = null)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            PreRelease = pre;
            Metadata = meta;
        }

        // --------------------------------------------------------------------
        // Parse
        // --------------------------------------------------------------------
        /// <summary>
        /// Parses a semantic version string into a <see cref="VersionModel"/>.
        /// </summary>
        public static VersionModel Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Version cannot be null or empty.", nameof(input));

            var match = SemVerPattern.Match(input.Trim());
            if (!match.Success)
                throw new FormatException($"Invalid semantic version: {input}");

            return new VersionModel(
                int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                int.Parse(match.Groups["patch"].Value),
                match.Groups["pre"].Success ? match.Groups["pre"].Value : null,
                match.Groups["meta"].Success ? match.Groups["meta"].Value : null
            );
        }

        // --------------------------------------------------------------------
        // Bump
        // --------------------------------------------------------------------
        /// <summary>
        /// Returns a new version incremented according to the specified part.
        /// </summary>
        /// <param name="part">major, minor, patch, or prerelease</param>
        /// <param name="pre">Optional prerelease tag (e.g. alpha.1)</param>
        public VersionModel Bump(string part, string? pre = null)
        {
            return part.ToLower() switch
            {
                "major" => new VersionModel(Major + 1, 0, 0, pre),
                "minor" => new VersionModel(Major, Minor + 1, 0, pre),
                "patch" => new VersionModel(Major, Minor, Patch + 1, pre),
                "prerelease" => new VersionModel(Major, Minor, Patch, pre),
                _ => throw new ArgumentException($"Invalid bump type: {part}")
            };
        }

        // --------------------------------------------------------------------
        // CompareTo
        // --------------------------------------------------------------------
        /// <summary>
        /// Compares two <see cref="VersionModel"/> instances.
        /// </summary>
        public int CompareTo(VersionModel? other)
        {
            if (other == null)
                return 1;

            int cmp = Major.CompareTo(other.Major);
            if (cmp != 0) return cmp;

            cmp = Minor.CompareTo(other.Minor);
            if (cmp != 0) return cmp;

            return Patch.CompareTo(other.Patch);
        }

        // --------------------------------------------------------------------
        // ToString
        // --------------------------------------------------------------------
        /// <summary>
        /// Converts the version back to a normalized string.
        /// </summary>
        public override string ToString()
        {
            var baseVer = $"{Major}.{Minor}.{Patch}";
            if (!string.IsNullOrEmpty(PreRelease))
                baseVer += $"-{PreRelease}";
            if (!string.IsNullOrEmpty(Metadata))
                baseVer += $"+{Metadata}";
            return baseVer;
        }
    }
}
