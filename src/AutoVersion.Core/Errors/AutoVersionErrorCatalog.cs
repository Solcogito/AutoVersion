// ============================================================================
// File:        AutoVersionErrorCatalog.cs
// Project:     AutoVersion Lite
// ============================================================================

using Solcogito.Common.Errors;

namespace Solcogito.AutoVersion.Errors
{
    public sealed class AutoVersionErrorCatalog : IErrorCatalog
    {
        public IEnumerable<ErrorDescriptor> GetDescriptors()
        {
            yield return new ErrorDescriptor(
                new ErrorIdentifier(
                    ErrorCategory.Versioning,
                    new ErrorInstitution("AutoVersion"),
                    "NO_CURRENT_VERSION"),
                ErrorSeverity.Error,
                title: "No current version",
                messageTemplate: "No existing version could be resolved."
            );

            yield return new ErrorDescriptor(
                new ErrorIdentifier(
                    ErrorCategory.Versioning,
                    new ErrorInstitution("AutoVersion"),
                    "INVALID_PRERELEASE"),
                ErrorSeverity.Error,
                title: "Invalid prerelease",
                messageTemplate: "Prerelease identifier '{value}' is invalid."
            );

            yield return new ErrorDescriptor(
                new ErrorIdentifier(
                    ErrorCategory.System,
                    new ErrorInstitution("AutoVersion"),
                    "UNEXPECTED_EXCEPTION"),
                ErrorSeverity.Critical,
                title: "Unexpected failure",
                messageTemplate: "An unexpected error occurred."
            );
        }
    }
}
