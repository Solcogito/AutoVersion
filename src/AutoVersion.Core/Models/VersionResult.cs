// ============================================================================
// File:        VersionResult.cs
// Project:     AutoVersion.Core
// Version:     0.9.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Structured result model for version bump operations.
//   Enables JSON serialization for CLI and CI pipelines.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Collections.Generic;

namespace Solcogito.AutoVersion.Core
{
    public sealed record VersionResult(
        string OldVersion,
        string NewVersion,
        List<string> FilesUpdated,
        bool TagCreated
    );
}
