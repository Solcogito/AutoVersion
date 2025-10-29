# 🧭 AutoVersion Lite — Development Roadmap
Project Goal: Create a free, open-source semantic versioning & changelog automation tool for Unity and general development pipelines.

---

## 🧱 Overview

| Version | Codename | Focus |
|----------|-----------|-------|
| v0.0.0  | Bootstrap | Environment, repo setup, scaffolding |
| v0.1.0  | SemVer Core | Version parsing, increment logic |
| v0.2.0  | Config & File Ops | JSON schema, file replacement |
| v0.3.0  | Changelog Engine | Conventional Commit parsing |
| v0.4.0  | Artifact Handling | Versioned file renaming |
| v0.5.0  | Git Integration | Tag, push, safety checks |
| v0.6.0  | Unity Editor Menu | In-editor automation |
| v0.7.0  | Documentation | Full docs & examples |
| v0.8.0  | CI + Quality Gates | Cross-platform pipeline |
| v0.9.0  | Polish & Release Prep | Stability, UX, edge cases |
| v1.0.0  | Public Lite Release | Marketing & distribution |

---

## 🧩 v0.0.0 — Bootstrap

**Objective**  
Lay the groundwork for clean, modular, cross-platform development and CI/CD.

**Tasks**
- Initialize Git repository  
- Add `.gitignore` for Unity + .NET  
- Create directory structure:  
  ```
  /src/
    AutoVersion.Core/
    AutoVersion.Cli/
    AutoVersion.Unity/Editor/
  /docs/
  /samples/
  /.github/workflows/
  ```
- Add LICENSE (MIT)  
- Create empty CHANGELOG.md  
- Add `.editorconfig` + `.gitattributes`  
- Implement minimal CLI shell (`autoversion --help`)  
- Configure CI workflow (`ci.yml`) with multi-OS matrix (build only)  
- Add basic unit test setup using xUnit  
- Ensure build + tests run on Windows, macOS, Linux  

**Deliverables**
- Clean repo  
- Build & test pass  
- CI green on all OSes  
- README placeholder with project summary  

**Acceptance Criteria**
- Running `dotnet run --project AutoVersion.Cli` prints version & help text  
- GitHub Actions reports ✅ on all builds  

---

## ⚙️ v0.1.0 — SemVer Core

**Objective**  
Implement semantic version parsing, validation, and bump logic.

**Features**
- Parse `major.minor.patch[-prerelease][+build]`  
- Compare and increment versions  
- CLI commands:  
  ```
  autoversion current
  autoversion bump [major|minor|patch|prerelease] [--pre alpha.1]
  ```
- Dry-run mode  
- Display new version before writing  

**Tasks**
- Create `VersionModel` class with parse & `ToString()`  
- Add tests for all bump scenarios  
- Add CLI parser (System.CommandLine)  
- Implement global `--dry-run` flag  

**Deliverables**
- `autoversion bump patch` increments local version  
- CLI feedback: old → new version  
- All unit tests passing  

**Acceptance**
- Deterministic SemVer math  
- Handles malformed input gracefully  

---

## 🧰 v0.2.0 — Config & File Ops

**Objective**  
Enable version updates across multiple file types via JSON config.

**Features**
- `autoversion.json` configuration file  
- Update version in:
  - `package.json` (JSON)  
  - `Directory.Build.props` (XML)  
  - `AssemblyInfo.cs` (Regex)  
- Token replacement for arbitrary text files  
- Backup & rollback safety  
- Config validation with schema  

**Tasks**
- Define `autoversion.schema.json`  
- Implement file-type handlers: JSON, XML, Text  
- Add error handling and logging  
- Add `--config` flag for custom config path  
- Write examples to `/samples/`  

**Deliverables**
- `autoversion bump patch` updates all listed files  
- Dry-run shows diff summary  
- Schema validation with helpful errors  

**Acceptance**
- Tested on sample JSON/XML/text files  
- Preserves original formatting & line endings  

---

## 🧾 v0.3.0 — Changelog Engine

**Objective**  
Auto-generate `CHANGELOG.md` from Conventional Commits.

**Features**
- Parse commits since last tag  
- Group by section (feat, fix, perf, docs, chore)  
- Write formatted markdown changelog  
- Support `--since-tag`, `--unreleased`, and `--preview`  
- Configurable section mapping  

**Tasks**
- Implement Git commit reader  
- Implement Conventional Commit parser  
- Add changelog section in config  
- Add markdown writer  
- Add dry-run preview  

**Deliverables**
- `CHANGELOG.md` updates correctly  
- Dry-run prints changelog preview  
- Parser accuracy ≥ 90%  

**Acceptance**
- Real commits (feat/fix) correctly categorized  
- Valid markdown produced  

---

## 📦 v0.4.0 — Artifact Handling

**Objective**  
Enable artifact renaming with version suffixes.

**Features**
- Rename files using `{version}` placeholder  
- Config example:
  ```
  "artifacts": [
    { "path": "Builds/Product.unitypackage", "rename": "Product_{version}.unitypackage" }
  ]
  ```
- Optional overwrite flag  
- Skip non-existent files gracefully  

**Tasks**
- Implement rename logic  
- Add `--force` flag  
- Integrate with changelog bump flow  
- Add tests for Windows/Mac/Linux paths  

**Deliverables**
- Files renamed on bump  
- Dry-run preview of rename operations  
- Configurable naming pattern  

**Acceptance**
- Safe rename; existing files backed up  
- Logs clear before → after mapping  

---

## 🧩 v0.5.0 — Git Integration

**Objective**  
Create & push tags automatically, ensuring repo safety.

**Features**
- Preflight clean repo check  
- Annotated tag creation  
- Auto push to remote  
- Config:  
  ```
  "git": { "tagPrefix": "v", "push": true }
  ```
- Friendly errors for tag conflicts or dirty state  

**Tasks**
- Implement Git wrapper  
- Add `--allow-dirty` override  
- Integrate tag creation after successful bump  
- Tests: detect dirty repo, duplicate tags  

**Deliverables**
- `git tag vX.Y.Z` created automatically  
- Tag push optional via config  
- CLI output confirms push result  

**Acceptance**
- Works on real repo (with remotes)  
- Tag message pulled from changelog top section  

---

## 🎮 v0.6.0 — Unity Editor Menu

**Objective**  
Expose AutoVersion Lite features inside Unity Editor.

**Features**
- Menu entries under `Tools/AutoVersion/`
  - Current Version  
  - Bump Major/Minor/Patch/Prerelease  
  - Generate Changelog  
- Progress bar + log feedback  
- Reads `autoversion.json` from project root  

**Tasks**
- Create `Editor/AutoVersionMenu.cs`  
- Link to Core library (no runtime dependencies)  
- Add asset sample with working config  
- Editor testing on Unity 2022.3 LTS  

**Deliverables**
- Fully functional Editor menu  
- Matches CLI behavior  
- Usability verified with demo scene  

**Acceptance**
- Editor menus appear correctly  
- Commands update files & changelog  

---

## 📚 v0.7.0 — Documentation & Samples

**Objective**  
Deliver all docs & learning assets.

**Deliverables**
- `README.md`: one-minute setup + demo GIF  
- `QUICKSTART.md`: CLI + Unity setup  
- `CONFIG.md`: full schema reference  
- `WORKFLOWS.md`: GitHub Actions examples  
- `UNITY.md`: Editor usage  
- `GUMROAD.md`: Pro preview  
- `TEMPLATES.md`: changelog/social templates  
- `FAQ.md`: 10+ common issues  
- `CHANGELOG.md`: auto-managed  
- `LICENSE`: MIT  

**Tasks**
- Write all docs  
- Create `/samples/Sample.UnityProject`  
- Add schema validation example  
- Record short demo GIF  

**Acceptance**
- Docs cover install → use → CI  
- Internal links verified  
- Quickstart works on clean machine  

---

## ⚙️ v0.8.0 — CI + Quality Gates

**Objective**  
Guarantee cross-platform consistency and code quality.

**Features**
- CI Matrix: Windows, macOS, Ubuntu  
- Lint, build, and test jobs  
- commitlint integration  
- release-on-tag workflow example  
- Optional dry-run job for PRs  

**Tasks**
- Extend `ci.yml`  
- Add `.commitlintrc.json`  
- Create workflows:
  - `ci.yml`
  - `release-on-tag.yml`
- Document CI in `/docs/WORKFLOWS.md`  

**Deliverables**
- Green CI on all OSes  
- Tag triggers release workflow  
- Conventional commits validated  

**Acceptance**
- PR → all checks green  
- Bad commit messages rejected  

---

## 🧪 v0.9.0 — Polish & Release Prep

**Objective**  
Finalize Lite package; handle UX, errors, and edge cases.

**Features**
- Graceful failure messages  
- Preserve file encodings (UTF8/BOM)  
- Config discovery (auto-find nearest)  
- Colorized CLI output  
- `--json` output for pipelines  
- Backups and rollback  

**Tasks**
- Implement colored console feedback  
- Detect and preserve EOL formats  
- Validate schema automatically  
- Add full CLI help  

**Deliverables**
- Stable CLI  
- Friendly output (green success, red error)  
- Dry-run safety verified  

**Acceptance**
- Stable on real Unity + C# repos  
- No partial changes  

---

## 🚀 v1.0.0 — Public Lite Release

**Objective**  
Ship to GitHub & Gumroad as a free asset.

**Tasks**
- Finalize `CHANGELOG.md` and tag v1.0.0  
- Record showcase video (30–60s)  
- Publish GitHub Release  
- Publish Gumroad Lite version (free)  
- Tweet + Reddit + Discord announcement  

**Deliverables**
- Public repo: https://github.com/solcogito/AutoVersion  
- Gumroad page: “AutoVersion Lite — Free Version Automation Tool”  
- Docs, samples, media included  

---

## 🔮 Future (v1.1.0 → v2.0.0 Pro Preview)

- GitHub Releases + Asset uploads  
- Gumroad API integration  
- Webhooks (Discord, Slack, Twitter)  
- Multi-project presets  
- Prerelease channels  
- Unity `.unitypackage` exporter  
- Full EditorWindow “Release” GUI  

---

## ✅ Quality Checklist (for every milestone)

- [ ] Code builds cleanly on all OSes  
- [ ] Unit tests ≥ 90% coverage for new features  
- [ ] CHANGELOG updated via AutoVersion itself  
- [ ] CI green  
- [ ] Docs updated in same commit  
- [ ] Manual test on Unity project  
- [ ] Version tag created & verified  

---

## 🧾 Credits

© 2025 Solcogito S.E.N.C.  
Project Lead: Benoit Desrosiers  
License: MIT (Lite)
