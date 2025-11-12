# 🧭 AutoVersion Lite

[![GitHub release](https://img.shields.io/github/v/release/Solcogito/AutoVersion?include_prereleases)](https://github.com/Solcogito/AutoVersion/releases)
[![Build](https://github.com/Solcogito/AutoVersion/actions/workflows/ci.yml/badge.svg)](https://github.com/Solcogito/AutoVersion/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Win%20%7C%20Mac%20%7C%20Linux-lightgrey)](#)


### Semantic Versioning & Changelog Automation for Unity and .NET

AutoVersion Lite automates version bumps and Git tagging.  
It’s built for Unity and .NET developers who want CI-ready release pipelines — without manual editing.

---

## 🚀 Overview
AutoVersion Lite is a **semantic versioning** tool.  
It updates versions from Conventional Commits and integrates directly with Git and Unity Editor menus.

---

## ✨ Key Features

- ✅ Full Semantic Versioning (SemVer 2.0.0)
- 🔧 Version bumping (major / minor / patch / prerelease)
- 📦 Updates JSON, XML, and text files
- 🏷️ Git tagging
- 🎮 Unity Editor menu integration
- 🧪 Dry-run safety mode
- ⚙️ Config-driven workflow (`autoversion.json`)
- 🌍 Cross-platform & CI-ready
- 📜 MIT licensed

---

## 🧰 Installation

### Requirements
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git 2.40+](https://git-scm.com/)
- *(Optional)* Unity 2022.3 LTS

### Steps
```bash
git clone https://github.com/Solcogito/AutoVersion.git
cd AutoVersion
pwsh _Infrastructure/build.ps1 -Release
```
Run the CLI:
```bash
dotnet run --project src/AutoVersion.Cli -- bump patch
```
---

## 🧩 Basic Usage

Show current version:
```bash
autoversion current
```
Bump patch version:
```bash
autoversion bump patch
```
Dry-run mode (simulate):
```bash
autoversion bump patch --dry-run
```
Create and push tag automatically:
```bash
pwsh _Infrastructure/publish.ps1
```
---

## ⚙️ Configuration Example (autoversion.json)
```json
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
```
---

## 🎮 Unity Integration

Inside Unity 2022.3 LTS:
- Open the sample project in samples/Sample.UnityProject
- Go to menu: Tools → AutoVersion → Bump Patch
- The version will increment and update configured files.

---

## 💻 CLI Command Summary
Prints current detected version
```bash
autoversion current 
```
Increments selected version type
```bash
autoversion bump [major|minor|patch|prerelease]  
```
Checks autoversion.json against schema
```bash
autoversion config --validate  
```
Lists all available commands
```bash
autoversion help  
```
---

## 🧪 CI / CD Workflows

GitHub Actions workflows included:

#### ci.yml
    
- Builds & tests on Windows
- macOS & Linux runners planned (matrix-ready)

#### lint.yml
- Validates Conventional Commits, code style, JSON schema, and docs

#### release-on-tag.yml  
- Automatically builds and publishes GitHub Release on tag (v*)

Example:
```bash
git tag v1.0.0  
git push origin v1.0.0
```
→ AutoVersion builds, runs tests, attaches binaries to Release page.

---

## 🗺️ Roadmap Summary

v0.0.0 – Bootstrap  
- Repo setup, CI, lint, docs, schema

v0.1.0 – SemVer Core  
- Version parsing, bump logic

v0.2.0 – Config & File Ops  
- autoversion.json schema + file replacement

v0.3.0 – Changelog Engine (removed; feature cancelled before release)  
- Originally planned: Conventional Commit parser + markdown generation  
- Feature intentionally removed; kept for historical context only

v0.4.0 – Artifact Handling  
- Versioned renaming of builds

v0.5.0 – Git Integration  
- Tag creation, push, repo checks

v0.6.0 – Unity Editor Menu  
- Tools/AutoVersion UI integration

v0.7.0 – Documentation  
- README, Quickstart, config guides

v0.8.0 – CI + Quality Gates  
- Matrix builds, linting, testing

v0.9.0 – Polish  
- UX improvements, dry-run, edge cases, stability

v1.0.0 – Public Lite Release  
- GitHub + Gumroad launch

---

## 📂 File Structure

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

---

## 🧾 License & Credits

© 2025 Solcogito S.E.N.C.  
Author: Benoit Desrosiers  
License: MIT (Lite version)  
Repository: https://github.com/Solcogito/AutoVersion

---