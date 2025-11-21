# 🧭 TECH OVERVIEW — AutoVersion Lite v1.1.x

---

## 🎯 Purpose

AutoVersion Lite provides **predictable semantic version automation** for .NET and general projects through a simple CLI built on a custom schema parser.

No Unity integration.  
No Git integration.  
No changelog automation (yet).  
Just clean, safe, testable versioning.

---

## 🧱 Architecture Summary

AutoVersion is composed of three active layers:

| Layer | Location | Role |
|-------|----------|------|
| **Core** | `AutoVersion.Core` | Version parsing, comparison, bumping, file I/O environment |
| **CLI** | `AutoVersion.Cli` | Commands, ArgForge schema, and routing |
| **Tests** | `AutoVersion.Tests` | Full unit test suite for all commands and parsing |

Future (not implemented): `AutoVersion.Unity`

---

## ⚙️ Technology Stack

| Category | Technology |
|----------|------------|
| Runtime | .NET 8 |
| Language | C# 12 |
| Parsing | Custom ArgForge schema |
| Testing | xUnit |
| CI/CD | GitHub Actions |
| Scripting | PowerShell / Bash |

---

## 🔁 CLI Workflow

### Example: Bump

1. User calls:  
   ```
   autoversion bump minor
   ```
2. ArgForge schema validates input  
3. VersionEnvironment loads version.json + version.txt  
4. VersionModel bumps it  
5. Writes file unless dry-run  
6. CLI logs output  
7. Exit code returned  

---

## 🗂 Source of Truth: Version Files

AutoVersion reads:

- `version.json`  
- `version.txt`

**Rule:**  
> AutoVersion uses the highest valid semantic version.

Example:  
- version.json → `1.2.3`  
- version.txt → `1.1.0`  

Current version = **1.2.3**

---

## 🧠 Design Principles

- **Predictable** — deterministic version changes  
- **Safe** — dry-run support, isolated environment  
- **Injectable & Testable** — all dependencies abstracted  
- **Minimal** — semantic versioning only  
- **Transparent** — human-readable output  

---

## 🚧 Future Direction

Not part of v1.1.x:

- Changelog generation  
- Git tagging API  
- Multi-project config  
- Unity Editor UI  
- Webhooks  

---

**End of Tech Overview**
