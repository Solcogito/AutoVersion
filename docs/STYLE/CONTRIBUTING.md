# 🤝 Contributing to AutoVersion Lite v1.1.x
(c) 2025 Solcogito S.E.N.C.

Thank you for your interest in contributing!  
AutoVersion Lite is a lightweight, deterministic semantic‑versioning CLI designed for clarity, safety, and testability.  
This guide explains how to contribute cleanly and consistently to the project.

---

## 🧩 Table of Contents
1. Setup & Prerequisites  
2. Project Structure  
3. Branch & Commit Rules  
4. Code Style  
5. Pull Requests  
6. Testing  
7. Documentation  
8. Community Guidelines  

---

## 🧰 Setup & Prerequisites

### Requirements
| Tool | Version | Purpose |
|------|---------|---------|
| .NET SDK | 8.0+ | Build Core, CLI, and Tests |
| PowerShell 7+ or Bash | — | Run build scripts |
| Git | Latest | Version control |

### Quick Start
```bash
git clone https://github.com/Solcogito/AutoVersion.git
cd AutoVersion
pwsh _Infrastructure/build.ps1
```

This will:
- Build Core + CLI  
- Run the full test suite  
- Produce artifacts under `/builds/`  

---

## 🧱 Project Structure

AutoVersion Lite uses a minimal and focused architecture:

```
AutoVersion/
├── src/
│   ├── AutoVersion.Core/       # Versioning logic
│   ├── AutoVersion.Cli/        # CLI commands + ArgForge schema
│   └── AutoVersion.Tests/      # All xUnit tests
├── _Infrastructure/            # Build / release scripts
└── docs/                       # Documentation (DESIGN, USAGE, etc.)
```

No Unity integration.  
No Git tagging module.  
No changelog generator.  
Those belong to future Pro modules only.

---

## 🌿 Branch & Commit Rules

### Branch Naming
| Type | Format | Example |
|------|---------|---------|
| Feature | `feature/<short-desc>` | `feature/bump-command-refactor` |
| Fix | `fix/<short-desc>` | `fix/preflag-validation` |
| Docs | `docs/<short-desc>` | `docs/update-tech-overview` |
| Maintenance | `chore/<short-desc>` | `chore/editorconfig-update` |

### Commit Messages (Conventional Commits)
| Type | Usage |
|------|--------|
| `feat:` | New user-facing feature |
| `fix:` | Bug fix |
| `docs:` | Documentation changes |
| `style:` | Formatting; no logic change |
| `refactor:` | Internal improvements |
| `test:` | Add/update tests |
| `chore:` | Maintenance tasks |

Examples:
```
feat: add prerelease bump label handling
fix: prevent invalid version from writing to file
docs: update architecture diagram
```

---

## 🧠 Code Style

AutoVersion Lite follows strict and simple rules:

- 4-space indent for C#  
- 2-space indent for JSON/YAML/Markdown  
- UTF‑8 with LF line endings  
- Explicit access modifiers  
- `var` allowed only when the RHS makes the type obvious  
- Private fields → `_camelCase`  
- Expression-bodied members when it improves clarity  

These are enforced through:
- `.editorconfig`
- `.gitattributes`
- Roslyn analyzers

---

## 🔁 Pull Requests

### Before Submitting a PR
1. Pull the latest **main**
   ```bash
   git pull origin main
   ```
2. Run the full test suite  
   ```bash
   pwsh _Infrastructure/build.ps1
   ```
3. Ensure your changes do not introduce warnings or formatting issues.  
4. Ensure your commit messages follow Conventional Commits.  
5. Update documentation if your change modifies behavior.

### PR Checklist
- [ ] Builds with no warnings  
- [ ] All tests pass  
- [ ] New tests added for new behavior (if required)  
- [ ] Docs updated (`COMPONENT_GUIDE`, `ARCHITECTURE`, etc.)  
- [ ] Commits follow Conventional Commit rules  

---

## 🧪 Testing

All tests live under:

```
src/AutoVersion.Tests/
```

Run them with:
```bash
dotnet test --configuration Release
```

Or through the unified script:
```bash
pwsh _Infrastructure/build.ps1
```

### Test Naming Convention
```
Method_Scenario_ExpectedResult
```

Example:
```
BumpCommand_InvalidPart_ReturnsExit1
```

Tests cover:
- VersionModel parsing + bumping  
- VersionEnvironment file resolution  
- ArgForge schema negative paths  
- All command classes (bump, set, current)  
- Logger output (via FakeCliLogger)  

---

## 🧾 Documentation

Documentation lives in:

```
docs/
  DESIGN/   → Architecture, component guides
  USAGE/    → End-user documentation
  STYLE/    → (empty for Lite; reserved for future guidelines)
```

Update:
- `COMPONENT_GUIDE.md` when new classes appear  
- `ARCHITECTURE.md` when architecture changes  
- `TECH_OVERVIEW.md` when flow or stack changes  

Lite does **not** automatically generate changelogs.

---

## 🌎 Community Guidelines

- Be respectful and constructive  
- Provide clear reproduction steps for issues  
- Attach logs when possible  
- Keep discussions technical and focused  

---

## 💡 In Summary

> “Small, predictable improvements compound into stability.”

- Write clean code  
- Test everything  
- Document changes  
- Contribute consistently  

Welcome to AutoVersion Lite — part of the growing Solcogito ecosystem.

---

**End of Contributing Guide**
