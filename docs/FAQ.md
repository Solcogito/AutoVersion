# ❓ FAQ — AutoVersion Lite

This document answers the most common questions about installing, configuring, and using AutoVersion Lite.  
If something doesn’t work as expected, check here before opening an issue.

---

## 🧩 General

### 🟦 What is AutoVersion Lite?

AutoVersion Lite is a free open-source tool that automates:
- Semantic versioning (SemVer 2.0.0)
- CHANGELOG.md generation from commits
- Git tagging and artifact renaming
- Unity Editor integration for version management

It’s designed to save time and ensure consistent version control across your projects.

---

### 🟦 What platforms are supported?

AutoVersion Lite supports:
- Windows 10/11  
- macOS 13+  
- Linux (Ubuntu 22+, Fedora 38+)  
- Unity 2022.3 LTS and newer  
- .NET 8 SDK  

---

### 🟦 Is this compatible with Unity’s Package Manager (UPM)?

Yes. You can include AutoVersion as a package once it’s published via:
```
https://github.com/Solcogito/AutoVersion.git#upm
```
or include the `AutoVersion.Unity` folder manually under `Assets/Editor/`.

---

### 🟦 What’s the difference between Lite and Pro?

| Feature | Lite (Free) | Pro (Paid) |
|----------|-------------|-------------|
| SemVer bump | ✅ | ✅ |
| Changelog generation | ✅ | ✅ |
| Git tagging | ✅ | ✅ |
| Unity Editor menu | ✅ | ✅ |
| Artifact renaming | ✅ | ✅ |
| Custom changelog templates | ❌ | ✅ |
| Multi-project configs | ❌ | ✅ |
| Gumroad API / webhooks | ❌ | ✅ |
| GUI “Release Window” | ❌ | ✅ |

---

## ⚙️ Configuration

### 🟨 Where should `autoversion.json` be placed?

In your project root — the same folder as your `.sln` file or `Assets/` directory.  
Example:
```
MyProject/
├── Assets/
├── autoversion.json
├── CHANGELOG.md
└── Directory.Build.props
```

AutoVersion automatically detects the nearest `autoversion.json` when run from a subfolder.

---

### 🟨 What file types can AutoVersion update?

- **JSON** (package.json, manifest.json)  
- **XML** (Directory.Build.props, config.xml)  
- **Text / Regex** (AssemblyInfo.cs, custom files)  
- **Any text file** with token replacement via patterns or regex  

---

### 🟨 Can I use comments or trailing commas in `autoversion.json`?

No — it must be **valid JSON** according to the JSON spec (RFC 8259).  
Use tools like [JSONLint](https://jsonlint.com/) or run:
```
autoversion config --validate
```

---

### 🟨 Can I have multiple configs in one repo?

Yes. You can run AutoVersion with the `--config` flag:
```
autoversion bump minor --config ./modules/core/autoversion.json
```
Each config acts independently — useful for multi-package repositories or large Unity solutions.

---

## 🧾 Usage

### 🟩 How do I check the current version?

```
autoversion current
```

It will read from `autoversion.json` or from the first detected version file (`package.json`, `Directory.Build.props`, etc.).

---

### 🟩 How do I simulate a version bump?

Use `--dry-run`:
```
autoversion bump patch --dry-run
```
No files are modified; it simply prints what would change.

---

### 🟩 How do I generate a changelog?

```
autoversion changelog
```
AutoVersion scans all commits since the last Git tag and updates `CHANGELOG.md`.

---

### 🟩 How do I integrate it into Unity?

1. Copy `AutoVersion.Unity` and `AutoVersion.Core` into `Assets/Editor/AutoVersion/`.  
2. Open Unity and go to:  
   **Tools → AutoVersion → Bump Patch**  
3. Watch your version files and changelog update automatically.

For details, see `/docs/UNITY.md`.

---

## 🧪 Troubleshooting

### 🔴 Error: “Working directory not clean.”

Git has uncommitted changes.  
Either commit or stash your files:
```
git add .
git commit -m "chore: save work"
```
Then retry:
```
pwsh _Infrastructure/publish.ps1
```

---

### 🔴 Error: “Could not execute because the specified command or file was not found.”

The CLI wasn’t built. Run:
```
pwsh _Infrastructure/build.ps1 -Release
```
Then retry the publish script.

---

### 🔴 Error: “autoversion.json not found.”

Ensure the config file exists at the root.  
You can specify a custom path:
```
autoversion bump patch --config ./path/to/autoversion.json
```

---

### 🔴 CHANGELOG.md not updating

Check that your commit messages follow **Conventional Commits**, for example:
```
feat(ui): added dark mode toggle
fix: corrected null reference in VersionManager
```

AutoVersion groups commits based on their type (`feat`, `fix`, etc.).  
Non-conventional messages are ignored.

---

### 🔴 Git tag not created

- Ensure `git.push` is `true` in your config.  
- Make sure the repository is clean (`git status` must be empty).  
- Check remote access (`git remote -v`).  
- Verify that a duplicate tag doesn’t exist with `git tag`.

---

### 🔴 Unity: Menu not appearing

Make sure the file is placed under:
```
Assets/Editor/AutoVersionMenu.cs
```
Unity only compiles editor scripts from folders named `Editor/`.

If still missing, check the Unity Console for compilation errors.

---

### 🔴 Unity: Version didn’t change

Ensure the `autoversion.json` is reachable from the project root and not located inside `Assets/`.  
Run the CLI manually once to confirm configuration works.

---

### 🔴 “Object reference not set” errors in Editor

Ensure the AutoVersion.Core assembly is referenced correctly.  
If using source files, check namespaces:  
`Soleria.AutoVersion.Core` (or `Solcogito.AutoVersion.Core` depending on package).

---

## 🧠 Tips & Tricks

- Use commit scopes for clarity (`feat(core):`, `fix(ui):`)  
- Add pre-commit hooks to auto-format JSON and verify version bumps  
- Combine with GitHub Actions to auto-release on tags  
- Use `autoversion bump prerelease --pre alpha.1` for staged builds  
- Keep CHANGELOG short and actionable — one entry per meaningful change  

---

## 🔗 Resources

- [Semantic Versioning 2.0.0](https://semver.org/)  
- [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)  
- [GitHub Actions Docs](https://docs.github.com/en/actions)  
- [Unity Manual – Editor Scripting](https://docs.unity3d.com/Manual/editor-CustomEditors.html)  

---

## 📁 Related Files

- `/docs/CONFIG.md` – Configuration reference  
- `/docs/WORKFLOWS.md` – CI/CD automation  
- `/docs/UNITY.md` – Unity Editor integration  
- `/docs/QUICKSTART.md` – CLI + setup guide  

---

**End of FAQ**
