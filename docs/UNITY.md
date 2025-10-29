# 🎮 UNITY — AutoVersion Lite Integration Guide

This guide explains how to integrate AutoVersion Lite directly inside the Unity Editor.  
It provides in-editor menu commands to bump versions, generate changelogs, and sync your configuration with the CLI version.

---

## 🧱 Overview

AutoVersion Lite can be used:
- as a **command-line tool** for automated pipelines, or  
- as a **Unity Editor utility** for interactive version control inside your projects.

Both share the same configuration file (`autoversion.json`) and produce identical results.

---

## ⚙️ Requirements

- Unity **2022.3 LTS** or newer  
- .NET 8 SDK (for building the CLI if using combined workflow)  
- A valid `autoversion.json` in your Unity project root  

Recommended folder layout:

```
Assets/
├── Editor/
│   ├── AutoVersionMenu.cs
│   └── AutoVersion/
│       ├── Core.dll
│       ├── Unity.dll
│       └── autoversion.json
```

---

## 🧩 Installation

### Option 1 — Manual Import

1. Copy the following folders into your Unity project:
   ```
   src/AutoVersion.Unity/
   src/AutoVersion.Core/
   ```
2. Compile the DLLs or include the `.cs` source files directly under `Assets/Editor/AutoVersion/`.

### Option 2 — Unity Package Manager (future)

AutoVersion Lite will provide a UPM distribution:
```
https://github.com/Solcogito/AutoVersion.git#upm
```

To add manually:
1. Open **Edit → Project Settings → Package Manager**  
2. Add Git URL → paste the repository link  
3. Wait for the AutoVersion Lite package to install  

---

## 🧭 Editor Menu

After setup, a new menu will appear in Unity:

```
Tools → AutoVersion
  ├── Show Current Version
  ├── Bump Major
  ├── Bump Minor
  ├── Bump Patch
  ├── Bump Prerelease
  ├── Generate Changelog
  └── Open Config File
```

Each option executes the same logic as the CLI counterpart.

---

## 💻 Example Implementation (Editor/AutoVersionMenu.cs)

```csharp
using UnityEditor;
using UnityEngine;
using Solcogito.AutoVersion.Core;

public static class AutoVersionMenu
{
    private const string ConfigPath = "autoversion.json";

    [MenuItem("Tools/AutoVersion/Show Current Version")]
    public static void ShowVersion()
    {
        string version = VersionManager.GetCurrentVersion(ConfigPath);
        EditorUtility.DisplayDialog("AutoVersion", $"Current version: {version}", "OK");
    }

    [MenuItem("Tools/AutoVersion/Bump Patch")]
    public static void BumpPatch()
    {
        RunVersionCommand("patch");
    }

    [MenuItem("Tools/AutoVersion/Bump Minor")]
    public static void BumpMinor() => RunVersionCommand("minor");

    [MenuItem("Tools/AutoVersion/Bump Major")]
    public static void BumpMajor() => RunVersionCommand("major");

    [MenuItem("Tools/AutoVersion/Generate Changelog")]
    public static void GenerateChangelog()
    {
        if (CLI.Run("changelog"))
            EditorUtility.DisplayDialog("AutoVersion", "CHANGELOG.md generated successfully!", "OK");
    }

    private static void RunVersionCommand(string bumpType)
    {
        if (CLI.Run($"bump {bumpType}"))
            EditorUtility.DisplayDialog("AutoVersion", $"Bumped {bumpType} version successfully!", "OK");
        else
            EditorUtility.DisplayDialog("AutoVersion", "Failed to bump version.", "Close");
    }
}
```

---

## 🔧 Behavior

| Action | Description |
|---------|--------------|
| **Show Current Version** | Reads the current version from `autoversion.json` or detected file. |
| **Bump Major/Minor/Patch** | Increments the version and updates all linked files. |
| **Bump Prerelease** | Adds or increments prerelease tag (e.g., `1.2.3-alpha.1`). |
| **Generate Changelog** | Parses commits and updates `CHANGELOG.md`. |
| **Open Config File** | Opens `autoversion.json` directly in the inspector or code editor. |

---

## 🎨 Visual Feedback

AutoVersion Lite provides progress and result dialogs:

| Stage | Message |
|--------|----------|
| Running | “Bumping patch version…” |
| Success | “Version bumped successfully: 1.2.4 → 1.2.5” |
| Error | “Failed to locate configuration file or repo is dirty.” |

Additionally, Unity Console logs show:
```
[AutoVersion] Bumping patch version...
[AutoVersion] Version updated successfully.
[AutoVersion] CHANGELOG.md generated.
```

---

## 🧪 Testing Integration

### Local Test Steps

1. Open Unity Editor  
2. Create an empty project  
3. Add `autoversion.json` with a simple JSON config  
4. Create `Assets/Editor/AutoVersionMenu.cs` from the example above  
5. From the menu: **Tools → AutoVersion → Bump Patch**  
6. Check that your config files update correctly  

Expected output in the Console:
```
[AutoVersion] Current version: 1.0.0
[AutoVersion] New version: 1.0.1
```

---

## 🧠 Tips & Tricks

- Add **UnityEvent hooks** to trigger rebuilds after version bump.  
- Combine with **ScriptableBuildPipeline** to automatically version builds.  
- Use **MenuItem priorities** to integrate into your existing Tools menu.  
- Set `"git.push": false` in config if you prefer manual control over commits.

---

## ⚙️ Advanced Integration

You can also call AutoVersion from scripts:

```csharp
using Solcogito.AutoVersion.Core;

VersionManager.Bump("patch", dryRun: false);
CLI.Run("changelog");
```

Or hook into build pre-processing:
```csharp
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class VersionBuildHook : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        CLI.Run("bump patch");
        CLI.Run("changelog");
    }
}
```

---

## 📁 Related Files

- `/docs/CONFIG.md` – Configuration schema reference  
- `/docs/QUICKSTART.md` – Setup guide for CLI and Editor  
- `/docs/WORKFLOWS.md` – CI/CD automation examples  
- `/docs/FAQ.md` – Common troubleshooting  

---

**End of Unity Integration Guide**
