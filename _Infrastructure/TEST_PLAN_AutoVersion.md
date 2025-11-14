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
| Help screen | `autoversion --help` | Lists commands and flags | ☐ |
| Show version | `autoversion --version` | Prints version number | ☐ |
| Invalid flag | `autoversion --bogus` | Error + exit code 1 | ☐ |
| Dry-run flag | `autoversion bump patch --dry-run` | Displays preview only | ☐ |
| Force flag | `autoversion bump patch --force` | Overrides safety checks | ☐ |

---

### 3.2 Version Bump Operations
| Type | Command | Expected Result | Status |
|------|----------|----------------|--------|
| Patch | `autoversion bump patch` | 0.0.5 → 0.0.6 | ☐ |
| Minor | `autoversion bump minor` | 0.0.5 → 0.1.0 | ☐ |
| Major | `autoversion bump major` | 0.0.5 → 1.0.0 | ☐ |
| Prerelease | `autoversion bump prerelease` | Appends `-alpha.N` suffix | ☐ |
| Custom tag | `--tag beta` | Applies metadata field | ☐ |

---

### 3.3 Config & File Handling
| Case | Setup | Expected Output | Status |
|------|--------|----------------|--------|
| Default file creation | Delete autoversion.json | Program generates default config | ☐ |
| Load valid config | Normal file present | CLI reads config correctly | ☐ |
| Corrupt config | Remove a required field | Clear error message | ☐ |
| Env override | `AUTOVERSION_VERSION=1.2.3` | Value overrides config | ☐ |

---

## 🔗 4. Integration Tests

### 4.1 Git Integration
| Step | Action | Expected |
|------|---------|----------|
| 1 | Initialize repo | Git recognizes version files |
| 2 | Run bump | Commit-ready modified version file | ☐ |
| 3 | Tag manually | `git tag v1.1.2` works | ☐ |
| 4 | Push | No conflicts | ☐ |

---

### 4.2 BuildStamp Integration
| Step | Action | Expected |
|------|---------|----------|
| 1 | Run `autoversion bump patch` | Version increments | ☐ |
| 2 | Run `buildstamp` | buildinfo.json matches version | ☐ |

---

### 4.3 ZipRelease Integration
| Step | Action | Expected |
|------|---------|----------|
| 1 | Version bump → ZipRelease | Version propagated into archive | ☐ |
| 2 | Extract archive | Version consistent in metadata | ☐ |

---

## 🧠 5. Regression Tests

| Case | Description | Expected |
|------|--------------|----------|
| v1.0 configs | Older format still loads | ☐ |
| Branch switching | Bumps remain correct per branch | ☐ |
| Double bump prevention | Back-to-back runs block unless `--force` | ☐ |

---

## ☁️ 6. CI/CD Pipeline Tests
| Step | Command | Expected Result |
|------|----------|----------------|
| Push to main | triggers CI | Workflow succeeds | ☐ |
| Matrix build | Windows/macOS/Linux | All green | ☐ |
| AutoVersion step | Executes bump logic | ☐ |
| Artifacts | Created and zipped | ☐ |

---

## 🧰 7. Manual QA Checklist
| Test | Verification | Status |
|------|---------------|--------|
| Help text readability | Clear, minimal, accurate | ☐ |
| Error messages | Informative and actionable | ☐ |
| Output formatting | Works in PowerShell and bash | ☐ |
| Dry-run clarity | Shows exactly what will happen | ☐ |

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
