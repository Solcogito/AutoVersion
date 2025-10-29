# 🧾 CHANGELOG — AutoVersion Lite

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
