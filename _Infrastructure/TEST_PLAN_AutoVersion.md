# 🧪 TEST_PLAN.md — AutoVersion Lite v1.1.2

Project: **AutoVersion Lite**  
Maintainer: **Solcogito S.E.N.C.**  
Target Version: **1.1.2 (Stable Release)**  
Test Environments: Windows 11, Ubuntu 22.04, macOS Sonoma  
Tools: .NET 8, PowerShell 7, GitHub Actions, BuildStamp, ZipRelease

---

## 🧱 1. Objectives
- Validate all `bump` and `current` commands.
- Confirm configuration loading, default creation, and merging behavior.
- Ensure stable version increments across OS and shells.
- Verify integration with BuildStamp and ZipRelease.
- Certify CLI stability for professional pipelines.

---

## 🧩 2. Test Categories

| Category | Description |
|-----------|-------------|
| 🧪 Unit Tests | CLI parsing, version model behavior, I/O operations |
| 🔗 Integration Tests | Git operations, file updates, BuildStamp/ZipRelease |
| 🧠 Regression Tests | Compatibility with older configs and workflows |
| 🧰 Manual QA | CLI usability, readability, error clarity |
| ☁️ CI/CD Tests | GitHub Actions execution, matrix builds |

---

## 🧪 3. Unit Tests

### 3.1 CLI Argument Parsing
| Case | Command | Expected Output | Status |
|------|----------|----------------|--------|
| Help screen | `autoversion --help` | Lists commands and flags | ✅ | <!-- '-h' and 'help' also ok -->
| Show version | `autoversion currrent` | Prints version number | ✅ |
| Dry-run flag | `autoversion bump patch --dry-run` | Displays preview only | ✅ | <!-- what is display preview only? -->
| Force flag | `autoversion bump patch --force` | Overrides safety checks | ✅ |  <!-- Feathure removed, will be handle by another program -->

---

### 3.2 Version Bump Operations
| Type | Command | Expected Result | Status |
|------|----------|----------------|--------|
| Patch | `autoversion bump patch` | 0.0.5 → 0.0.6 | ✅ |
| Minor | `autoversion bump minor` | 0.0.5 → 0.1.0 | ✅ |
| Major | `autoversion bump major` | 0.0.5 → 1.0.0 | ✅ |
| Prerelease | `autoversion bump prerelease` | Appends `-alpha.N` suffix | ✅ |
| Custom tag | `--tag beta` | Applies metadata field | ✅ | <!-- Feathure removed, will be handle by another program -->

---

### 3.3 Config & File Handling
| Case | Setup | Expected Output | Status |
|------|--------|----------------|--------|
| Default file creation | Delete autoversion.json | Program generates default config | ✅ | <!-- no autoversion.json -->
| Load valid config | Normal file present | CLI reads config correctly | ✅ | <!-- only Normal file is relevent now -->
| Corrupt config | Remove a required field | Clear error message | ✅ | <!-- Coloration for message error has been remove.Will be handle by something else -->
| Env override | `AUTOVERSION_VERSION=1.2.3` | Value overrides config | ✅ | <!-- AutoVersion Pro -->

### 3.4 Set Operations
| Case                   | Setup                           | Expected Output                         | Status |
| ---------------------- | ------------------------------- | --------------------------------------- | ------ |
| Set valid version      | `autoversion set 2.5.0`         | Files updated & version applied         | ✅     |
| Invalid version string | `autoversion set abc`           | Clear validation error                  | ✅     |
| Dry-run                | Add `--dry-run`                 | No file written; simulation output only | ✅     |
| With prerelease        | `autoversion set 1.3.0-beta.2`  | Version applied with prerelease         | ✅     |

---

## 🔗 4. Integration Tests

### 4.1 Git Integration
| Step | Action | Expected | <!-- Git operations will be handle by either Pro another program -->
|------|---------|----------|
| 1 | Initialize repo | Git recognizes version files | ✅ |
| 2 | Run bump | Commit-ready modified version file | ✅ |
| 3 | Tag manually | `git tag v1.1.2` works | ✅ |
| 4 | Push | No conflicts | ✅ |

---

### 4.2 BuildStamp Integration
| Step | Action | Expected |
|------|---------|----------|
| 1 | Run `autoversion bump patch` | Version increments | ✅ | 
| 2 | Run `buildstamp` | buildinfo.json matches version | ✅ | <!-- see BuildStamp -->

---

### 4.3 ZipRelease Integration
| Step | Action | Expected | <!-- removed from AutoVersion -->
|------|---------|----------|
| 1 | Version bump → ZipRelease | Version propagated into archive | ☐ |
| 2 | Extract archive | Version consistent in metadata | ☐ |

---

## 🧠 5. Regression Tests

| Case | Description | Expected |
|------|--------------|----------|
| v1.0 configs | Older format still loads | ✅ | <!-- config folder has been removed -->
| Branch switching | Bumps remain correct per branch |✅ | <!-- AutoVersion has been simplefied, limited to it's field -->
| Double bump prevention | Back-to-back runs block unless `--force` |✅ | <!-- Force has been removed -->

---

## ☁️ 6. CI/CD Pipeline Tests
| Step | Command | Expected Result |
|------|----------|----------------|
| Push to main | triggers CI | Workflow succeeds | ✅ |
| Matrix build | Windows/macOS/Linux | All green | ✅ |

---

## 🧰 7. Manual QA Checklist
| Test | Verification | Status |
|------|---------------|--------|
| Help text readability | Clear, minimal, accurate | ✅ | 
| Error messages | Informative and actionable | ✅ | <!-- coloration in future program -->
| Output formatting | Works in PowerShell and bash | ☐ |<!-- not directly tested yet, but .net is all fonctionnal -->
| Dry-run clarity | Shows exactly what will happen | ✅ |

---

## 🧾 8. Coverage Summary
| Layer | Goal | Status |
|-------|--------|--------|
| Unit | 90% CLI logic coverage | ☐ |
| Integration | All pipeline tools covered | ☐ |
| Regression | Backward compatibility ensured | ☐ |
| Manual | Critical UX paths validated | ☐ |
| CI/CD | All OS builds green | ☐ |

---

## 🧭 9. Acceptance Criteria (v1.1.2)

- [ ] CLI works end-to-end  
- [ ] Config creation + loading validated  
- [ ] Version bumping stable across OS  
- [ ] Environment variable overrides work  
- [ ] BuildStamp integration verified  
- [ ] ZipRelease integration verified  
- [ ] All CI pipelines green  

---

MIT © 2025 Solcogito S.E.N.C.
