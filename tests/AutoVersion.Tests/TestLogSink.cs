// ============================================================================
// File:        TestLogSink.cs
// Project:     AutoVersion.Tests
// ----------------------------------------------------------------------------
// Description:
//   In-memory LogScribe sink for deterministic CLI testing.
//
//   Captures all formatted LogScribe output without touching Console.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.Text;

using Solcogito.Common.LogScribe;

namespace AutoVersion.Tests
{
    public sealed class TestLogSink : ILogSink
    {
        private readonly StringBuilder _buffer = new();
        private readonly LogFormatter _formatter = new();

        public void Write(LogMessage message)
        {
            if (message == null)
                return;

            _buffer.AppendLine(_formatter.Format(message));
        }

        public string Content => _buffer.ToString();
    }
}
