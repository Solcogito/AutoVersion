# 🧾 CHANGELOG — AutoVersion Lite

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
