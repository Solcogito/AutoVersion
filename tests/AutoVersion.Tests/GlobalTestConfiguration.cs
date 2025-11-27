// ============================================================================
// File:        GlobalTestConfiguration.cs
// Project:     AutoVersion Lite (Unified Test Suite)
// Version:     0.8.0
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Global xUnit configuration. Disables test parallelization because the CLI
//   layer uses global Console streams, which cannot safely be captured from
//   multiple tests at the same time.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

// IMPORTANT: Using directives MUST COME BEFORE the assembly attribute.
using Xunit;

// IMPORTANT: Assembly attribute MUST BE OUTSIDE any namespace,
// and MUST come AFTER using directives.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
