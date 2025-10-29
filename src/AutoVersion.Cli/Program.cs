// ============================================================================
// File:        Program.cs
// Project:     AutoVersion Lite
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Entry point for the AutoVersion CLI. Delegates argument handling to
//   CommandRouter. Handles fatal exceptions gracefully and ensures clean exit.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;

namespace Solcogito.AutoVersion.Cli
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CommandRouter.Run(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Fatal error: {ex.Message}");
                Console.ResetColor();
                Environment.Exit(1);
            }
        }
    }
}
