# 📘 AUTOVERSION.md
**AutoVersion Lite – Full CLI Reference**

> The authoritative reference for all AutoVersion Lite commands, flags, and usage patterns.  
> For onboarding, see **QUICKSTART.md**.  
> For configuration details, see **CONFIG.md**.

---

# 🧭 Overview

AutoVersion Lite is a cross-platform CLI tool designed for **semantic versioning**, **Git tagging**,  
**file updates**, and **artifact renaming** through a simple configuration file.

This document lists **all commands**, **all flags**, and **usage examples**.

To view help from the CLI:
```bash
autoversion help
```

---

# 🧩 Commands

## 1. autoversion current
Displays the current detected version.

**Usage**
```bash
autoversion current
```

**Example Output**
```
Current version: 1.4.2
```

---

## 2. autoversion bump
Increments the current version according to SemVer rules.

### Allowed bump types
- `major`
- `minor`
- `patch`
- `prerelease`

**Usage**
```bash
autoversion bump <major|minor|patch|prerelease>
```

### Examples
```bash
autoversion bump patch
autoversion bump minor --dry-run
autoversion bump prerelease --allow-dirty
```

---

## 3. autoversion config
Validate or inspect the configuration file (`autoversion.json`).

### Validate against schema
```bash
autoversion --config --validate
```

### Print loaded config (debugging)
```bash
autoversion --config --show
```

---

## 4. autoversion help
Lists all commands and options.

```bash
autoversion --help
```

---

# ⚙️ Global Options

These flags are available on all commands:

| Flag | Description |
|------|-------------|
| `--config <path>` | Use a custom configuration file |
| `--dry-run` | Simulate all changes without writing to disk |
| `--allow-dirty` | Allow running even if Git working directory is not clean |
| `--no-git` | Skip all Git operations (tagging, pushing) |
| `-h`, `--help` | Show help |

---

# 🏷️ Git Integration

AutoVersion supports:

- Tag creation  
- Tag pushing  
- Repository cleanliness checks  
- Tag prefix enforcement  

Configured in `autoversion.json`:

```json
{
  "git": {
    "tagPrefix": "v",
    "push": true
  }
}
```

### Skip Git steps for testing or CI
```bash
autoversion bump patch --no-git
```

---

# ⚙️ Configuration File

AutoVersion Lite uses a JSON configuration file to determine:

- Which files to update  
- Where the version field is located  
- How artifacts should be renamed  
- Whether Git tagging is enabled  

### Supported file formats (Lite version)

AutoVersion Lite supports **JSON version fields**, which covers the most common workflows:

- `package.json`
- Unity metadata containing JSON
- Any custom JSON configuration

Example:
```json
{
  "files": [
    { "path": "package.json", "type": "json", "key": "version" }
  ]
}
```
---

# 📦 Artifact Handling

AutoVersion can rename built files automatically when bumping versions:

```json
{
  "artifacts": [
    {
      "path": "Builds/Product.unitypackage",
      "rename": "Product_{version}.unitypackage"
    }
  ]
}
```

Triggered when running:
```bash
autoversion bump patch
```

---

# 🧪 Usage Examples

### Simulate a patch bump (no files changed)
```bash
autoversion bump patch --dry-run
```

### Increment prerelease number
```bash
autoversion bump prerelease
```

### Skip Git tagging
```bash
autoversion bump minor --no-git
```

### Use a custom config file
```bash
autoversion --config configs/dev.json bump patch
```

---

# 🐞 Troubleshooting

### “Repository is not clean”
AutoVersion stops by default to protect users.

Override:
```bash
autoversion bump patch --allow-dirty
```

---

# 🔧 Exit Codes (IMPORTANT: VERIFY THESE IN YOUR CODEBASE)
**These values must be checked in your implementation.  
Your actual C# implementation may differ.**

| Code | Meaning |
|------|---------|
| `0` | Success |
| `1` | Invalid arguments |
| `2` | Invalid or missing configuration |
| `3` | Version file not found or unreadable |
| `4` | Git error (tag, push, dirty, or missing repo) |
| `5` | I/O or write failure |

> ⚠️ Reminder: Compare these to your actual implementation and update accordingly.

---

# 🗂 Related Documentation

- **README.md** – Overview + usage  
- **QUICKSTART.md** – Beginner guide  
- **CONFIG.md** – Full configuration details  
- **WORKFLOWS.md** – CI/CD examples  
- **ARCHITECTURE.md** – Internal structure  
- **FAQ.md** – Common issues  

---

# © License

AutoVersion Lite  
© 2025 Solcogito S.E.N.C.  
MIT Licensed  
Author: Benoit Desrosiers
