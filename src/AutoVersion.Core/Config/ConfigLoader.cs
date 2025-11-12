// ============================================================================
// File:        ConfigLoader.cs
// Project:     Solcogito.AutoVersion.Core
// Description: Loads the configuration file and logs all JSON entries.
//              Creates default autoversion.json if missing or invalid,
//              validates structure, and safely recreates if needed.
//              Supports read-only detection and CI override flag.
// ============================================================================

using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Solcogito.AutoVersion.Core.Config;

namespace Solcogito.AutoVersion.Core
{
    public static class ConfigLoader
    {
        public static string DefaultConfigPath => "autoversion.json";

        public static ConfigModel Load(string? customPath = null)
        {
            var path = string.IsNullOrWhiteSpace(customPath)
                ? DefaultConfigPath
                : customPath;

            // CI/CD automation flag (forces default recreation without prompt)
            bool forceDefault = Environment.GetEnvironmentVariable("AUTOVERSION_FORCE_DEFAULT")?.ToLowerInvariant() == "true";

            // ----------------------------------------------------------------
            // CASE 1 — Missing file → create immediately
            // ----------------------------------------------------------------
            if (!File.Exists(path))
            {
                Console.WriteLine($"[WARN] Configuration file not found: {path}");
                Console.WriteLine("[INFO] Creating new default configuration...");

                var defaultConfig = CreateDefaultConfig();
                WriteJson(path, defaultConfig);

                Console.WriteLine($"[SUCCESS] Default configuration created at: {path}");
                return JsonSerializer.Deserialize<ConfigModel>(
                    JsonSerializer.Serialize(defaultConfig)
                )!;
            }

            // ----------------------------------------------------------------
            // CASE 2 — File exists → try to parse & validate
            // ----------------------------------------------------------------
            try
            {
                string json = File.ReadAllText(path);
                var config = JsonSerializer.Deserialize<ConfigModel>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (config == null)
                    throw new InvalidOperationException("Configuration deserialization returned null.");

                // ✅ Validate structure
                try
                {
                    ConfigValidator.Validate(config);
                }
                catch (Exception valEx)
                {
                    return HandleInvalidConfig(path, valEx.Message, forceDefault);
                }

                return config;
            }
            catch (JsonException jex)
            {
                Console.WriteLine($"[ERROR] Invalid JSON syntax: {jex.Message}");
                return HandleInvalidConfig(path, "Invalid JSON syntax", forceDefault);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load configuration: {ex.Message}");
                return HandleInvalidConfig(path, "Deserialization or IO error", forceDefault);
            }
        }

        // --------------------------------------------------------------------
        // Handle invalid or corrupted configuration with safe overwrite
        // --------------------------------------------------------------------
        private static ConfigModel HandleInvalidConfig(string path, string reason, bool forceDefault)
        {
            Console.WriteLine();
            Console.WriteLine($"[ERROR] Configuration invalid: {reason}");

            // Check for read-only before prompt or force
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.IsReadOnly)
                {
                    Console.WriteLine($"[ERROR] File is read-only: {path}");
                    Console.WriteLine("[HINT] Remove the Read-only flag or adjust permissions before retrying.");
                    throw new UnauthorizedAccessException($"Cannot recreate configuration: '{path}' is read-only.");
                }
            }

            if (!forceDefault)
            {
                Console.WriteLine("[PROMPT] Would you like to recreate a default configuration file?");
                Console.WriteLine($"This will overwrite: {path}");
                Console.Write("Type 'Y' to confirm, or any other key to cancel: ");
                var key = Console.ReadKey();
                Console.WriteLine();

                if (char.ToUpperInvariant(key.KeyChar) != 'Y')
                {
                    Console.WriteLine("[ABORTED] Configuration recreation cancelled by user.");
                    throw new InvalidOperationException("Configuration validation failed and user cancelled regeneration.");
                }
            }
            else
            {
                Console.WriteLine("[INFO] AUTOVERSION_FORCE_DEFAULT=true → recreating configuration automatically...");
            }

            var defaultConfig = CreateDefaultConfig();
            WriteJson(path, defaultConfig);
            Console.WriteLine($"[SUCCESS] Default configuration recreated at: {path}");

            return JsonSerializer.Deserialize<ConfigModel>(
                JsonSerializer.Serialize(defaultConfig)
            )!;
        }

        // --------------------------------------------------------------------
        // Helper: Serialize object to indented JSON and save to disk
        // --------------------------------------------------------------------
        private static void WriteJson(string path, object data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        // --------------------------------------------------------------------
        // Default configuration aligned with real autoversion.json schema
        // --------------------------------------------------------------------
        private static object CreateDefaultConfig()
        {
            // Step 1: Define standard config structure (without schema)
            var config = new
            {
                versionFile = "version.txt",

                files = new[]
                {
                    new {
                        path = "version.txt",
                        type = "text",
                        token = "__VERSION__",
                        encoding = "utf-8"
                    },
                    new {
                        path = "README.md",
                        type = "text",
                        token = "__VERSION__",
                        encoding = "utf-8"
                    }
                },

                git = new
                {
                    tagPrefix = "test-v",
                    push = true,
                    allowDirty = false,
                    commitMessage = "test(release): bump version to {version}"
                },

                logging = new
                {
                    verbose = true,
                    color = true,
                    level = "info"
                },

                dryRun = false
            };

            // Step 2: Convert to dictionary and inject "$schema"
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(config)
            )!;
            dict["$schema"] = "./_Infrastructure/autoversion.schema.json";

            return dict;
        }
    }
}
