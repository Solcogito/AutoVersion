# 🧾 CHANGELOG — AutoVersion Lite

====================================================================
PURPOSE
====================================================================

This file lists all notable changes to the AutoVersion Lite project.
It is maintained automatically using the `autoversion changelog`
command, which parses Conventional Commit messages.

====================================================================
FORMAT
====================================================================

Each release entry follows the pattern:

## [X.Y.Z] – YYYY-MM-DD
### Added
### Changed
### Fixed
### Removed

AutoVersion automatically inserts new entries at the top of this file
when a release is created or published.

====================================================================
INITIAL ENTRIES
====================================================================

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

====================================================================
CHANGELOG GENERATION COMMAND
====================================================================

To update this file manually:
    dotnet run --project src/AutoVersion.Cli -- changelog

To simulate:
    dotnet run --project src/AutoVersion.Cli -- changelog --dry-run

====================================================================
END OF CHANGELOG
====================================================================
