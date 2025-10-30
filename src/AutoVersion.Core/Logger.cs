// ============================================================================
// File:        Logger.cs
// Project:     AutoVersion Lite
// Version:     0.4.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Lightweight console logger with dry-run awareness and consistent
//   message formatting for CLI feedback. All ASCII safe.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Provides formatted logging utilities for console output.
    /// </summary>
    public static class Logger
    {
        /// <summary>Global flag indicating whether actions are simulated only.</summary>
        public static bool DryRun { get; set; }

        /// <summary>Writes an informational message.</summary>
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[INFO] " + message);
            Console.ResetColor();
        }

        /// <summary>Writes an action message (e.g., version bump).</summary>
        public static void Action(string message)
        {
            var prefix = DryRun ? "[DRY-RUN]" : "[ACTION]";
            Console.ForegroundColor = DryRun ? ConsoleColor.Yellow : ConsoleColor.Green;
            Console.WriteLine(prefix + " " + message);
            Console.ResetColor();
        }

        /// <summary>Writes a warning message in yellow.</summary>
        public static void Warn(string message)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARN] " + message);
            Console.ForegroundColor = prev;
        }

        /// <summary>Writes a success message in green.</summary>
        public static void Success(string message)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] " + message);
            Console.ForegroundColor = prev;
        }

        /// <summary>Writes an error message in red.</summary>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + message);
            Console.ResetColor();
        }
    }
}
