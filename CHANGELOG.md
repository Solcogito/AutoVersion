## [1.1.3] - 2025-11-06

## [1.1.2] - 2025-11-03

### Documentation

- Official release README adjustment

## [1.1.1] - 2025-11-01

### Other

- Official release setup on GitHub: AutoVersion_Lite_v1.1.1.zip

## [1.1.0] - 2025-11-01

### Documentation
- reorganize documentation and update style/config files (098f9dd)
- Added core versioning logic (`VersionModel`, `VersionManager`)
- Added CLI entry point (`autoversion bump`, `autoversion current`)
- Added .NET + Unity integration foundation
- Added docs, style, and contributor guides
- Cleaned repository and finalized GitHub structure

### Maintenance
- Update docs and ignore rules; cleaned Media and GumroadPackage (d09afb7)

### Other
- add GitHub Actions workflow (818b08f)

## [1.0.0] - 2025-11-01
### Added
- **git**: implement automatic tag creation and push integration for v0.5.0 (d5e562f)
- **artifacts**: implement artifact handling and rename logic for v0.4.0 (415f833)

### Documentation
- reorganize documentation and update style/config files (098f9dd)
- **release**: add full v0.7.0 documentation and samples (9f2cc38)
- **changelog**: update for v0.3.0 ΓÇô Changelog Engine (df01d67)

### Maintenance
- Update docs and ignore rules; cleaned Media and GumroadPackage (d09afb7)
- **release**: prepare v1.0.0 public lite release (58522e9)
- **release**: bump version to 0.9.1 (7e829b3)
- **release**: bump to v0.6.1 ΓÇö Unity Editor Menu integration (5bfeb43)
- **release**: bump version to 0.5.1 (051e826)
- **release**: Changelog.md (bf0472f)
- **release**: v0.3.0 ΓÇô Changelog Engine (cb3ae2f)

### Other
- add GitHub Actions workflow (818b08f)
- implement v0.8.0 cross-platform workflows and commitlint (f3223ff)
- **git**: stabilize GitService_CreatesTagSuccessfully on Windows (3971836)

## [0.2.0] - 2025-11-01
### Added
- **git**: implement automatic tag creation and push integration for v0.5.0 (d5e562f)
- **artifacts**: implement artifact handling and rename logic for v0.4.0 (415f833)

### Documentation
- reorganize documentation and update style/config files (098f9dd)
- **release**: add full v0.7.0 documentation and samples (9f2cc38)
- **changelog**: update for v0.3.0 ΓÇô Changelog Engine (df01d67)

### Maintenance
- Update docs and ignore rules; cleaned Media and GumroadPackage (d09afb7)
- **release**: prepare v1.0.0 public lite release (58522e9)
- **release**: bump version to 0.9.1 (7e829b3)
- **release**: bump to v0.6.1 ΓÇö Unity Editor Menu integration (5bfeb43)
- **release**: bump version to 0.5.1 (051e826)
- **release**: Changelog.md (bf0472f)
- **release**: v0.3.0 ΓÇô Changelog Engine (cb3ae2f)
- **release**: v0.2.0 ΓÇô Config & File Ops (35d0d1e)

### Other
- add GitHub Actions workflow (818b08f)
- implement v0.8.0 cross-platform workflows and commitlint (f3223ff)
- **git**: stabilize GitService_CreatesTagSuccessfully on Windows (3971836)

## [1.0.0] – 2025-10-30

### 🚀 Public Lite Release (Codename: Emergent Horizon)
- Public GitHub and Gumroad release
- Added showcase video and documentation polish
- All core features stable and verified:
  - Structured JSON output for CI/CD
  - Rollback safety
  - Dry-run and EOL preservation
- Verified UTF-8/BOM consistency across platforms
- Ready for public distribution under MIT License

## [0.9.2] - 2025-10-30

### 🎉 JSON Verification & Final Stability

- JSON output validated via jq (UTF-8 encoding confirmed)
- Aliases corrected and CLI verified across Windows PowerShell 5.x
- Build + rollback system fully stable

**Status:** ✅ Ready for 1.0.0 documentation & release polish

## [0.9.1] – 2025-11-06

### 🧩 Structured Output & Rollback Safety

#### Added
- Full JSON output architecture (`--json`) for CLI and CI pipelines  
- `VersionResult` model for structured version bump results  
- `ChangelogCommand.RunJson()` for CI changelog export  
- Automatic rollback and `.bak` file backup on version write  
- UTF-8 BOM and EOL preservation in `version.txt`  
- Color suppression when output is redirected (CI-safe)

#### Changed
- `VersionManager.Bump()` now returns `VersionResult`  
- `CommandRouter` supports `--json` across `current`, `bump`, and `changelog`  
- CLI fully compatible with `jq` and GitHub Actions

#### Fixed
- Compilation issues from older signatures resolved  
- Improved resilience under file lock or encoding mismatch

---

**Codename:** *Structured Horizon*  
**Status:** ✅ JSON-ready for CI integration and v1.0.0 release prep

## [0.9.0] – 2025-11-05

### 🧪 Polish & Release Prep

#### Added
- Colorized CLI output for success, warning, and error messages  
- Automatic schema validation during `bump` and `changelog` commands  
- `--json` output mode for CI/CD pipelines  
- Auto-discovery of nearest `autoversion.json` (monorepo-friendly)  
- Backup system for versioned files before modification  
- Automatic rollback on failure  
- Enhanced `autoversion help` with grouped command categories

#### Changed
- Improved dry-run output formatting and color emphasis  
- Detected and preserved original file encodings (UTF8, BOM, CRLF/LF)  
- More verbose validation messages for malformed configs  
- Clearer success/failure log flow (now shows `[OK]`, `[WARN]`, `[ERROR]`)

#### Fixed
- Resolved partial-write issue when multiple targets failed  
- Fixed rare case where empty changelog caused tag skip  
- Fixed inconsistent `--allow-dirty` flag handling across commands  

---

**Goal:**  
Deliver a *bulletproof*, production-ready CLI and Unity integration, stable under all workflows.

**Codename:** *“Mirror Finish”*  
**Status:** ✅ Feature Complete – Ready for v1.0.0 Release Prep

## [0.8.0] – 2025-11-01

### ⚙️ CI + Quality Gates

#### Added
- Cross-platform GitHub Actions matrix:
  - ✅ Windows, macOS, Ubuntu
- `lint.yml` workflow for schema + docs validation
- `commitlint` integration enforcing Conventional Commits
- `release-on-tag.yml` for automated publishing
- CI job caching + artifact upload optimization
- Extended `/docs/WORKFLOWS.md` with new CI examples

#### Changed
- Build scripts now trigger tests automatically on CI
- Updated README badges for CI, Lint, and Release
- Synchronized `.editorconfig`, `.gitattributes`, and `.gitignore` for platform parity

#### Fixed
- Minor formatting inconsistencies in docs and schema
- Unified line endings and encoding across all `.ps1` and `.sh` scripts

---

**Goal:** Guarantee consistent builds and enforce clean commit discipline across all operating systems.  
**Codename:** *“Automated Trust”*

## [0.7.0] – 2025-10-30

### 📚 Documentation & Samples

#### Added
- Complete `/docs/` suite:
  - `README.md` (documentation index & navigation)
  - `QUICKSTART.md` (CLI + Unity setup guide)
  - `CONFIG.md` (full schema reference)
  - `WORKFLOWS.md` (CI/CD workflow integration)
  - `UNITY.md` (Editor integration manual)
  - `GUMROAD.md` (Pro licensing preview)
  - `TEMPLATES.md` (release note & promo templates)
  - `FAQ.md` (expanded troubleshooting)
- Added `_assets/demo.gif` and `schema-diagram.png` placeholders
- Added `/samples/Sample.UnityProject` for Unity demo

#### Changed
- Cross-linked all docs and unified emoji header style
- Refined root `README.md` with consistent branding and roadmap
- Finalized architecture and component documentation (`ARCHITECTURE.md`, `TECH_OVERVIEW.md`, `COMPONENT_GUIDE.md`)

#### Fixed
- Corrected misnamed `WORKFLOWS.md` (was CHANGELOG.md)
- Synchronized internal references across all `.md` files

---

### Summary
This milestone delivers the **complete public documentation and sample suite**, establishing AutoVersion Lite as a fully documented, CI-ready, Unity-compatible release automation tool.

---

**Commit hash template:** `v0.7.0-docs`  
**Maintainer:** Benoit Desrosiers — Solcogito S.E.N.C.  
**Release Codename:** *“Reference Point”*

## [0.6.1] - 2025-10-30
### Added
- Full Unity Editor integration (AutoVersionMenu.cs)
- Unity demo scene (AutoVersionMenuDemo.unity)
- Editor tests for menu + version validation
- Verified CLI and Core build flow with Unity Editor

### Fixed
- Changelog overwriting behavior replaced with safe prepend logic
- Artifact rename logic with proper backups and dry-run support

### Internal
- 13/13 tests passing in .NET pipeline
- Release packaging verified (Core + CLI + Unity)
---

## [0.6.0] - 2025-10-30
### Added
- Initial Unity Editor bridge and CLI hooks
- Added Unity project demo structure and version management
- Early changelog auto-generation integrated with Unity
- Artifact rename handling system (v0.4.0 feature merged forward)

## [0.5.0] - 2025-10-30
### Added
- **git**: implemented automatic tag creation and push integration
- **bump**: added repo clean check with `--allow-dirty` override
- **core**: introduced `GitService` utility (createTag, pushTag, isClean)
- **pipeline**: integrated tagging after changelog + artifact steps

### Fixed
- **bump**: corrected `AllowDirty` boolean handling (removed null-coalescing misuse)

### Documentation
- **readme**: added Git config examples and dry-run usage notes
- **changelog**: verified formatting and section ordering

### Maintenance
- **tests**: added full coverage for Git tagging and pipeline integration
- **ci**: validated on Windows/macOS/Linux matrix build

## [0.4.0] - 2025-10-29
### Added
- **artifacts**: implement artifact handling and rename logic for v0.4.0 (415f833)

### Documentation
- **changelog**: update for v0.3.0 ΓÇô Changelog Engine (df01d67)
- **impl**: add v0.1.0 SemVer Core implementation plan (a141441)
- finalize bootstrap phase and changelog setup (2013459)
- **impl**: add v0.1.0 SemVer Core implementation plan (cd819eb)
- **design**: add tech overview, architecture, and component guides (ab8f666)
- add CONFIG, FAQ, UNITY, and TEMPLATES documentation (2b0da4d)

### Maintenance
- **release**: v0.3.0 ΓÇô Changelog Engine (cb3ae2f)
- **release**: v0.2.0 ΓÇô Config & File Ops (35d0d1e)
- **release**: v0.1.0 ΓÇô SemVer Core (e8eb35e)
- **release**: stabilize core + tests, fully passing build pipeline (v0.0.3) (1c7bd81)
- **scripts**: update publish.ps1 (bde7d67)
- **build**: add AutoVersion.sln and update scripts (a414789)
- **init**: bootstrap AutoVersion v0.0.0 (39518d7)

## [0.3.0] – 2025-10-29
### Documentation
- **impl**: add v0.1.0 SemVer Core implementation plan (a141441)
- finalize bootstrap phase and changelog setup (2013459)
- **impl**: add v0.1.0 SemVer Core implementation plan (cd819eb)
- **design**: add tech overview, architecture, and component guides (ab8f666)
- add CONFIG, FAQ, UNITY, and TEMPLATES documentation (2b0da4d)

### Maintenance
- **release**: v0.2.0 – Config & File Ops (35d0d1e)
- **release**: v0.1.0 – SemVer Core (e8eb35e)
- **release**: stabilize core + tests, fully passing build pipeline (v0.0.3) (1c7bd81)
- **scripts**: update publish.ps1 (bde7d67)
- **build**: add AutoVersion.sln and update scripts (a414789)
- **init**: bootstrap AutoVersion v0.0.0 (39518d7)

---

AutoVersion 0.3.0 introduces the **Changelog Engine**,  
enabling automatic markdown generation from Conventional Commits.  
This version completes the foundation of the AutoVersion Lite core.

## [Unreleased] – 2025-10-29

### Docs
- **impl**: add v0.1.0 SemVer Core implementation plan (a141441)
- finalize bootstrap phase and changelog setup (2013459)
- **impl**: add v0.1.0 SemVer Core implementation plan (cd819eb)
- **design**: add tech overview, architecture, and component guides (ab8f666)
- add CONFIG, FAQ, UNITY, and TEMPLATES documentation (2b0da4d)

### Chore
- **release**: v0.2.0 ΓÇô Config & File Ops (35d0d1e)
- **release**: v0.1.0 ΓÇô SemVer Core (e8eb35e)
- **release**: stabilize core + tests, fully passing build pipeline (v0.0.3) (1c7bd81)
- **scripts**: update publish.ps1 (bde7d67)
- **build**: add AutoVersion.sln and update scripts (a414789)
- **init**: bootstrap AutoVersion v0.0.0 (39518d7)

# 🧾 CHANGELOG — AutoVersion Lite

## [0.2.0] – 2025-10-29
### Added
- `autoversion.json` configuration system
- Schema validation via `autoversion.schema.json`
- Multi-file update support (JSON, XML, Regex, Text)
- Dry-run simulation and diff output
- Backup and rollback protection
- `--config` flag for custom configuration paths

### Changed
- Refactored version handling to strict nullable model
- Updated `SemVerParserTests` for safe null assertions
- Unified project nullability rules (`<Nullable>enable</Nullable>`)

### Fixed
- Removed nullability warnings in core models
- Test warnings eliminated for `SemVerParserTests`

## [0.1.0] – 2025-10-29
### Added
- Semantic Versioning Core (`VersionModel`, `SemVerParser`, `VersionFile`)
- CLI commands (`current`, `bump`) with dry-run support
- PowerShell build/test/publish automation (`_Infrastructure/`)
- Full xUnit test suite validating bump logic and parsing
- Rich documentation headers, consistent namespace standards
- Nullability handling baseline (`Directory.Build.props`)

### Changed
- Unified repository under `src/AutoVersion.*` namespace
- Build pipeline upgraded for release artifacts
- CLI refactored for stability and cross-module access
- Logger enhanced with color-coded output and dry-run context
- Version bump logic aligned with deterministic SemVer spec

### Fixed
- Access-level issue preventing test access to `BumpCommand`
- Early nullability mismatches during build
- Namespace and visibility consistency between Core and CLI modules

---

## ⚙️ v0.1.0 — SemVer Core

**Objective**  
Implement semantic version parsing, validation, and bump logic.

**Features**
- Parse `major.minor.patch[-prerelease][+build]`  
- Compare and increment versions  
- CLI commands:  
	- autoversion current
	- autoversion bump [major|minor|patch|prerelease] [--pre alpha.1]
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

## [0.0.1] – 2025-10-29
### Added
- Repository scaffolding
- CI/CD + lint + docs
- Core folder structure

---

This file lists all notable changes to the AutoVersion Lite project.  
It is maintained automatically using the `autoversion changelog` command,  
which parses Conventional Commit messages.

---

## 🧩 Format

Each release entry follows this structure:

## [X.Y.Z] – YYYY-MM-DD

### Added

### Changed

### Fixed

### Removed

AutoVersion automatically inserts new entries at the top of this file  
when a release is created or published.

---

## [Unreleased]

### Added
- Initial repository structure
- CI + linting workflows
- Documentation and schema files
- Build and publish scripts

### Changed
- Bootstrap pipeline finalized

### Fixed
- N/A

---

## 🧪 Commands

Update this file manually:
    dotnet run --project src/AutoVersion.Cli -- changelog

Simulate generation:
    dotnet run --project src/AutoVersion.Cli -- changelog --dry-run

---

**End of Changelog**

