// ============================================================================
// File:        ConsoleCliLogger.cs
// Project:     AutoVersion Lite
// ----------------------------------------------------------------------------
// Description:
//   Adapter that exposes the static Logger as an ICliLogger instance so
//   commands can depend on the interface, while production still uses the
//   existing console logger behavior.
// ============================================================================

namespace Solcogito.AutoVersion.Core
{
    public sealed class ConsoleCliLogger : ICliLogger
    {
        public bool DryRun
        {
            get => Logger.DryRun;
            set => Logger.DryRun = value;
        }

        public void Info(string message)    => Logger.Info(message);
        public void Action(string message)  => Logger.Action(message);
        public void Warn(string message)    => Logger.Warn(message);
        public void Success(string message) => Logger.Success(message);
        public void Error(string message)   => Logger.Error(message);
    }
}
