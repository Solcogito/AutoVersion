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
        public VersionModel GetCurrentVersion()
        {
            return VersionResolver.ResolveVersionDetailed().Version;
        }

        public string GetVersionFilePath()
        {
            return VersionResolver.ResolveVersionFilePath();
        }

        public void WriteVersion(VersionModel version)
        {
            var path = GetVersionFilePath();
            VersionFile.Write(path, version);
        }
    }
}
