// ============================================================================
// File:        SemVerParser.cs
// Project:     AutoVersion Lite
// Version:     0.2.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Provides utility methods for semantic version parsing and validation.
//   Used internally by VersionModel and CLI-level logic for input sanitization,
//   now fully null-safe and typed.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Text.RegularExpressions;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Utility for validating and extracting version components.
    /// </summary>
    public static class SemVerParser
    {
        private static readonly Regex Pattern = new(
            @"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)" +
            @"(?:-(?<pre>[0-9A-Za-z-.]+))?(?:\+(?<build>[0-9A-Za-z-.]+))?$",
            RegexOptions.Compiled
        );

        /// <summary>
        /// Determines whether a version string is syntactically valid.
        /// </summary>
        public static bool IsValid(string version) => Pattern.IsMatch(version);

        /// <summary>
        /// Attempts to parse a version string without throwing exceptions.
        /// </summary>
        public static bool TryParse(string input, out VersionModel? result)
        {
            result = null;
            var match = Pattern.Match(input);
            if (!match.Success) return false;

            result = new VersionModel(
                int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                int.Parse(match.Groups["patch"].Value),
                match.Groups["pre"].Success ? match.Groups["pre"].Value : null,
                match.Groups["build"].Success ? match.Groups["build"].Value : null
            );
            return true;
        }
    }
}
