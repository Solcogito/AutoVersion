// ============================================================================
// File:        Program.cs
// Project:     AutoVersion Lite
// Version:     0.9.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Entry point for the AutoVersion CLI.
//   Delegates argument handling to CommandRouter and propagates exit codes.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;

namespace Solcogito.AutoVersion.Cli
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                CommandRouter.Run(args);
                return Environment.ExitCode;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Fatal error: {ex.Message}");
                Console.ResetColor();
                return 1;
            }
        }
    }
}
