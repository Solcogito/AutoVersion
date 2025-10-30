// ============================================================================
// File:        ConfigValidator.cs
// Project:     AutoVersion Lite
// Version:     0.2.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Validates autoversion.json against the provided JSON schema.
//   Relies on NJsonSchema if available, or performs minimal manual validation.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Solcogito.AutoVersion.Core.Config
{
    /// <summary>
    /// Performs lightweight validation of the loaded configuration.
    /// </summary>
    public static class ConfigValidator
    {
        /// <summary>
        /// Checks for required fields and logical consistency.
        /// </summary>
        public static void Validate(ConfigModel cfg)
        {
            if (cfg.Files == null || cfg.Files.Count == 0)
                throw new InvalidOperationException("Configuration must include at least one file target.");

            foreach (var f in cfg.Files)
            {
                if (string.IsNullOrWhiteSpace(f.Path))
                    throw new InvalidOperationException("Each file entry must define a path.");
                if (string.IsNullOrWhiteSpace(f.Type))
                    throw new InvalidOperationException($"File '{f.Path}' missing required 'type' property.");
            }
        }
    }
}
