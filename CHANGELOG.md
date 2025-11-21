# Changelog

All notable changes to **AutoVersion Lite** will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.2.1] – 2025-11-21

### Added
- Comprehensive unit tests for all CLI commands (`current`, `set`, `bump`).
- Test utilities: `ConsoleCapture` and `FakeCliLogger` for safe, deterministic CLI testing.
- Consistent file headers and documentation comments across Core, CLI, and Tests.
- New documentation set focused on Lite:
  - `README_LITE_1.1.x.md`
  - Quickstart, FAQ, Architecture, and Component guides.
- Clean `AUTOVERSION_LITE.md` CLI reference.

### Changed
- Refactored `CommandRouter` to rely fully on ArgForge (no more `System.CommandLine`).
- Improved error handling and exit codes across all commands.
- Simplified version resolution to always pick the highest valid version from
  `version.json` and `version.txt`.
- Cleaned `_Infrastructure` folder by removing unused CI, schema, and commitlint files.
- Updated `Directory.Build.props` to align with AutoVersion Lite’s real usage.

### Fixed
- Correct handling of missing or invalid version arguments in `SetCommand` and `BumpCommand`.
- Prevented `ObjectDisposedException` when capturing console output during tests.
- Resolved edge cases where version file path could be empty or unresolved.
- Ensured dry-run mode never writes to disk while still logging intended changes.

### Removed
- All configuration-based behavior (`autoversion.json` and related schema).
- Legacy Git integration, changelog automation, and template systems.
- Unity integration and artifact renaming-related code and documentation.
- Old CI workflows, commitlint configuration, and unused launch settings.

---

## [1.2.0] – 2025-11-20

### Added
- Initial ArgForge-based CLI commands.
- `autoversion current`, `autoversion set`, and `autoversion bump` basic support.

### Changed
- Migrated core versioning logic into `AutoVersion.Core`.
- Introduced `IVersionEnvironment` and `ICliLogger` abstractions.

### Fixed
- Early bug fixes related to version parsing and file handling.

---

## [1.1.0] – 2025-11-15

### Added
- Basic semantic version parsing and formatting.
- Support for `version.txt` as the primary version source.
- First implementation of `autoversion set` and `autoversion bump`.
- Minimal documentation and usage examples.

---

## Legacy History

For older, non-Lite behavior (Unity editor menus, configuration files, Git tagging,
multi-file propagation, changelog generation, templates, CI workflows, artifact
renaming and publish pipelines), see:

- **`CHANGELOG_LEGACY.md`**

Those entries describe an earlier program in the Solcogito suite and do not apply
to **AutoVersion Lite**.
