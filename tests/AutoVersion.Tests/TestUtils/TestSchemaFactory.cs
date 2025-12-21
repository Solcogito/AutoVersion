// ============================================================================
// File:        TestSchemaFactory.cs
// Project:     AutoVersion.Tests
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Builds the same schema as Program.BuildSchema for CLI parsing tests.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Solcogito.Common.ArgForge;

namespace Solcogito.AutoVersion.Tests.TestUtils
{
    internal static class TestSchemaFactory
    {
        public static ArgSchema Build()
        {
            var schema = ArgSchema.Create("autoversion", "Semantic versioning tool");

            schema.Flag("dry-run", null, "--dry-run", "Simulate operation, do not write files");
            schema.Option("path", "-p", "--path", "Explicit path to version file", requiredFlag: false);

            schema.Command("current", "Show the current version");

            schema.Command("set", "Set the version directly", cmd =>
            {
                cmd.Positional("version", 0, "Version to set (e.g., 1.2.3)", required: true);
            });

            schema.Command("bump", "Increment version components", bump =>
            {
                bump.Command("patch", "Increment patch version");
                bump.Command("minor", "Increment minor version");
                bump.Command("major", "Increment major version");

                bump.Command("prerelease", "Increment prerelease version", pre =>
                {
                    pre.Option("pre", null, "--pre", "Prerelease tag", requiredFlag: false);
                });
            });

            return schema;
        }
    }
}
