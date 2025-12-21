// ============================================================================
// File:        VersionSelection.cs
// Project:     AutoVersion.Core
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Shared helper for selecting a version from VersionResolveResult.
//   - Supports normalization (select highest)
//   - Can forbid normalization when requested
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using Solcogito.Common.Versioning;
using Solcogito.Common.LogScribe;

namespace Solcogito.AutoVersion.Core
{
    internal static class VersionSelection
    {
        public static VersionCandidate Select(
            IReadOnlyList<VersionCandidate> candidates,
            Logger logger,
            bool allowNormalize,
            string context)
        {
            if (candidates.Count == 0)
                throw new InvalidOperationException("No version candidates available.");

            if (candidates.Count == 1)
                return candidates[0];

            if (!allowNormalize)
                throw new InvalidOperationException(
                    "Multiple version candidates found and normalization is disabled.");

            VersionCandidate highest = candidates[0];

            for (int i = 1; i < candidates.Count; i++)
            {
                if (candidates[i].Version.CompareTo(highest.Version) > 0)
                    highest = candidates[i];
            }

            logger.Info(
                $"[normalize] {context}: selected highest version {highest.Version} " +
                $"from {candidates.Count} candidates");

            return highest;
        }
    }
}
