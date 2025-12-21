using Solcogito.AutoVersion.Cli;
using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Tests.TestUtils
{
    internal static class TestArgs
    {
        public static ArgResult Parse(params string[] argv)
        {
            var schema = ArgSchema.Create(
                rootName: "autoversion",
                description: "AutoVersion test schema"
            );

            var parser = new ArgParser();
            return parser.Parse(schema, argv);
        }
    }
}
