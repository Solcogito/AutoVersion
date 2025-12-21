# 📘 AutoVersion — CLI Reference (v1.3.x)

This is the **official CLI reference** for **AutoVersion Lite v1.1.x**.  
It documents ONLY features that exist in the Lite version.

AutoVersion Lite provides:
- deterministic semantic versioning
- safe version reads/writes
- simple bump, set, and current commands
- version.txt / version.json resolution (highest wins)

Lite DOES NOT provide:
- configuration files (`autoversion.json`)
- Git tagging
- changelog generation
- multi-file version propagation
- templates
- artifact renaming
- Unity integration
- publish pipelines

These belong to *other programs*, not Lite or Pro.

---

# 🧭 Commands

## 1. autoversion current
Reads `version.json` and/or `version.txt` and prints the **highest** version.

```
autoversion current
```

---

## 2. autoversion set <version>
Writes a new version.

```
autoversion set 1.2.3
```

Fails if the version is invalid.

---

## 3. autoversion bump <major|minor|patch|pre>
Bumps a specific part of the version.

### Usage
```
autoversion bump <type> [-p label] [--dry-run]
```

### Examples
```
autoversion bump patch
autoversion bump minor --dry-run
autoversion bump prerelease --pre alpha
```

---

# ⚙️ Global Options (Lite)

| Flag | Description |
|------|-------------|
| `--dry-run` | Simulate change without writing to disk |
| `-p`, `--pre <label>` | Prerelease label |

---

# 🧪 Exit Codes

| Code | Meaning |
|------|---------|
| 0 | Success |
| 1 | Invalid input |
| 2 | File I/O or unexpected error |

---

# 📁 Version Lookup Rules

AutoVersion Lite reads:
- **version.json** (if present)
- **version.txt**

The **highest semantic version** is returned.

---

**End of AutoVersion Lite CLI Reference**
