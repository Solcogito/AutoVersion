# 🧭 TECH_OVERVIEW — AutoVersion Lite v0.0.1

---

## 🎯 Purpose

AutoVersion Lite is a lightweight cross-platform **semantic versioning and changelog automation tool**  
for Unity and .NET projects. It is designed to streamline release workflows by automating version bumps,  
CHANGELOG generation, and Git tagging from the command line or within the Unity Editor.

---

## 🧱 Architecture Summary

AutoVersion consists of three main layers:

| Layer | Location | Description |
|-------|-----------|-------------|
| **Core** | `src/AutoVersion.Core/` | Contains the logic for version parsing, bumping, and changelog management. |
| **CLI** | `src/AutoVersion.Cli/` | Provides a command-line interface using `System.CommandLine`. |
| **Unity** | `src/AutoVersion.Unity/` | Integrates AutoVersion functionality directly into the Unity Editor menus. |

Support systems include:
- `_Infrastructure/` — build and release automation scripts
- `.github/workflows/` — CI/CD pipelines
- `/docs/` — developer documentation and configuration schema

---

## ⚙️ Technology Stack

| Category | Technology |
|-----------|-------------|
| Runtime | .NET 8 SDK |
| Language | C# 12 |
| Build | MSBuild + PowerShell + Bash |
| Testing | xUnit |
| CI/CD | GitHub Actions |
| Packaging | UPM / .NET CLI |
| Editor Integration | Unity 2022.3 LTS (Editor-only assemblies) |

---

## 🔁 Development Flow

1. Developer runs `pwsh _Infrastructure/build.ps1 -Release`
2. CLI builds Core + Tests
3. Tests run automatically (xUnit)
4. `publish.ps1` bumps version and updates changelog
5. Git tag created and pushed
6. CI builds on all OSes and releases artifacts

---

## 🧠 Design Principles

- **Single Source of Truth**: version number lives in one place and propagates to all files.  
- **Predictable Automation**: every step (bump → changelog → tag) is reproducible and reversible.  
- **Cross-Platform**: works identically on Windows, macOS, Linux, and within Unity.  
- **Readable & Transparent**: all output is human-readable — no black box behavior.  
- **Zero External Dependencies**: no npm, pip, or node toolchains — pure .NET & Unity.

---

## 🧩 Future Direction

- AutoVersion Pro will extend Core functionality:
  - Multi-project configs
  - GUI “Release Window”
  - Webhooks (Discord, Gumroad)
  - Integration with asset build pipelines

---

**End of Tech Overview**
