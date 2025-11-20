using System;
using Solcogito.Common.ArgForge;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class CurrentCommand
    {
        /// <summary>
        /// Displays the current project version.
        /// </summary>
        public static int Execute(ArgResult args, IVersionEnvironment env, ICliLogger logger)
        {
            try
            {
                var version = env.GetCurrentVersion().ToString();
                Console.WriteLine(version);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error reading current version: " + ex.Message);
                logger.Error("Error reading current version: " + ex.Message);
                return 1;
            }
        }
    }
}
