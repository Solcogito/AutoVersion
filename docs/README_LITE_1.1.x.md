# AutoVersion Lite v1.1.x — README

AutoVersion Lite is a **simple, deterministic semantic versioning CLI tool** designed for small to medium projects that need clean and predictable version management without configuration files, Git integration, or Unity dependencies.

This README covers **ONLY the features supported in AutoVersion Lite v1.1.x**.

---

# 🚀 What Is AutoVersion Lite?

AutoVersion Lite provides:

- Semantic version parsing (SemVer 2.0.0)
- Automatic version bumping
- Safe version writes with dry-run support
- Version source resolution (version.json / version.txt)
- Fully testable architecture
- Cross-platform .NET 8 CLI

AutoVersion Lite **does NOT** include:

- autoversion.json configuration  
- multi-file version propagation  
- changelog generation  
- Git tagging  
- Unity integration  
- artifact renaming  
- templates  
- publish pipelines  

These features belong to *other Solcogito versioning tools*.

---

# 📦 Installation

## Requirements
- .NET 8 SDK  
- PowerShell 7+ or bash  
- Windows, macOS, or Linux

## Clone & Build
```bash
git clone https://github.com/Solcogito/AutoVersion.git
cd AutoVersion
pwsh _Infrastructure/build.ps1
```

This builds all projects and runs tests located in:

```
src/AutoVersion.Tests/
```

---

# 🧭 CLI Usage

## Check current version
```bash
autoversion current
```

AutoVersion will read:
- version.json (if present)
- version.txt

It will return the **highest** semantic version.

---

## Set a version
```bash
autoversion set 1.2.3
```

---

## Bump a version
```
autoversion bump patch
autoversion bump minor
autoversion bump major
```

### Prerelease bumps
```bash
autoversion bump pre -p alpha
```

### Dry-run
```bash
autoversion bump patch --dry-run
```

---

# 🔧 Exit Codes

| Code | Meaning |
|------|---------|
| 0 | Success |
| 1 | Invalid input |
| 2 | File I/O or unexpected error |

---

# 📁 Version File Rules

AutoVersion Lite resolves versions as follows:

1. If **version.json** exists and contains a valid SemVer → use it  
2. If **version.txt** exists → use it  
3. If both exist → pick the **highest** version  

---

# 🧪 Development

Run the full test suite:
```bash
dotnet test --configuration Release
```

Or use the unified build script:
```bash
pwsh _Infrastructure/build.ps1
```

---

# 📚 Documentation

| File | Purpose |
|------|---------|
| `docs/USAGE/QUICKSTART_LITE_1.1.x.md` | Quick guide |
| `docs/USAGE/FAQ_LITE_1.1.x.md` | Common questions |
| `docs/DESIGN/ARCHITECTURE.md` | Internal architecture |
| `AUTOVERSION_LITE.md` | Full CLI reference |
| `AUTOVERSION_PRO_PREVIEW.md` | Upcomming Pro version features |
---

# 📜 License

AutoVersion Lite  
© 2025 Solcogito S.E.N.C.  
MIT License

---

**End of README (Lite Edition)**