# 🚀 QUICKSTART — AutoVersion Lite

====================================================================
PURPOSE
====================================================================

This guide walks you through installing, configuring, and running
AutoVersion Lite in under five minutes.

AutoVersion Lite automates version management, changelog generation,
and Git tagging for Unity and .NET projects.

====================================================================
REQUIREMENTS
====================================================================

- .NET 8 SDK or later
- Git 2.40+
- (Optional) Unity 2022.3 LTS

====================================================================
STEP 1 — CLONE THE REPOSITORY
====================================================================

    git clone https://github.com/solcogito/AutoVersion.git
    cd AutoVersion

====================================================================
STEP 2 — BUILD AND TEST
====================================================================

Windows (PowerShell):
    pwsh _Infrastructure/build.ps1 -Release

Linux/macOS (bash):
    bash _Infrastructure/build.sh --release

This restores dependencies, builds all projects, and runs the test suite.

====================================================================
STEP 3 — CONFIGURE AUTOVERSION.JSON
====================================================================

Example configuration file:

{
  "versionFile": "Directory.Build.props",
  "files": [
    { "path": "package.json", "type": "json", "key": "version" },
    { "path": "Directory.Build.props", "type": "xml", "xpath": "/Project/PropertyGroup/Version" }
  ],
  "git": {
    "tagPrefix": "v",
    "push": true
  }
}

Place it in the project root or any working directory.

====================================================================
STEP 4 — RUN THE CLI
====================================================================

Show current version:
    dotnet run --project src/AutoVersion.Cli -- current

Bump patch version:
    dotnet run --project src/AutoVersion.Cli -- bump patch

Simulate (dry run):
    dotnet run --project src/AutoVersion.Cli -- bump minor --dry-run

====================================================================
STEP 5 — GENERATE CHANGELOG
====================================================================

    dotnet run --project src/AutoVersion.Cli -- changelog

AutoVersion will parse your Git commits and regenerate CHANGELOG.md
using Conventional Commit rules.

====================================================================
STEP 6 — UNITY INTEGRATION (OPTIONAL)
====================================================================

1. Open the sample project: samples/Sample.UnityProject/
2. In Unity, open the menu:
       Tools → AutoVersion → Bump Patch
3. Watch version updates propagate automatically.

====================================================================
STEP 7 — PUBLISH YOUR RELEASE
====================================================================

    pwsh _Infrastructure/publish.ps1 -Bump minor

This will:
- Run tests
- Bump version
- Generate changelog
- Commit and tag (vX.Y.Z)
- Push to remote

Example output:

    🔧 Bumping version (minor)
    📝 Generating changelog
    🚀 Release v1.3.0 pushed to origin

====================================================================
TROUBLESHOOTING
====================================================================

• "dotnet not found" → Install .NET 8 SDK
• "dirty working directory" → Commit or stash before publishing
• "autoversion.json invalid" → Validate with AJV or --validate option
• "no tag prefix" → Add "git.tagPrefix" in autoversion.json

====================================================================
NEXT STEPS
====================================================================

• Check out ROADMAP.md for planned features.
• Contribute via pull requests on GitHub.
• Share feedback and ideas on the Solcogito Discord.

====================================================================
END OF QUICKSTART
====================================================================
