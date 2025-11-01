# 🤝 Contributing to AutoVersion Lite
(c) 2025 Solcogito S.E.N.C.

Thank you for taking the time to contribute!  
AutoVersion Lite is an open project maintained by **Solcogito S.E.N.C.**, designed to automate versioning, changelogs, and tagging for developers and studios.

This guide explains how to set up your environment, make clean contributions, and follow our conventions.

---

## 🧩 Table of Contents
1. [Setup & Prerequisites](#setup--prerequisites)
2. [Project Structure](#project-structure)
3. [Branch & Commit Rules](#branch--commit-rules)
4. [Code Style](#code-style)
5. [Pull Requests](#pull-requests)
6. [Testing](#testing)
7. [Documentation](#documentation)
8. [Community Guidelines](#community-guidelines)

---

## 🧰 Setup & Prerequisites

### Requirements
| Tool | Version | Purpose |
|------|----------|----------|
| .NET SDK | 8.0 or newer | Build Core, CLI, and Tests |
| PowerShell 7+ or Bash | — | Run build & release scripts |
| Git | Latest | Version control |
| Unity Editor (optional) | 2022.3 LTS | For Unity menu integration testing |

### Quick Start
```bash
git clone https://github.com/Solcogito/AutoVersion.git
cd AutoVersion
pwsh _Infrastructure/build.ps1
```

- Builds the Core + CLI projects  
- Runs all tests via xUnit  
- Outputs artifacts to `/builds/`

---

## 🧱 Project Structure

```
AutoVersion/
├── src/
│   ├── AutoVersion.Core/      # Core versioning logic
│   ├── AutoVersion.Cli/       # Command-line interface
│   └── AutoVersion.Unity/     # Unity editor integration (optional)
├── _Infrastructure/           # Build and publish scripts
├── docs/                      # Documentation
│   ├── DESIGN/                # Architecture, components
│   ├── STYLE/                 # Code style & contributing guides
│   └── USAGE/                 # User-facing documentation
└── tests/                     # Unit tests
```

---

## 🌿 Branch & Commit Rules

### Branch Naming
| Type | Format | Example |
|-------|---------|---------|
| Feature | `feature/<short-description>` | `feature/changelog-parser` |
| Fix | `fix/<short-description>` | `fix/version-bump-error` |
| Documentation | `docs/<short-description>` | `docs/update-readme` |
| Maintenance | `chore/<short-description>` | `chore/cleanup` |

### Commit Messages
Use **[Conventional Commits](https://www.conventionalcommits.org/)**.

| Type | Description |
|------|--------------|
| `feat:` | New feature |
| `fix:` | Bug fix |
| `docs:` | Documentation only |
| `style:` | Code style or formatting |
| `refactor:` | Code change without new features |
| `test:` | Add or update tests |
| `chore:` | Maintenance or tooling |

**Examples:**
```
feat: add Git tag support
fix: correct patch bump for prerelease versions
docs: add architecture diagram
```

These are parsed automatically by AutoVersion to generate CHANGELOG.md.

---

## 🧠 Code Style

AutoVersion follows the conventions defined in [CODE_STYLE.md](CODE_STYLE.md).

Quick summary:
- 4-space indent for code  
- 2-space indent for JSON/YAML/Markdown  
- UTF-8 + LF endings across all platforms  
- Explicit access modifiers  
- Expression-bodied members where clear  
- `var` used only when the type is obvious  
- Private fields use `_camelCase`  

All settings are enforced automatically via:
- `.editorconfig`
- `.gitattributes`
- IDE code analyzers (StyleCop / Roslyn)

---

## 🔁 Pull Requests

### Before submitting:
1. **Update your branch** from `main`
   ```bash
   git pull origin main
   ```
2. **Run tests**
   ```bash
   pwsh _Infrastructure/build.ps1 -NoTests:$false
   ```
3. **Ensure no ignored files** are staged:
   ```bash
   git status --ignored
   ```
4. **Verify version bump** and changelog are correct.

### PR Checklist
- [ ] Code builds with no errors or warnings  
- [ ] Tests pass on all platforms  
- [ ] Documentation updated where relevant  
- [ ] Follows [CODE_STYLE.md](CODE_STYLE.md)  
- [ ] Title follows Conventional Commits  

---

## 🧪 Testing

Tests use **xUnit**.  
All tests live under `/src/AutoVersion.Tests/`.

Run locally:
```bash
dotnet test --configuration Release
```

Or via PowerShell:
```bash
pwsh _Infrastructure/build.ps1
```

Test naming convention:
```
MethodName_Scenario_ExpectedResult
```
Example: `BumpVersion_PatchIncrements_LastDigit`

---

## 🧾 Documentation

User and developer docs are located under `/docs/`.

| Type | Location | Purpose |
|------|-----------|----------|
| **Design** | `/docs/DESIGN/` | Architecture & technical overview |
| **Style** | `/docs/STYLE/` | Code standards & contributing |
| **Usage** | `/docs/USAGE/` | User-facing tutorials and configs |

When adding or changing features, update:
- `COMPONENT_GUIDE.md` if a new class or service was added  
- `ARCHITECTURE.md` if the overall system changed  
- `CHANGELOG.md` via your commit messages (AutoVersion will regenerate it)

---

## 🌎 Community Guidelines

Be respectful and constructive.  
We welcome clear suggestions, reproducible bugs, and improvement ideas.  

If you open an issue:
- Include your OS and .NET version
- Describe exact steps to reproduce
- Attach logs or screenshots if applicable

---

## 💡 In Summary

> “Automate style so you can focus on substance.”

- Code cleanly  
- Commit clearly  
- Contribute consistently  

Welcome to the **Recursive Architect** workflow — where every improvement multiplies productivity for the entire Solcogito ecosystem.

---

**End of Contributing Guide**
