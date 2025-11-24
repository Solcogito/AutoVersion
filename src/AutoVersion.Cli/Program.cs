// ============================================================================
// File:        Program.cs
// Project:     AutoVersion Lite
// Version:     0.7.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Application entrypoint. Builds the CLI schema, parses arguments using
//   ArgForge, and forwards execution to the CommandRouter with injected
//   services (version environment + logger).
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using Solcogito.Common.ArgForge;
using Solcogito.Common.LogScribe;
using Solcogito.AutoVersion.Core;
using System.Net.WebSockets;

namespace Solcogito.AutoVersion.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var schema = BuildSchema();
            var logger = new Logger()
                .WithMinimumLevel(LogLevel.Debug)
                .WithSink(new ConsoleSink());
            logger.Debug("Debug logging enabled.");
            // Help handling (no args or explicit help)
            if (args.Length == 0 ||
                args[0].Equals("--help", StringComparison.OrdinalIgnoreCase) ||
                args[0].Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                args[0].Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                logger.Internal(schema.GetHelp());
                return 0;
            }

            var parser = new ArgParser();
            var result = parser.Parse(args, schema);

            if (!result.IsValid)
            {
                logger.Error("Error: " + result.Error);
                logger.Internal(schema.GetHelp());
                return 1;
            }

            // ----------------------------------------------------------------
            // Composition root: create concrete services here
            // ----------------------------------------------------------------
            IVersionEnvironment env = new DefaultVersionEnvironment(logger);

            return CommandRouter.Run(result, schema, env, logger);
        }

        // =====================================================================
        // SCHEMA DEFINITION
        // =====================================================================
        private static ArgSchema BuildSchema()
        {
            var schema = ArgSchema.Create("autoversion", "Semantic versioning tool");

            // GLOBAL FLAGS
            schema.Flag("dry-run", null, "--dry-run", "Simulate operation, do not write files");

            // current
            schema.Command("current", "Show the current version");

            // set <version>
            schema.Command("set", "Set the version directly", cmd =>
            {
                cmd.Positional("version", 0, "Version to set (e.g., 1.2.3)", required: true);
            });

            // bump <patch|minor|major|prerelease>
            schema.Command("bump", "Increment version components", bump =>
            {
                bump.Command("patch", "Increment patch version");
                bump.Command("minor", "Increment minor version");
                bump.Command("major", "Increment major version");

                bump.Command("prerelease", "Increment prerelease version", pre =>
                {
                    pre.Option("pre", "-p", "--pre", "Tag name for prerelease bump (alpha.1, rc.2, etc.)", requiredFlag: false);
                });
            });

            return schema;
        }
    }
}
