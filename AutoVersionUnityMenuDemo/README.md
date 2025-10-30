# 🎮 AutoVersion Unity Menu Demo (v0.6.0)

> **AutoVersion Lite** — Automated semantic versioning, changelog generation, and artifact handling for .NET, Unity, and CI pipelines.  
>  
> This Unity demo integrates AutoVersion directly into the **Unity Editor menu** under `Tools/AutoVersion/`, allowing developers to bump versions, generate changelogs, and inspect version info — all without leaving Unity.

---

## 🧱 Project Overview

| Component | Description |
|------------|-------------|
| **AutoVersion CLI** | The standalone tool (`autoversion.exe`) used to bump versions, create changelogs, and tag Git repos. |
| **Unity Menu Integration** | Custom Editor script that exposes AutoVersion commands under `Tools → AutoVersion`. |
| **Version Display** | Scene object that reads `version.txt` and shows the current project version at runtime. |
| **Demo Scene** | `Assets/AutoVersionDemo/Scenes/AutoVersionMenuDemo.unity` — demonstrates version display and Editor workflow. |

---

## 🧩 Folder Structure

```
AutoVersionUnityMenuDemo/
│
├── autoversion.exe
├── autoversion.json
├── version.txt
│
├── Assets/
│   └── AutoVersionDemo/
│       ├── Editor/
│       │   └── AutoVersionMenu.cs
│       ├── Scripts/
│       │   └── VersionDisplay.cs
│       ├── Scenes/
│       │   └── AutoVersionMenuDemo.unity
│       └── CHANGELOG.md
│
├── Packages/
│   └── manifest.json
│
└── ProjectSettings/
```

---

## ⚙️ Configuration (`autoversion.json`)

```json
{
  "git": {
    "tagPrefix": "v",
    "push": false
  },
  "changelog": {
    "path": "Assets/AutoVersionDemo/CHANGELOG.md"
  },
  "artifacts": [
    { "path": "Builds/Product.unitypackage", "rename": "Product_{version}.unitypackage" }
  ]
}
```

---

## 🧠 Using AutoVersion Inside Unity

### 📍 Menu Location  
**Unity Editor → Tools → AutoVersion**

| Command | Description |
|----------|-------------|
| **Show Current Version** | Reads `version.txt` and shows it in a dialog. |
| **Bump Major** | Increments version from `1.2.3` → `2.0.0`. |
| **Bump Minor** | Increments version from `1.2.3` → `1.3.0`. |
| **Bump Patch** | Increments version from `1.2.3` → `1.2.4`. |
| **Bump Prerelease** | Creates prerelease tags (e.g., `1.2.3-alpha.1`). |
| **Generate Changelog** | Runs the AutoVersion changelog engine. |
| **Open Changelog** | Opens the generated `CHANGELOG.md` in your OS default editor. |
| **Run Dry-Run Preview** | Simulates a version bump without file modification. |
| **Help → Documentation** | Opens the GitHub page. |
| **Help → About AutoVersion** | Shows credits and license info. |

---

## 🧪 Demo Scene

Scene path:  
```
Assets/AutoVersionDemo/Scenes/AutoVersionMenuDemo.unity
```

### Contains:
- `VersionDisplay` GameObject (with `VersionDisplay.cs` script)
- A `TextMeshPro` label showing the version from `version.txt`

### Workflow:
1. In Unity, open **Tools → AutoVersion → Show Current Version**  
   → Dialog confirms current version (e.g., `0.6.0`).
2. Use **Bump Patch** or **Minor/Major** to update version.
3. Confirm `version.txt` and `CHANGELOG.md` were updated.
4. Press **Play** — the `VersionDisplay` text updates to the new version.

---

## 📸 Screenshots

| Editor Menu | Demo Scene |
|--------------|------------|
| ![AutoVersion Menu Screenshot](docs/img/editor_menu.png) | ![Version Display Screenshot](docs/img/version_display.png) |

---

## 🔧 Requirements

| Requirement | Version |
|--------------|----------|
| Unity | 2022.3 LTS or newer |
| .NET Runtime | 8.0 |
| AutoVersion CLI | v0.5.0+ |

---

## 🧱 Building the CLI

If you’re working with the full AutoVersion source:

```powershell
pwsh _Infrastructure/build.ps1 -Release
```

Build output will appear in:
```
F:\UltiWork\money\AutoVersion\Builds\autoversion.exe
```

Copy `autoversion.exe` into your Unity project root (`AutoVersionUnityMenuDemo/`).

---

## 🪶 License

MIT License  
© 2025 **Solcogito S.E.N.C.**  
Authored by **Benoit Desrosiers**

---

## 🌐 Links

- [GitHub Repository](https://github.com/Solcogito/AutoVersion)
- [Unity Asset Documentation (coming soon)](https://solcogito.github.io/AutoVersion)

---

> *“Automate your release flow. Let creativity lead — let AutoVersion handle the rest.”*
