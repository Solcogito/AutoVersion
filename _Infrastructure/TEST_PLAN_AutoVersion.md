# 🧪 TEST_PLAN.md — AutoVersion v1.1.2

Project: AutoVersion (Lite)
Maintainer: Solcogito S.E.N.C.
Target Version: 1.1.2 (Stable Release)
Test Environments: Windows 11, Ubuntu 22.04, macOS Sonoma
Tools: .NET 8, PowerShell 7, GitHub Actions, BuildStamp, ZipRelease

## 🧱 1. Objectives
- Validate all `bump`, `current`, and `changelog` commands.  
- Ensure CHANGELOG entries and version fields update correctly.  
- Confirm configuration and schema parsing works with autoversion.json.  
- Verify integration with BuildStamp and ZipRelease.  
- Certify CLI stability across platforms and shells.

## 🧩 2. Test Categories

| Category | Description |
|-----------|-------------|
| 🧪 Unit Tests | Function-level tests for bump logic, semantic versioning, and I/O parsing |
| 🔗 Integration Tests | Git integration, CHANGELOG updates, and cross-tool chaining |
| 🧠 Regression Tests | Ensure old configs and pre-1.1.0 formats still work |
| 🧰 Manual QA | CLI usability and text output readability |
| ☁️ CI/CD Tests | GitHub Actions execution and artifact generation |

## 🧪 3. Unit Tests

### 3.1 CLI Argument Parsing
| Case | Command | Expected Output | Status |
|------|----------|----------------|--------|
| Help screen | `autoversion --help` | Lists commands and flags | ☐ |
| Show version | `autoversion --version` | Prints v1.1.2 | ☐ |
| Invalid flag | `autoversion --bogus` | Error + exit code 1 | ☐ |
| Dry-run flag | `autoversion bump patch --dry-run` | Displays preview only | ☐ |

### 3.2 Bump Operations
| Type | Command | Expected Result | Status |
|------|----------|----------------|--------|
| Patch | `autoversion bump patch` | 0.0.5 → 0.0.6 | ☐ |
| Minor | `autoversion bump minor` | 0.0.5 → 0.1.0 | ☐ |
| Major | `autoversion bump major` | 0.0.5 → 1.0.0 | ☐ |
| Prerelease | `autoversion bump prerelease` | Appends `-alpha.N` suffix | ☐ |
| Custom identifier | `--tag beta` | Adds metadata field | ☐ |

### 3.3 CHANGELOG Generation
| Case | Command | Expected Output | Status |
|------|----------|----------------|--------|
| Default | `autoversion changelog` | Updates CHANGELOG.md | ☐ |
| Append entry | Run twice | New entry added, old kept | ☐ |
| Invalid file | Missing CHANGELOG | Error with clear message | ☐ |

## 🔗 4. Integration Tests

### 4.1 Git and BuildStamp Integration
| Step | Action | Expected |
|------|---------|----------|
| 1 | Run `autoversion bump patch` | Version incremented |
| 2 | Run `buildstamp` | Version matches AutoVersion output | ☐ |
| 3 | Verify artifact | buildinfo.json contains updated version | ☐ |

### 4.2 ZipRelease Integration
| Step | Action | Expected |
|------|---------|----------|
| 1 | Run AutoVersion then ZipRelease | Version propagated into archive | ☐ |
| 2 | Unzip artifact | Version matches CHANGELOG.md | ☐ |

### 4.3 Config & Environment
| Test | Setup | Expected |
|------|--------|----------|
| Load config | autoversion.json present | CLI reads defaults | ☐ |
| Env override | `AUTOVERSION_BUILD=999` | Field updated | ☐ |
| Merge logic | CLI flag beats config | ☐ |

## 🧠 5. Regression Tests
| Case | Description | Expected |
|------|--------------|----------|
| v1.0 configs | Old format compatible | ✅ parses |
| Multi-branch | Switch branch between bumps | Version tracks each branch |
| Double bump guard | Prevents multiple runs in same commit | Error shown |

## ☁️ 6. CI/CD Pipeline Tests
| Step | Command | Expected Result |
|------|----------|----------------|
| Push to main | triggers autoversion-test.yml | Workflow passes |
| Step AutoVersion | bump + commit | ✅ |
| Step BuildStamp | build metadata | ✅ |
| Step ZipRelease | zipped output | ✅ |
| Total duration | < 1 min per OS | ✅ |

## 🧰 7. Manual QA Checklist
| Test | Verification | Status |
|------|---------------|--------|
| Help text clarity | Sections and examples readable | ☐ |
| Output color formatting | Readable in PowerShell and bash | ☐ |
| Changelog preview | No placeholder tokens | ☐ |
| Build chain | AutoVersion → BuildStamp → ZipRelease | ☐ |

## 🧾 8. Coverage Summary
| Layer | Coverage Goal | Status |
|-------|---------------|--------|
| Unit | 90% CLI logic coverage | ☐ |
| Integration | 100% pipeline tools covered | ☐ |
| Regression | Old versions consistent | ☐ |
| Manual | All critical paths tested | ☐ |
| CI/CD | Matrix builds green | ☐ |

## 🧠 9. Known Limitations (Lite)
- No interactive prompt mode.  
- No signed CHANGELOG hashes.  
- Config schema fixed (static).  

## 🧭 10. Acceptance Criteria (v1.1.2)
- [ ] CLI works end-to-end  
- [ ] CHANGELOG accurate  
- [ ] Config and env merge validated  
- [ ] Cross-tool integration green  
- [ ] CI/CD passes on all OS  
- [ ] Docs examples verified  

Once complete:  
```
git tag v1.1.2  
git push origin main --tags
```

MIT © 2025 Solcogito S.E.N.C.
