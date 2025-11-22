// ============================================================================
// File:        DefaultVersionEnvironment.cs
// Project:     AutoVersion Lite
// ----------------------------------------------------------------------------
// Description:
//   Default implementation of IVersionEnvironment that delegates to the
//   existing VersionResolver + VersionFile API from Solcogito.Common.Versioning.
// ============================================================================

using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Core
{
    public sealed class DefaultVersionEnvironment : IVersionEnvironment
    {
        public VersionResolutionResult GetCurrentVersion()
        {
            return VersionResolver.ResolveVersionDetailed();
        }

        public void WriteVersion(VersionResolutionResult vR)
        {
            if (string.IsNullOrWhiteSpace(vR.FilePath))
                throw new InvalidOperationException(
                    "VersionResolutionResult.FilePath is null or empty. Cannot write version file.");

            VersionFile.Write(vR.FilePath, vR.Version);
        }

        public void WriteVersion(VersionModel version)
        {
            var vR = GetCurrentVersion();
            if (string.IsNullOrWhiteSpace(vR.FilePath))
                throw new InvalidOperationException(
                    "Current version file path is null or empty. Cannot write version file.");
            VersionFile.Write(vR.FilePath, version);
        }
    }
}
