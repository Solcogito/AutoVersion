using System.Collections.Generic;

using Solcogito.Common.LogScribe;

namespace Solcogito.AutoVersion.Tests.TestUtils
{
    public sealed class TestLogSink : ILogSink
    {
        public List<LogMessage> Messages { get; } = new();

        public void Write(LogMessage message)
        {
            Messages.Add(message);
        }
    }
}
