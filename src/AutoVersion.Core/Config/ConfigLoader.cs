// ============================================================================
// File:        ConfigLoader.cs
// Project:     AutoVersion Lite
// Version:     0.6.0
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Provides simple loading of the AutoVersion configuration file
//   (autoversion.json). This Lite version intentionally avoids any
//   Git- or artifact-specific behavior: it only deserializes the
//   configuration into ConfigModel and lets higher layers decide
//   what to do with it.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.IO;
using System.Text.Json;

namespace Solcogito.AutoVersion.Core.Config
{
    public static class ConfigLoader
    {
        public const string DefaultFileName = "autoversion.json";

        /// <summary>
        /// Gets the default configuration path (current directory + autoversion.json).
        /// </summary>
        public static string DefaultConfigPath =>
            Path.Combine(Directory.GetCurrentDirectory(), DefaultFileName);

        /// <summary>
        /// Loads the configuration file into a ConfigModel instance.
        /// </summary>
        /// <param name="customPath">
        /// Optional custom path. If null or empty, DefaultConfigPath is used.
        /// </param>
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
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                });

            if (config is null)
                throw new InvalidOperationException("Configuration invalid: deserialization returned null.");

            return config;
        }
    }
}
