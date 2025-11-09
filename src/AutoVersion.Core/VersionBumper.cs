// ============================================================================
// File:        VersionBumper.cs
// Project:     AutoVersion Lite
// Version:     0.2.1
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Provides high-level methods to increment (bump) versions based on
//   semantic versioning rules. This class delegates version data structure
//   handling to Solcogito.Common.Versioning.VersionModel, ensuring a strict
//   separation between data (API) and logic (CLI/Core).
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Solcogito.Common.Versioning; // Namespace from the shared API

using System;
using System.Linq;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Provides controlled semantic version bumping based on standard SemVer rules.
    /// </summary>
    public static class VersionBumper
    {
        // --------------------------------------------------------------------
        // Public API
        // --------------------------------------------------------------------

        /// <summary>
        /// Increments the specified version according to a bump type.
        /// </summary>
        /// <param name="current">The current version model.</param>
        /// <param name="bumpType">The bump type: "major", "minor", "patch", or "prerelease".</param>
        /// <param name="preRelease">Optional prerelease tag override.</param>
        /// <returns>A new <see cref="VersionModel"/> instance with the incremented version.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="bumpType"/> is invalid.</exception>
        public static VersionModel Bump(VersionModel current, string bumpType, string? preRelease = null)
        {
            if (string.IsNullOrWhiteSpace(bumpType))
                throw new ArgumentException("Bump type must be specified.", nameof(bumpType));

            bumpType = bumpType.Trim().ToLowerInvariant();

            return bumpType switch
            {
                "major" => new VersionModel(current.Major + 1, 0, 0),
                "minor" => new VersionModel(current.Major, current.Minor + 1, 0),
                "patch" => new VersionModel(current.Major, current.Minor, current.Patch + 1),
                "prerelease" => HandlePrerelease(current, preRelease),
                _ => throw new ArgumentException($"Unknown bump type '{bumpType}'.", nameof(bumpType))
            };
        }

        /// <summary>
        /// Attempts to bump a version string safely.
        /// </summary>
        /// <param name="input">The current version string (e.g., "1.2.3-alpha.1").</param>
        /// <param name="bumpType">The bump type.</param>
        /// <param name="preRelease">Optional prerelease override.</param>
        /// <returns>The new version string, or <c>null</c> if parsing fails.</returns>
        public static string? TryBump(string input, string bumpType, string? preRelease = null)
        {
            try
            {
                var version = VersionModel.Parse(input);
                return Bump(version, bumpType, preRelease).ToString();
            }
            catch
            {
                return null;
            }
        }

        // --------------------------------------------------------------------
        // Private Helpers
        // --------------------------------------------------------------------

        /// <summary>
        /// Handles prerelease incrementation logic.
        /// </summary>
        private static VersionModel HandlePrerelease(VersionModel current, string? preRelease)
        {
            // Use provided prerelease tag override, or increment existing one.
            if (!string.IsNullOrEmpty(preRelease))
                return new VersionModel(current.Major, current.Minor, current.Patch, preRelease, current.Metadata);

            var nextPre = IncrementPrerelease(current.PreRelease);
            return new VersionModel(current.Major, current.Minor, current.Patch, nextPre, current.Metadata);
        }

        /// <summary>
        /// Increments the numeric portion of a prerelease tag if present.
        /// </summary>
        private static string IncrementPrerelease(string? pre)
        {
            if (string.IsNullOrEmpty(pre))
                return "alpha.1";

            var parts = pre.Split('.');
            if (int.TryParse(parts[^1], out int num))
            {
                parts[^1] = (num + 1).ToString();
                return string.Join('.', parts);
            }
            return pre + ".1";
        }
    }
}
