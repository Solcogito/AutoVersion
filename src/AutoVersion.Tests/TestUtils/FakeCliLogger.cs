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
