// ============================================================================
// File:        Program.cs
// Project:     AutoVersion Lite
// Version:     0.7.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Application entrypoint. Builds the CLI schema, parses arguments using
//   ArgForge, and forwards execution to the CommandRouter.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;

using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var schema = BuildSchema();

            // If user typed only autoversion OR help flags
            if (args.Length == 0 ||
                args[0].Equals("--help", StringComparison.OrdinalIgnoreCase) ||
                args[0].Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                args[0].Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(schema.GetHelp("autoversion"));
                return 0;
            }

            // Parse arguments
            var result = ArgParser.Parse(args, schema);

            if (!result.IsValid)
            {
                Console.WriteLine("Error: " + result.Error);
                Console.WriteLine();
                Console.WriteLine(schema.GetHelp("autoversion"));
                return 1;
            }

            // Route execution
            return CommandRouter.Run(result, schema);
        }

        // =====================================================================
        // SCHEMA DEFINITION
        // =====================================================================
        private static ArgSchema BuildSchema()
        {
            var schema = ArgSchema.Create("autoversion", "Semantic versioning tool");

            // ------------------------------------------------------------
            // GLOBAL FLAGS (valid for every command)
            // ------------------------------------------------------------
            schema.Flag("dry-run", null, "--dry-run", "Simulate operation, do not write files");

            // ------------------------------------------------------------
            // current  (root command)
            // ------------------------------------------------------------
            schema.Command("current", "Show the current version");

            // ------------------------------------------------------------
            // set <version> 
            // ------------------------------------------------------------
            schema.Command("set", "Set the version directly", cmd =>
            {
                cmd.Positional("version", 0, "Version to set (e.g., 1.2.3)", required: true);
            });

            // ------------------------------------------------------------
            // bump <patch|minor|major|prerelease>
            // ------------------------------------------------------------
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
