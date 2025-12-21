// ============================================================================
// File:        AutoVersionErrors.cs
// Project:     AutoVersion Lite
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Central error identifiers for AutoVersion (CLI + Core boundary).
//   These are ErrorIdentifier constants used to construct ErrorInfo.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Solcogito.Common.Errors;

namespace Solcogito.AutoVersion.Errors
{
    public static class AutoVersionErrors
    {
        // --------------------------------------------------------------------
        // CLI arguments
        // --------------------------------------------------------------------
        public static readonly ErrorIdentifier MissingCommand =
            new(ErrorCategory.Argument, new ErrorInstitution("AutoVersion.Cli"), "MISSING_COMMAND");

        public static readonly ErrorIdentifier UnknownCommand =
            new(ErrorCategory.Argument, new ErrorInstitution("AutoVersion.Cli"), "UNKNOWN_COMMAND");

        public static readonly ErrorIdentifier InvalidPath =
            new(ErrorCategory.Argument, new ErrorInstitution("AutoVersion.Cli"), "INVALID_PATH");

        public static readonly ErrorIdentifier MissingPath =
            new(ErrorCategory.Argument, new ErrorInstitution("AutoVersion.Cli"), "MISSING_PATH");

        public static readonly ErrorIdentifier MissingVersion =
            new(ErrorCategory.Argument, new ErrorInstitution("AutoVersion.Cli"), "MISSING_VERSION");

        public static readonly ErrorIdentifier InvalidVersion =
            new(ErrorCategory.Versioning, new ErrorInstitution("AutoVersion.Cli"), "INVALID_VERSION");

        public static readonly ErrorIdentifier InvalidPrerelease =
            new(ErrorCategory.Argument, new ErrorInstitution("AutoVersion.Cli"), "INVALID_PRERELEASE");

        public static readonly ErrorIdentifier UnknownBumpType =
            new(ErrorCategory.Argument, new ErrorInstitution("AutoVersion.Cli"), "UNKNOWN_BUMP_TYPE");

        // --------------------------------------------------------------------
        // Resolution / environment
        // --------------------------------------------------------------------
        public static readonly ErrorIdentifier ResolveFailed =
            new(ErrorCategory.Versioning, new ErrorInstitution("AutoVersion.Core"), "RESOLVE_FAILED");

        public static readonly ErrorIdentifier NoFinalVersion =
            new(ErrorCategory.Versioning, new ErrorInstitution("AutoVersion.Core"), "NO_FINAL_VERSION");

        public static readonly ErrorIdentifier WriteFailed =
            new(ErrorCategory.Versioning, new ErrorInstitution("AutoVersion.Core"), "WRITE_FAILED");

        // --------------------------------------------------------------------
        // Unexpected boundary failures
        // --------------------------------------------------------------------
        public static readonly ErrorIdentifier CliFailure =
            new(ErrorCategory.System, new ErrorInstitution("AutoVersion.Cli"), "CLI_FAILURE");
    }
}
