# 🧩 COMPONENT GUIDE — AutoVersion Lite v1.1.x

This guide describes every functional component in **AutoVersion Lite**, focusing only on what exists in the *current* version.

---

## ⚙️ Core Components (`AutoVersion.Core`)

### 🟩 VersionModel.cs  
Central semantic-version container and parser.

| Method | Description |
|--------|-------------|
| `Parse(string)` | Converts strings like `1.2.3-alpha.1` into structured data. |
| `TryParse(string, out VersionModel)` | Safe parsing variant. |
| `CompareTo(VersionModel)` | Standard semantic comparison. |
| `Bump(string part, string? preLabel)` | Bumps major/minor/patch or handles prerelease logic. |
| `ToString()` | Returns normalized semantic version. |

Handles:
- major/minor/patch
- prerelease identifiers
- metadata ignored for ordering
- normalization and validation

---

### 🟩 VersionEnvironment.cs (`IVersionEnvironment` / `DefaultVersionEnvironment`)  
Handles all **file system interactions**.

Responsible for:

- Locating version files  
  - `version.json` (if present)  
  - `version.txt`  
- Loading both and returning the **highest semantic version**
- Writing the version  
- Ensuring safe, testable file I/O  
- Providing paths for the CLI

This is injectable and fully mockable.

---

### 🟩 ICliLogger / ConsoleCliLogger  
Centralized CLI output.

- Capturable logs during tests (`FakeCliLogger`)
- Separation of console concerns
- Supports info/warn/error levels

---

## 🧩 CLI Components (`AutoVersion.Cli`)

### 🟦 ArgForge Schema  
Custom schema-driven parser defining:

- Commands  
- Options  
- Flags  
- Positional arguments  
- Type validation  
- Error messages & exit codes

It supports:

- Unknown flag detection  
- Missing positional detection  
- Duplicate argument rules  
- Proper error propagation

---

### 🟦 CommandRouter.cs  
The entry-point router:

- Builds the ArgForge schema  
- Parses raw arguments  
- Selects the correct command  
- Passes ArgResult to execution  
- Maps all errors to standardized exit codes  

---

### 🟩 Commands

#### **BumpCommand.cs**
```
autoversion bump <major|minor|patch|pre> [-p <label>] [--dry-run]
```

- Loads current version  
- Bumps part  
- Applies prerelease label when given  
- Writes file unless `--dry-run`  
- Logs actions  
- Exit codes:
  - `0` success  
  - `1` invalid input  
  - `2` fatal error  

#### **SetCommand.cs**
```
autoversion set <version>
```

- Validates provided version  
- Writes it using VersionEnvironment  
- Prevents unsafe overwrites  

#### **CurrentCommand.cs**
```
autoversion current
```

- Loads `version.json` and/or `version.txt`  
- Shows **the highest version**

---

## 📚 Supporting Files

| File | Description |
|------|-------------|
| `test/` | Unit tests for all components |
| `Directory.Build.props` | Shared compiler settings (.NET 8, warnings as errors) |

---

## 🧠 Planned Extensions (Not Yet Implemented)

- Changelog generator  
- Git tagging  
- Multi-file update engine  
- Unity Editor integration  

---

**End of Component Guide**
