# ✅ RELEASE_CHECKLIST.md — AutoVersion v1.1.3 Verification

##### Objective  
Guarantee that AutoVersion v1.1.3 (Lite) is stable, documented, and synchronized with BuildStamp for public use.

## 1️⃣ Project Integrity
| Task | Check | Status |
|------|--------|--------|
| dotnet build succeeds | Core and CLI compile | ✅ |
| Config and docs exist | autoversion.json present | ✅ | <!-- TODO: confirm if the files itself is really ok (is it done later)  -->
| MIT License updated | LICENSE| ✅ |
| CHANGELOG accurate | last entry = v1.1.3 | ✅ |
| Version tag confirmed | `autoversion current` → v1.1.3 | ✅ |

## 2️⃣ CLI Validation
| Command | Expected Behavior | Status |
|----------|------------------|--------|
| autoversion --help | Shows flags and usage | ✅ |
| autoversion bump patch | Increments build number | ✅ |
| --dry-run | No write, prints preview | ✅ |
| --allow-dirty | Runs with uncommitted files | ✅ |
| Invalid arg | Exit code = 1 | ✅ |

## 3️⃣ Configuration & Environment
| Task | Expected | Status |
|------|-----------|--------|
| autoversion.json loads | All fields read | ✅ |
| Env overrides | AUTOVERSION_VERSION applied | ☐ | <-- Github enhancement -->
| CLI overrides config | Flag takes priority | ✅ |
| Missing config | Graceful error + defaults | ✅ |

## 4️⃣ Cross-Tool Integration
| Tool | Integration Test | Status |
|------|------------------|--------|
| BuildStamp | buildstamp uses new version | ☐ | <-- no available yet -->
| ZipRelease | archive includes CHANGELOG + metadata | ☐ | <-- no available yet -->
| CompleteRelease | AutoVersion → BuildStamp → ZipRelease chain | ☐ | <-- no available yet -->

## 5️⃣ Cross-Platform QA
| OS | Shell | Result | Status |
|----|--------|--------|--------|
| Windows 11 | PowerShell 7 | CLI runs OK | ✅ |
| Ubuntu 22.04 | bash | CLI runs OK | ☐ | <-- no available yet -->
| macOS Sonoma | zsh | CLI runs OK | ☐ | <-- no available yet -->
| GitHub Actions | Ubuntu-latest | Workflow passes | ☐ | <-- no available yet -->

## 6️⃣ Documentation Review
| File | Verified | Status |
|------|-----------|--------|
| README.md | Overview + usage | ✅ |
| AUTOVERSION.md | Full CLI reference | ✅ |
| CHANGELOG.md | Matches version history | ✅ |
| WORKFLOWS.md | CI examples accurate | ✅ |
| FAQ.md | Top issues covered | ✅ |
| ROADMAP.md | Up to v1.2.0 outline | ✅ |

## 7️⃣ Packaging & Artifacts
| Task | Check | Status |
|------|--------|--------|
| Builds/ clean | Before packaging | ✅ |
| CLI binary produced | autoversion.exe | ✅ |
| ziprelease includes binaries | ✅ |
| SHA256 hash recorded | Integrity verified | ☐ | <-- not available yet -->
| Archive opens cleanly | Manual test | ✅ |

## 8️⃣ Tagging & Deployment
| Task | Check | Status |
|------|--------|--------|
| Commit all changes | `git add . && git commit -m "Release v1.1.2"` | ☐ |
| Tag release | `git tag v1.1.2` | ☐ |
| Push tags | `git push origin main --tags` | ☐ |
| Workflow build | ✅ on GitHub Actions | ☐ |

## 9️⃣ Post-Release Validation
| Task | Expected | Status |
|------|-----------|--------|
| dotnet tool install --global Solcogito.AutoVersion | Works globally | ☐ |
| Run CLI after install | autoversion --help outputs | ☐ |
| Cross-tool chain retest | Full pipeline green | ☐ |
| Add badge to README | ![v1.1.2] | ☐ |

## 🔟 Final Verification

- [ ] All CLI commands validated  
- [ ] BuildStamp integration verified  
- [ ] CI/CD workflow green  
- [ ] Docs reviewed and synced  
- [ ] Release archive tested  
- [ ] GitHub release published  

Once complete, tag and publish 🎉

MIT (Lite) © 2025 Solcogito S.E.N.C.
