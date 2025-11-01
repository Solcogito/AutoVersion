# 🚀 QUICKSTART — AutoVersion Lite

This guide walks you through installing, configuring, and running AutoVersion Lite in under five minutes.  
AutoVersion Lite automates version management, changelog generation, and Git tagging for Unity and .NET projects.

---

## 🧩 Requirements

- .NET 8 SDK or later  
- Git 2.40+  
- (Optional) Unity 2022.3 LTS

---

## 🧱 Step 1 — Clone the Repository

    git clone https://github.com/Solcogito/AutoVersion.git
    cd AutoVersion

---

## ⚙️ Step 2 — Build and Test

Windows (PowerShell):  
    pwsh _Infrastructure/build.ps1 -Release  

Linux/macOS (bash):  
    bash _Infrastructure/build.sh --release  

This restores dependencies, builds all projects, and runs tests.

---

## ⚙️ Step 3 — Configure autoversion.json

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

Place it in the project root.

---

## 💻 Step 4 — Run the CLI

Show current version:  
    dotnet run --project src/AutoVersion.Cli -- current  

Bump patch version:  
    dotnet run --project src/AutoVersion.Cli -- bump patch  

Simulate (dry run):  
    dotnet run --project src/AutoVersion.Cli -- bump minor --dry-run  

---

## 🧾 Step 5 — Generate Changelog

    dotnet run --project src/AutoVersion.Cli -- changelog

AutoVersion parses Git commits and updates CHANGELOG.md automatically.

---

## 🎮 Step 6 — Unity Integration (Optional)

1. Open the sample project: samples/Sample.UnityProject/  
2. In Unity, open menu: Tools → AutoVersion → Bump Patch  
3. Watch version updates propagate automatically.

---

## 🚀 Step 7 — Publish Your Release

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

---

## 🧠 Troubleshooting

• "dotnet not found" → Install .NET 8 SDK  
• "dirty working directory" → Commit or stash before publishing  
• "autoversion.json invalid" → Validate with AJV or --validate option  
• "no tag prefix" → Add "git.tagPrefix" in autoversion.json  

---

## 🔗 Next Steps

• Check out ROADMAP.md for planned features.  
• Contribute via pull requests on GitHub.  
• Share feedback and ideas on the Solcogito Discord.  

---

**End of Quickstart**
