# 🚀 QUICKSTART — AutoVersion v1.3.x

This guide helps you install and use **AutoVersion Lite v1.1.x** — a simple, deterministic semantic‑versioning CLI.

AutoVersion Lite does **not** support configuration files, Git tagging, changelog generation, or Unity integration.  
Any such features belong to future or Pro editions.

---

## 🧩 Requirements
- .NET 8 SDK or newer  
- PowerShell 7+ or Bash  
- Git (optional)

---

## 🧱 Step 1 — Clone the Repository
```bash
git clone https://github.com/Solcogito/AutoVersion.git
cd AutoVersion
```

---

## ⚙️ Step 2 — Build and Test
Windows:
```powershell
pwsh _Infrastructure/build.ps1
```

Linux/macOS:
```bash
bash _Infrastructure/build.sh
```

This builds the solution and runs all tests in `src/AutoVersion.Tests`.

---

## ⚙️ Step 3 — Check Current Version
```bash
autoversion current
```

AutoVersion Lite reads:
- `version.json` (if present)  
- `version.txt`

And returns the **highest** valid semantic version.

---

## ⚙️ Step 4 — Set a Version
```bash
autoversion set 1.2.3
```
Writes the version to the environment’s selected file.

---

## ⚙️ Step 5 — Bump Versions
Patch:
```bash
autoversion bump patch
```

Minor (dry‑run):
```bash
autoversion bump minor --dry-run
```

Prerelease:
```bash
autoversion bump prerelease --pre alpha
```

---

## [Pro Feature — Not available in AutoVersion Lite]
### Advanced Features  
The following exist only in Pro/future versions:
- `autoversion.json` configuration  
- multi‑file propagation  
- Unity menu integration  

---

## 🔗 Next Steps
See the DESIGN documentation for deeper insight into the architecture.

---

**End of Quickstart**
