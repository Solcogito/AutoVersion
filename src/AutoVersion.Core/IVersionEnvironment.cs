// ============================================================================
// File:        IVersionEnvironment.cs
// Project:     AutoVersion Lite
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Abstraction over version resolution + write.
//
//   Invariants:
//   - VersionResolveResult intentionally does NOT carry a mutation target path.
//   - Therefore, write operations MUST be given an explicit path.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Minimal contract required to resolve and persist semantic versions.
    /// </summary>
    /// <remarks>
    /// This interface defines the domain boundary for AutoVersion.
    ///
    /// Non-negotiable invariant:
    /// - <see cref="VersionResolveResult"/> does not include a file path.
    ///
    /// Consequence:
    /// - Any mutation must be targeted explicitly via <see cref="WriteVersion"/>.
    /// - Implementations must never infer a write target from discovery results.
    ///
    /// This interface must remain free of CLI concerns:
    /// - No parsed arguments
    /// - No help schema
    /// - No presentation or formatting
    /// </remarks>
    public interface IVersionEnvironment
    {
        /// <summary>
        /// Logger used for operational output and diagnostics.
        /// </summary>
        Logger Logger { get; }

        /// <summary>
        /// Indicates whether commands may normalize multiple sources to a single selected version.
        /// </summary>
        /// <remarks>
        /// Typical normalization policy:
        /// - Detect multiple sources
        /// - Select highest version
        /// - Align all sources to the chosen version (or bumped version)
        /// </remarks>
        bool AllowNormalize { get; }

        /// <summary>
        /// Short context label for diagnostics. Not intended for branching logic.
        /// </summary>
        string Context { get; }

        /// <summary>
        /// Resolves version candidates and read/parse errors.
        /// </summary>
        /// <param name="explicitPath">
        /// If provided (non-null and non-whitespace), resolution must be performed
        /// using this single path as the sole source. If omitted, the environment's
        /// default sources must be used.
        /// </param>
        /// <returns>
        /// Resolve result containing checked sources, candidates, and structured errors.
        /// </returns>
        /// <remarks>
        /// Implementations should return an empty result when no sources are available,
        /// rather than throwing.
        /// </remarks>
        VersionResolveResult GetCurrentVersions(string? explicitPath = null);

        /// <summary>
        /// Writes a version to an explicit caller-provided target path.
        /// </summary>
        /// <remarks>
        /// Invariant enforcement:
        /// - Writes always require an explicit target path.
        /// - Never infer a write target from discovery results.
        /// </remarks>
        void WriteVersion(VersionModel version, string explicitPath);
    }
}
