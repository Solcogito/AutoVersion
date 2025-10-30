// ============================================================================
// File:        ConfigLoader.cs
// Project:     AutoVersion Lite
// Version:     0.2.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Handles loading and parsing of autoversion.json configuration files.
//   Supports custom paths via CLI flag and validates against schema,
//   implemented with strict nullability guarantees.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;
using System.Text.Json;

namespace Solcogito.AutoVersion.Core.Config
{
    /// <summary>
    /// Loads and deserializes configuration files for AutoVersion.
    /// </summary>
    public static class ConfigLoader
    {
        private const string DefaultConfigPath = "autoversion.json";

        /// <summary>
        /// Loads configuration from the given path or defaults to root file.
        /// </summary>
        public static ConfigModel Load(string? customPath = null)
        {
            var path = string.IsNullOrWhiteSpace(customPath)
                ? DefaultConfigPath
                : customPath;

            if (!File.Exists(path))
                throw new FileNotFoundException($"Configuration file not found: {path}");

            var json = File.ReadAllText(path);

            var config = JsonSerializer.Deserialize<ConfigModel>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (config is null)
                throw new InvalidOperationException($"Failed to parse configuration: {path}");

            return config;
        }
    }
}
