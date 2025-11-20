// ============================================================================
// File:        ICliLogger.cs
// Project:     AutoVersion Lite
// ----------------------------------------------------------------------------
// Description:
//   Abstraction over CLI logging so commands can be unit-tested by mocking
//   this interface instead of relying on static Logger methods.
// ============================================================================

namespace Solcogito.AutoVersion.Core
{
    public interface ICliLogger
    {
        bool DryRun { get; set; }

        void Info(string message);
        void Action(string message);
        void Warn(string message);
        void Success(string message);
        void Error(string message);
    }
}
