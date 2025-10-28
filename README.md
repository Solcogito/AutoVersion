# 🧭 AutoVersion Lite — README

====================================================================
PROJECT OVERVIEW
====================================================================

AutoVersion Lite is a semantic versioning and changelog automation tool
for Unity and .NET projects.

It automatically bumps versions, updates files, and generates changelogs
from Conventional Commits — all from a single command.

====================================================================
KEY FEATURES
====================================================================

• Full Semantic Versioning (SemVer 2.0.0)
• Version bumping (major / minor / patch / prerelease)
• Updates JSON, XML, and text files
• Auto-generates CHANGELOG.md from Conventional Commits
• Git tagging & changelog integration
• Unity Editor menu integration
• Dry-run safety mode
• Config-driven workflow (autoversion.json)
• MIT licensed, cross-platform, CI-ready

====================================================================
INSTALLATION
====================================================================

Requirements:
- .NET 8 SDK
- Git 2.40+
- (Optional) Unity 2022.3 LTS

Clone the repository:
    git clone https://github.com/solcogito/AutoVersion.git
    cd AutoVersion

Build and test:
    pwsh _Infrastructure/build.ps1 -Release

Run CLI:
    dotnet run --project src/AutoVersion.Cli -- bump patch

====================================================================
BASIC USAGE
====================================================================

Show current version:
    autoversion current

Bump patch version:
    autoversion bump patch

Dry-run mode (simulate):
    autoversion bump patch --dry-run

Generate changelog:
    autoversion changelog

Create and push tag automatically:
    pwsh _Infrastructure/publish.ps1

====================================================================
CONFIGURATION EXAMPLE (autoversion.json)
====================================================================

{
  "versionFile": "Directory.Build.props",
  "files": [
    { "path": "package.json", "type": "json", "key": "version" },
    { "path": "Directory.Build.props", "type": "xml", "xpath": "/Project/PropertyGroup/Version" },
    { "path": "AssemblyInfo.cs", "type": "regex", "pattern": "AssemblyVersion\\(\"(.*?)\"\\)" }
  ],
  "artifacts": [
    { "path": "Builds/Product.unitypackage", "rename": "Product_{version}.unitypackage" }
  ],
  "git": {
    "tagPrefix": "v",
    "push": true
  }
}

====================================================================
UNITY INTEGRATION
====================================================================

Inside Unity 2022.3 LTS:
- Open the sample project in samples/Sample.UnityProject
- Go to menu: Tools → AutoVersion → Bump Patch
- The version will increment and update configured files.

====================================================================
CLI COMMAND SUMMARY
====================================================================

autoversion current
    Prints current detected version

autoversion bump [major|minor|patch|prerelease]
    Increments selected version type

autoversion changelog
    Generates or updates CHANGELOG.md

autoversion config --validate
    Checks autoversion.json against schema

autoversion help
    Lists all available commands

====================================================================
RELEASE WORKFLOW (CI/CD)
====================================================================

GitHub Actions workflows included:

ci.yml
    - Builds and tests across Windows, macOS, and Linux

lint.yml
    - Validates Conventional Commits, code style, JSON schema, and docs

release-on-tag.yml
    - Automatically builds and publishes GitHub Release on tag (v*)

Example:
    git tag v1.0.0
    git push origin v1.0.0

→ AutoVersion builds, runs tests, attaches binaries to Release page.

====================================================================
ROADMAP SUMMARY
====================================================================

v0.0.0 – Bootstrap
    - Repo setup, CI, lint, docs, schema

v0.1.0 – SemVer Core
    - Version parsing, bump logic

v0.2.0 – Config & File Ops
    - autoversion.json schema + file replacement

v0.3.0 – Changelog Engine
    - Conventional Commit parser, markdown generation

v0.4.0 – Artifact Handling
    - Versioned renaming of builds

v0.5.0 – Git Integration
    - Tag creation, push, repo checks

v0.6.0 – Unity Editor Menu
    - Tools/AutoVersion/ UI integration

v0.7.0 – Documentation
    - README, Quickstart, config guides

v0.8.0 – CI + Quality Gates
    - Matrix builds, linting, testing

v0.9.0 – Polish
    - UX, dry-run, edge cases, stability

v1.0.0 – Public Lite Release
    - GitHub + Gumroad launch

====================================================================
FILE STRUCTURE
====================================================================

AutoVersion/
├── .editorconfig
├── .gitattributes
├── .gitignore
├── LICENSE
├── README.md
├── ROADMAP.md
├── CHANGELOG.md
├── autoversion.json
├── _Infrastructure/
│   ├── Directory.Build.props
│   ├── build.ps1
│   ├── build.sh
│   ├── publish.ps1
│   ├── commitlint.config.json
│   ├── autoversion.schema.json
│   └── package.json
├── src/
│   ├── AutoVersion.Core/
│   ├── AutoVersion.Cli/
│   ├── AutoVersion.Unity/
│   └── AutoVersion.Tests/
├── samples/
│   ├── Sample.UnityProject/
│   └── Sample.CliProject/
├── docs/
│   ├── QUICKSTART.md
│   ├── CONFIG.md
│   ├── WORKFLOWS.md
│   ├── UNITY.md
│   ├── FAQ.md
│   └── IMAGES/
└── .github/workflows/
    ├── ci.yml
    ├── lint.yml
    └── release-on-tag.yml

====================================================================
CREDITS
====================================================================

© 2025 Solcogito S.E.N.C.  
Author: benoit Desrosiers
License: MIT (Lite version)  
Repository: https://github.com/Solcogito/AutoVersion

====================================================================
BADGES (to add after first successful CI run)
====================================================================

[![CI](https://github.com/solcogito/AutoVersion/actions/workflows/ci.yml/badge.svg)](https://github.com/solcogito/AutoVersion/actions/workflows/ci.yml)
[![Lint](https://github.com/solcogito/AutoVersion/actions/workflows/lint.yml/badge.svg)](https://github.com/solcogito/AutoVersion/actions/workflows/lint.yml)

====================================================================
END OF README
====================================================================
