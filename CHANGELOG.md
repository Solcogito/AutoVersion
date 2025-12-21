# Changelog

All notable changes to **AutoVersion Lite** will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---
## [1.3.0] – 2025-12-20
**Added**

- Explicit, testable CLI output abstraction (`ITextSink`) separating user-facing output from logging.
- Deterministic in-memory text sinks for unit and sandbox testing.
- Minimal command-list emission for invalid subcommand errors, matching sandbox and UNIX CLI conventions.

**Changed**

- Standard output (`stdout`) is now reserved exclusively for user-visible command results and help text.
- Standard error (`stderr`) is now reserved for diagnostics, errors, and dry-run informational messages.
- Dry-run mode no longer emits any stdout output; all dry-run messaging is diagnostic-only.
- Help routing behavior clarified:
  - `--help` and root invocation emit full help to stdout.
  - Invalid subcommands emit a minimal, ordered list of valid subcommands only.
- Command router updated to emit contextual help on errors instead of full command usage blocks.
- Logger configuration adjusted to prevent informational logs from leaking into stdout.

**Fixed**

- Multiple cases of stdout/stderr leakage detected by sandbox validation.
- Inconsistent help output between valid, invalid, and error paths.
- Non-deterministic CLI output affecting scriptability and CI usage.
- Edge cases where help text was silently suppressed due to logging configuration.

**Improved**

- Full compliance with the AutoVersion sandbox specification.
- Script-safe, composable CLI behavior suitable for CI pipelines and automation.
- Clear, stable CLI contract suitable for long-term support and future 2.x evolution.

**Notes**

- This release represents a CLI contract stabilization.
- No command names, flags, or versioning semantics were changed.
- This is the final minor release before the next major version.
---
## [1.2.6] – 2025-11-25
**Added**

- New recursive, hierarchical help formatter providing Git-style expanded help.

  - Root autoversion help now displays all nested commands (bump/major, bump/minor, bump/prerelease, etc.).

  - Each subcommand now includes its flags, options, and positional arguments.

  - Improved discoverability and UX for new users.

**Changed**

- HelpFormatter refactored to support structured indentation and multi-level command trees.

- Root-level CLI help is now more descriptive and consistent with standard CLI conventions.

**Improved**

- Help output is clearer, more readable, and easier to navigate.

- Documentation alignment with the new help behavior.


## [1.2.5] – 2025-11-25

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
