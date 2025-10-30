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

