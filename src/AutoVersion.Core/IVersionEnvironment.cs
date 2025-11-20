// ============================================================================
// File:        IVersionEnvironment.cs
// Project:     AutoVersion Lite
// ----------------------------------------------------------------------------
// Description:
//   Abstraction for reading the current version and writing the version file.
//   Wraps VersionResolver + VersionFile so CLI code can be unit-tested with
//   mocks.
// ============================================================================

using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Core
{
    public interface IVersionEnvironment
    {
        /// <summary>Gets the current project version (from version.txt or default).</summary>
        VersionModel GetCurrentVersion();

        /// <summary>Resolves the version file path.</summary>
        string GetVersionFilePath();

        /// <summary>Writes the specified version to the version file.</summary>
        void WriteVersion(VersionModel version);
    }
}
