// ============================================================================
// File:        DefaultVersionEnvironment.cs
// Project:     AutoVersion Lite
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Default environment implementation:
//   - Reads via VersionResolver
//   - Writes via Common.IOKit.SafeFile (deterministic safe write)
//   - Does NOT infer a path from VersionResolveResult (it has none by design)
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;

using Solcogito.Common.Errors;
using Solcogito.Common.IOKit;
using Solcogito.Common.LogScribe;
using Solcogito.Common.Versioning;

namespace Solcogito.AutoVersion.Core
{
    /// <summary>
    /// Filesystem-backed implementation of <see cref="IVersionEnvironment"/> used by the CLI.
    /// </summary>
    /// <remarks>
    /// Responsibilities:
    /// - Provide a default discovery surface (configured default sources filtered by existence)
    /// - Resolve version candidates using <see cref="VersionResolver"/>
    /// - Persist versions safely using <see cref="SafeFile"/>
    ///
    /// Non-responsibilities:
    /// - CLI parsing
    /// - Help formatting
    /// - Command routing
    /// </remarks>
    public sealed class DefaultVersionEnvironment : IVersionEnvironment
    {
        private readonly IReadOnlyList<FileVersionSource> _configuredDefaultSources;
        private readonly Logger _logger;
        private readonly bool _allowNormalize;
        private readonly string _context;

        /// <inheritdoc />
        public Logger Logger => _logger;

        /// <inheritdoc />
        public bool AllowNormalize => _allowNormalize;

        /// <inheritdoc />
        public string Context => _context;

        /// <summary>
        /// Creates a filesystem-backed version environment.
        /// </summary>
        /// <param name="configuredDefaultSources">
        /// Default sources used when no explicit path is provided (e.g., "version.txt", "version.json").
        /// These are treated as relative to the current working directory.
        /// </param>
        /// <param name="logger">Logger for operational output.</param>
        /// <param name="allowNormalize">Normalization policy flag.</param>
        /// <param name="context">Diagnostic context label.</param>
        public DefaultVersionEnvironment(
            IReadOnlyList<FileVersionSource>? configuredDefaultSources,
            Logger logger,
            bool allowNormalize,
            string context)
        {
            _configuredDefaultSources = configuredDefaultSources ?? Array.Empty<FileVersionSource>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _allowNormalize = allowNormalize;
            _context = context ?? string.Empty;
        }

        /// <inheritdoc />
        public VersionResolveResult GetCurrentVersions(string? explicitPath = null)
        {
            IReadOnlyList<FileVersionSource> sources;

            if (!string.IsNullOrWhiteSpace(explicitPath))
            {
                sources = new[] { new FileVersionSource(explicitPath) };
            }
            else
            {
                sources = ResolveDefaultSourcesPresent(_configuredDefaultSources);
            }

            if (sources.Count == 0)
            {
                return new VersionResolveResult(
                    checkedSources: Array.Empty<string>(),
                    successfulVersions: Array.Empty<VersionModel>(),
                    errors: Array.Empty<ErrorInfo>(),
                    final: null
                );
            }

            return new VersionResolver(sources).ResolveVersionDetailed();
        }

        /// <inheritdoc />
        public void WriteVersion(VersionModel version, string explicitPath)
        {
            if (string.IsNullOrWhiteSpace(explicitPath))
                throw new ArgumentException(
                    "explicitPath is required for write operations.",
                    nameof(explicitPath));

            SafeFile.SafeWriteAllText(explicitPath, version.ToString());
        }

        private static IReadOnlyList<FileVersionSource> ResolveDefaultSourcesPresent(
            IReadOnlyList<FileVersionSource> defaults)
        {
            var sources = new List<FileVersionSource>();
            string cwd = Directory.GetCurrentDirectory();

            foreach (var source in defaults)
            {
                string path = Path.Join(cwd, source.Id);
                if (File.Exists(path))
                    sources.Add(source);
            }

            return sources;
        }
    }
}
