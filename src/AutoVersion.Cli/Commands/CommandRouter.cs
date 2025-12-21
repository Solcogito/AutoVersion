// ============================================================================
// File:        CommandRouter.cs
// Project:     AutoVersion
// ============================================================================

using System;
using System.Collections.Generic;
using System.CommandLine;

using Solcogito.AutoVersion.Core;
using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Cli.Commands
{
    internal static class CommandRouter
    {
        private static readonly Dictionary<string, Func<IVersionEnvironment, ICliContext, int>>
            RootHandlers = new(StringComparer.OrdinalIgnoreCase)
            {
                { "current", (e, c) => CurrentCommand.Execute(e, c) },
                { "set",     (e, c) => SetCommand.Execute(e, c)     },
            };

        public static int Run(IVersionEnvironment env, ICliContext cli)
        {
            var args = cli.Args;

            if (args.Command == null)
            {
                env.Logger.Stdout(cli.Help.Format(cli.Schema));
                return 0;
            }

            if (IsHelpRequested(args))
            {
                env.Logger.Stdout(cli.Help.Format(args.Command));
                return 0;
            }

            if (args.Command.Parent?.Name == "bump")
                return BumpCommand.Execute(env, cli);

            if (RootHandlers.TryGetValue(args.Command.Name, out var handler))
                return handler(env, cli);

            WriteCommandList(cli, args.Command);
            return 1;
        }

        private static void WriteCommandList(
            ICliContext cli,
            ArgCommand command)
        {
            cli.Output.WriteLine("Commands:");

            foreach (var sub in command.Subcommands.Values)
            {
                cli.Output.WriteLine($"  {sub.Name}");
            }
        }

        private static bool IsHelpRequested(ArgResult args)
            => args.Flags.ContainsKey("help")
            || args.Flags.ContainsKey("h");
    }
}
