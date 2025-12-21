# ❓ FAQ — AutoVersion v1.3.x

This FAQ covers **ONLY the real features of AutoVersion Lite v1.1.x**.  
All configuration, Git integration, changelog generation, Unity menus, templates, or artifact systems  
are **not part of AutoVersion Lite** and belong to other Solcogito tools.

---

# 🧩 General

## 🟦 What is AutoVersion?

AutoVersion is a lightweight, deterministic semantic-versioning CLI.

It provides:
- Reading versions from `version.txt` and/or `version.json`
- Selecting the **highest semantic version**
- Version bumping (`major`, `minor`, `patch`, `pre`)
- Setting versions explicitly
- Safe dry-run functionality
- Fully testable environment (no external dependencies)

AutoVersion does **not**:
- generate changelogs  
- tag Git commits  
- rename artifacts  
- use configuration files  
- update multiple files  
- integrate into Unity  

These belong to *other Solcogito tools*, not Lite.

---

## 🟦 What platforms are supported?

- Windows 10/11  
- Linux (via .NET 8)  
- macOS (via .NET 8)  
- .NET 8 CLI

Unity **is not supported** in AutoVersion Lite.

---

# 🧭 Usage

## 🟩 How do I check the current version?
```
autoversion current
```

AutoVersion Lite will:
1. Look for `version.json`  
2. Look for `version.txt`  
3. Return the **highest valid version**

---

## 🟩 How do I set the version?
```
autoversion set 1.2.3
```

---

## 🟩 How do I bump the version?

```
autoversion bump patch
autoversion bump minor
autoversion bump major
autoversion bump prerelease --pre alpha
```

Dry-run example:
```
autoversion bump patch --dry-run
```

---

# 🧪 Troubleshooting

## 🔴 “Version file not found”
AutoVersion Lite needs:
- `version.json` **or**
- `version.txt`

Create one manually, for example:
```
1.0.0
```

---

## 🔴 “Invalid version string”
Ensure your version follows SemVer:
```
MAJOR.MINOR.PATCH[-PRERELEASE]
```

Valid:
- `1.0.0`
- `2.1.3-alpha.1`
- `0.1.0`

Invalid:
- `version 1`
- `1.a.0`
- `1.0`

---

## 🔴 “Nothing changed — dry run mode”
Dry-run prevents writing to disk.

Remove `--dry-run` to apply changes:
```
autoversion bump minor
```

---

## 🔴 “Permission denied” or “file in use”
Your version file may be:
- read-only  
- blocked by an antivirus  
- locked by another application  

Resolve by:
- closing editors  
- enabling write permissions  

---

## 🧠 Tips & Tricks

- Use `--dry-run` before committing anything.  
- Prefer prerelease labels during iterative development:
  ```
  autoversion bump prerelease --pre beta
  ```
- Keep only **one** version file unless both are needed.  
- Commit version changes explicitly for clarity.  

---

# 🗂 Related Documentation

- `AUTOVERSION_LITE.md` — Complete CLI reference  
- `QUICKSTART_LITE_1.3.x.md` — Setup + basic usage  
- `ARCHITECTURE.md` — Internal project design  

---

**End of FAQ (Lite Version)**