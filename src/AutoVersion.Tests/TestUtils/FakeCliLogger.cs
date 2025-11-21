// ============================================================================
// File:        FakeCliLogger.cs
// Project:     AutoVersion Lite (Test Utilities)
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// -----------------------------------------------------------------------------
// Description:
//   In-memory implementation of ICliLogger for unit testing. Captures all log
//   messages in a deterministic list without writing to the real console.
//   Used to validate command behavior and error paths in AutoVersion.Tests.
// ============================================================================

using System.Collections.Generic;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Tests.TestUtils
{
    public sealed class FakeCliLogger : ICliLogger
    {
        public bool DryRun { get; set; }

        public List<string> Messages { get; } = new();

        public void Info(string message)    => Messages.Add("[INFO] " + message);
        public void Action(string message)  => Messages.Add("[ACTION] " + message);
        public void Warn(string message)    => Messages.Add("[WARN] " + message);
        public void Success(string message) => Messages.Add("[SUCCESS] " + message);
        public void Error(string message)   => Messages.Add("[ERROR] " + message);
    }
}
