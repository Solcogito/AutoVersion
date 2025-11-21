# 🧱 ARCHITECTURE — AutoVersion Lite v1.1.x

---

## 🧩 System Overview

AutoVersion Lite uses a **layered architecture**:

```
┌─────────────────────────────┐
│       AutoVersion.Cli       │  ← CLI (ArgForge, commands)
└─────────────┬───────────────┘
              │
┌─────────────▼───────────────┐
│      AutoVersion.Core       │  ← Versioning logic + environment
└─────────────┬───────────────┘
              │
┌─────────────▼───────────────┐
│      File System I/O        │  ← version.json + version.txt
└─────────────────────────────┘
```

Unity is not part of the Lite version.

---

## 🧱 Core Components

### VersionModel  
- Semantic version structure  
- Parsing / validation  
- Bumping (major/minor/patch/pre)  
- Comparison  

### VersionEnvironment  
- Detect version files  
- Load both  
- Return highest version  
- Safe write operations  

### Logger (ICliLogger)  
- Console output abstraction  
- Mockable for tests  

---

## 🧱 CLI Layer

### ArgForge Schema  
Responsible for:

- Defining commands  
- Options (`--pre`, `--dry-run`)  
- Flags  
- Positional argument rules  
- Type validation  
- Uniform error messages  

Handles:

- Unknown flags  
- Wrong positional count  
- Invalid values  
- Missing required inputs  

### CommandRouter  
- Builds schema  
- Parses input  
- Maps to correct command  
- Routes ArgResult  
- Converts errors into exit codes  

### Commands

#### BumpCommand  
- All bump types supported  
- Optional prerelease label  
- Dry-run support  
- Standard exit codes  

#### SetCommand  
- Validates version  
- Writes if correct  

#### CurrentCommand  
- Loads json + txt  
- Returns highest version  

---

## ⚙️ Build System

| Script | Purpose |
|--------|---------|
| `build.ps1` | Build + test (Windows) |
| `build.sh` | Build + test (Linux/macOS) |
| `publish.ps1` | Test + version bump + future tag integration |
| `Directory.Build.props` | Shared .NET 8 compiler rules |

---

## 🧪 Test Architecture

### VersionModelTests  
- Parse  
- TryParse  
- Normalize  
- Comparison rules  
- Bump logic  

### VersionEnvironmentTests  
- json/txt detection  
- Highest-version resolution  
- Write behavior  

### BumpCommandTests  
- Valid bump types  
- Invalid bump types  
- Pre-label  
- Dry-run  
- Exit codes  

### SetCommandTests  
- Invalid version → exit 1  
- Valid → writes safely  

### CurrentCommandTests  
- No files  
- One file  
- Both files (highest wins)  

### ArgForge Negative Tests  
- Unknown flag  
- Invalid positional count  
- Invalid option  
- Duplicate option  

### FakeCliLogger  
- Deterministic log capture  

---

## 📁 Integration Points

Active:
- CLI usage  
- Test suite  
- CI/CD  

Planned (not implemented):
- Unity integration  
- Git tagging  
- Changelog service  

---

**End of Architecture Overview**
