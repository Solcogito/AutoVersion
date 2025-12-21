// ============================================================================
// File:        Program.cs
// Project:     AutoVersion Lite
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   CLI entrypoint for AutoVersion Lite.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;

using Solcogito.AutoVersion.Cli;
using Solcogito.AutoVersion.Cli.Commands;
using Solcogito.AutoVersion.Core;
using Solcogito.Common.ArgForge;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;
using Solcogito.Common.IOKit;

namespace Solcogito.AutoVersion
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            ArgSchema schema = BuildSchema();
            Logger logger = CreateLogger();

            if (ShouldExitWithHelp(args))
            {
                var helpCliContext = CreateCliContext(
                    new ArgParser().Parse(schema, Array.Empty<string>()),
                    schema);

                WriteRootHelp(schema, helpCliContext!);
                return 0;
            }

            ArgResult parsedArgs = ParseArguments(schema, args, logger);
            if (!parsedArgs.IsValid)
                return 1;

            IVersionEnvironment versionEnvironment =
                CreateVersionEnvironment(parsedArgs, logger);

            ICliContext? cliContext =
                CreateCliContext(parsedArgs, schema);

            return RouteExecution(versionEnvironment, cliContext!);
        }

        // ====================================================================
        // Schema
        // ====================================================================

        private static ArgSchema BuildSchema()
        {
            var schema = ArgSchema.Create(
                "autoversion",
                "Semantic versioning tool"
            );

            schema.Flag(
                "dry-run",
                null,
                "--dry-run",
                "Simulate operation, do not write files"
            );

            schema.Flag(
                "no-normalize",
                null,
                "--no-normalize",
                "Do not normalize when multiple version files are found"
            );

            schema.Option(
                "path",
                "-p",
                "--path",
                "Explicit path to version file",
                requiredFlag: false
            );

            schema.Command(
                "current",
                "Show the current version"
            );

            schema.Command(
                "set",
                "Set the version directly",
                cmd =>
                {
                    cmd.Positional(
                        "version",
                        index: 0,
                        description: "Version to set (e.g., 1.2.3)",
                        required: true
                    );
                }
            );

            schema.Command(
                "bump",
                "Increment version components",
                bump =>
                {
                    bump.Command("patch", "Increment patch version");
                    bump.Command("minor", "Increment minor version");
                    bump.Command("major", "Increment major version");

                    bump.Command(
                        "prerelease",
                        "Increment prerelease version",
                        pre =>
                        {
                            pre.Option(
                                "pre",
                                null,
                                "--pre",
                                "Prerelease tag (alpha.1, rc.2, etc.)",
                                requiredFlag: false
                            );
                        }
                    );
                }
            );

            return schema;
        }

        // ====================================================================
        // Logging
        // ====================================================================

        private static Logger CreateLogger()
        {
            return new Logger()
                .WithMinimumLevel(LogLevel.Info)
                .WithSink(new ConsoleSink(ConsoleSinkRole.Stderr));
        }

        // ====================================================================
        // Help / Early Exit
        // ====================================================================

        private static bool ShouldExitWithHelp(string[] args)
        {
            if (args.Length == 0)
                return true;

            foreach (var arg in args)
            {
                if (arg.Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("--help", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static void WriteRootHelp(
            ArgSchema schema,
            ICliContext cli)
        {
            cli.Output.WriteLine(cli.Help.Format(schema));
        }

        // ====================================================================
        // Argument Parsing
        // ====================================================================

        private static ArgResult ParseArguments(
            ArgSchema schema,
            string[] args,
            Logger logger)
        {
            var parser = new ArgParser();
            ArgResult result = parser.Parse(schema, args);

            if (!result.IsValid)
            {
                logger.Error(result.Error!);
            }

            return result;
        }

        // ====================================================================
        // Environment Construction
        // ====================================================================

        private static IVersionEnvironment CreateVersionEnvironment(
            ArgResult args,
            Logger logger)
        {
            IReadOnlyList<FileVersionSource> defaultSources =
                CreateDefaultVersionSources();

            bool allowNormalize = !args.Flags.ContainsKey("no-normalize");

            return new DefaultVersionEnvironment(
                configuredDefaultSources: defaultSources,
                logger: logger,
                allowNormalize: allowNormalize,
                context: "AutoVersion CLI"
            );
        }

        private static ICliContext? CreateCliContext(
            ArgResult args,
            ArgSchema schema)
        {
            // USER-VISIBLE CLI OUTPUT SINK
            ITextSink output = new ConsoleTextSink();

            // HELP FORMATTER (presentation concern)
            HelpFormatter help = new HelpFormatter();

            return new DefaultCliContext(
                args,
                schema,
                help,
                output);
        }

        private static IReadOnlyList<FileVersionSource> CreateDefaultVersionSources()
        {
            return new List<FileVersionSource>
            {
                new FileVersionSource("version.txt"),
                new FileVersionSource("version.json"),
            };
        }

        // ====================================================================
        // Execution
        // ====================================================================

        private static int RouteExecution(
            IVersionEnvironment environment,
            ICliContext cliContext)
        {
            return CommandRouter.Run(environment, cliContext);
        }
    }
}
