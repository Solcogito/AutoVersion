using System;
using System.IO;

namespace Solcogito.AutoVersion.Tests.TestUtils
{
    /// <summary>
    /// Ensures Console.Out and Console.Error are safely redirected and restored,
    /// preventing cross-test pollution and ObjectDisposedException issues.
    /// </summary>
    public sealed class ConsoleCapture : IDisposable
    {
        private readonly TextWriter originalOut;
        private readonly TextWriter originalErr;

        public StringWriter OutWriter { get; }
        public StringWriter ErrWriter { get; }

        public ConsoleCapture()
        {
            originalOut = Console.Out;
            originalErr = Console.Error;

            OutWriter = new StringWriter();
            ErrWriter = new StringWriter();

            Console.SetOut(OutWriter);
            Console.SetError(ErrWriter);
        }

        public void Dispose()
        {
            Console.SetOut(originalOut);
            Console.SetError(originalErr);

            OutWriter.Dispose();
            ErrWriter.Dispose();
        }
    }
}
