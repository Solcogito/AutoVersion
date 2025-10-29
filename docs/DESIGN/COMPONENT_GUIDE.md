# 🧩 COMPONENT_GUIDES — AutoVersion Lite v0.0.1

---

## ⚙️ Core Components

### 🟩 VersionModel.cs
Encapsulates semantic version logic.

| Method | Description |
|---------|--------------|
| `Parse(string)` | Validates and converts a version string into structured data. |
| `Bump(string, string?)` | Increments version part (`major`, `minor`, `patch`, or `prerelease`). |
| `CompareTo(VersionModel)` | Implements version comparison. |
| `ToString()` | Returns normalized version string. |

**Example**
```csharp
var v = VersionModel.Parse("1.2.3-alpha");
var next = v.Bump("patch");
Console.WriteLine(next); // 1.2.4
```

---

### 🟩 VersionManager.cs
Handles file I/O for reading and writing version data.

| Method | Description |
|---------|--------------|
| `ReadCurrentVersion()` | Reads from `version.txt` or fallback file. |
| `WriteVersion(VersionModel)` | Writes current version string to file. |
| `Bump(string, bool)` | Reads, increments, and writes version (or simulates in dry-run). |

**Example**
```csharp
VersionManager.Bump("minor");
```

---

### 🟩 CommandRouter.cs
Parses CLI arguments using `System.CommandLine`.

| Command | Description |
|----------|-------------|
| `autoversion current` | Displays current version. |
| `autoversion bump [type]` | Increments the version (`major`, `minor`, etc). |
| `--dry-run` | Simulates without writing. |
| `--pre` | Adds prerelease label. |

---

## 🧠 Planned Components (v0.2.0+)

| Name | Purpose |
|------|----------|
| `ConfigLoader` | Reads & validates `autoversion.json` |
| `FileUpdater` | Applies version replacements in multiple file types |
| `ChangelogGenerator` | Creates CHANGELOG.md from Git commits |
| `GitWrapper` | Handles clean repo checks, tagging, and pushing |

---

## 🧾 Unity Integration Components (v0.6.0+)

| File | Description |
|-------|--------------|
| `AutoVersionMenu.cs` | Adds menu entries under Tools/AutoVersion |
| `AutoVersionWindow.cs` *(planned)* | GUI version dashboard |
| `VersionStatusDrawer.cs` *(planned)* | EditorWindow for current version & history |

---

## 🧩 Utility Scripts

| File | Description |
|-------|--------------|
| `build.ps1 / build.sh` | Cross-platform build & test automation |
| `publish.ps1` | End-to-end release workflow |
| `autoversion.schema.json` | JSON schema validation for config file |

---

**End of Component Guides**
