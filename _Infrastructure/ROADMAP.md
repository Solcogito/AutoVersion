# 🧭 AutoVersion Lite — Development Roadmap
Project Goal: Provide a small, fast, free semantic version bumping tool for Unity and general development pipelines.

---

## 🧱 Overview

| Version | Codename | Focus |
|----------|-----------|-------|
| v0.0.0  | Bootstrap | Environment, repo setup, scaffolding |
| v0.1.0  | SemVer Core | Version parsing, increment logic |
| v0.2.0  | Deferred → *AutoVersion Pro* | Multi-file config & schema (moved) |
| v0.3.0  | Deferred → *AutoChangeLog* | Changelog engine (moved) |
| v0.4.0  | Git Integration | Tagging, repo safety |
| v0.5.0  | Unity Editor Menu | Menu-based version bumping |
| v0.6.0  | Documentation | Install, usage, FAQ |
| v0.7.0  | CI + Quality Gates | Matrix builds, lint/commit checks |
| v0.8.0  | Polish | UX, errors, formatting |
| v1.0.0  | Public Lite Release | Distribution & announcement |

---

# 🧩 v0.0.0 — Bootstrap

**Objective**  
Lay the groundwork for clean, modular, cross-platform development and CI/CD.

**Tasks**
- Initialize Git repository  
- Add `.gitignore` for Unity + .NET  
- Add LICENSE (MIT)  
- Create folder structure:  
  ```
  /src/
    AutoVersion.Core/
    AutoVersion.Cli/
    AutoVersion.Unity/Editor/
  /docs/
  /.github/workflows/
  ```
- Add `.editorconfig` + `.gitattributes`  
- Add empty `CHANGELOG.md`  
- Implement minimal CLI skeleton (`autoversion --help`)  
- Configure CI basics (`ci.yml`)  
- Add xUnit setup  
- Validate build on Windows, macOS, Linux  

---

# ⚙️ v0.1.0 — SemVer Core

**Objective**  
Implement all essential semantic versioning logic.

**Features**
- Parse SemVer: `major.minor.patch[-pre][+build]`  
- Increment: major/minor/patch/prerelease  
- CLI:  
  ```
  autoversion current
  autoversion bump <type>
  ```
- `--dry-run` support  

---

# 🧰 v0.2.0 — Deferred (Moved to AutoVersion Pro)

This milestone originally included multi-file operations, schema validation, token replacement, and advanced configuration.

These features are now part of **AutoVersion Pro**, a separate tool designed for larger pipelines and multi-project setups.

---

# 🧾 v0.3.0 — Deferred (Moved to AutoChangeLog)

Changelog generation and commit parsing have been moved to a dedicated tool: **AutoChangeLog**.

---

# 📦 v0.4.0 — Git Integration

**Objective**  
Provide optional tagging and repo safety checks.

---

# 🎮 v0.5.0 — Unity Editor Menu

Expose AutoVersion Lite directly inside Unity.

---

# 📚 v0.6.0 — Documentation

Complete documentation for all Lite features.

---

# ⚙️ v0.7.0 — CI + Quality Gates

Cross-platform builds and commit conventions.

---

# 🧪 v0.8.0 — Polish & Stability

Final UX pass, safety checks, and CLI clarity.

---

# 🚀 v1.0.0 — Public Lite Release

Stable public release across GitHub and Gumroad.

---

# 🔮 Future (v1.1.0 → v2.0.0 Pro Preview)

- AutoVersion Pro  
- AutoChangeLog  
- Advanced automation  
- Unity package exporter  
- Release GUI  

---

# 🧾 Credits

© 2025 Solcogito S.E.N.C.  
MIT License (Lite)
