# 🧱 ARCHITECTURE — AutoVersion Lite v0.0.1

---

## 🧩 System Overview

AutoVersion follows a **modular layered architecture**, built to support both CLI and Unity Editor integration.

```
┌──────────────────────────────┐
│        AutoVersion.Cli       │  ← CLI entry point (System.CommandLine)
└─────────────┬────────────────┘
              │
┌─────────────▼────────────────┐
│       AutoVersion.Core       │  ← Versioning logic, changelog generator
└─────────────┬────────────────┘
              │
┌─────────────▼────────────────┐
│       AutoVersion.Unity      │  ← Unity Editor UI (optional)
└──────────────────────────────┘
```

---

## 🧱 Core Components

| Component | Purpose |
|------------|----------|
| `VersionModel` | Encapsulates semantic version (major.minor.patch[-pre][+meta]) |
| `VersionManager` | Reads/writes version files, executes bumps |
| `ChangelogService` *(planned)* | Generates Markdown changelog from Git commits |
| `GitService` *(planned)* | Handles tagging, clean checks, and remote push |
| `ConfigLoader` *(planned)* | Parses and validates `autoversion.json` |

---

## 🧭 Execution Flow

### Bump Command

1. CLI receives command → parses arguments  
2. VersionManager reads current version  
3. VersionModel performs bump logic  
4. If `--dry-run`: prints next version only  
5. Otherwise:
   - Writes new version to file
   - (Later phases) updates changelog & tags repo

```
[ CLI ] → [ VersionManager ] → [ VersionModel ] → [ File I/O ] → [ Console Output ]
```

---

## ⚙️ Build System

| Script | Purpose |
|---------|----------|
| `build.ps1` | Restores, builds, and tests all projects |
| `build.sh` | Linux/macOS equivalent |
| `publish.ps1` | Runs tests, bumps version, tags, and pushes |
| `Directory.Build.props` | Defines shared compiler properties (.NET 8, warnings as errors) |

---

## 🧪 Testing Layout

| Project | Test Scope |
|----------|-------------|
| `AutoVersion.Tests` | Unit tests for all Core components |
| Example test file: | `VersionModelTests.cs` — verifies parse, bump, and comparison logic |

Tests are executed automatically in CI via:
```
dotnet test --configuration Release
```

---

## 📁 Integration Points

- **Unity** → uses `AutoVersion.Unity` to invoke Core logic from menu actions.  
- **CI/CD** → GitHub Actions call `publish.ps1` or CLI commands.  
- **Gumroad (future)** → via API or webhook integration.

---

**End of Architecture Overview**
