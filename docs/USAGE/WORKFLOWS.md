# ⚙️ WORKFLOWS — AutoVersion Lite v0.8.0 CI & Quality Gates

This guide details how to integrate **AutoVersion Lite** into continuous-integration pipelines for building, testing, linting, and automatically releasing your projects.

---

## 🧩 Overview

**Goal:** Guarantee cross-platform consistency and commit quality.

Your CI/CD stack now includes:

| Workflow | Purpose | Location |
|-----------|----------|-----------|
| 🧱 **ci.yml** | Builds, tests, and validates config on Windows / macOS / Linux | `.github/workflows/ci.yml` |

All workflows rely on a single `.commitlintrc.json` file in the repo root.

---

## 🧱 CI Matrix – `ci.yml`

Runs builds and tests across all supported operating systems.

```yaml
name: CI – Build & Test Matrix

on:
  push:
    branches: [ main, develop ]
  pull_request:

jobs:
  build-test:
    name: Build & Test (${{ matrix.os }})
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.0.x

      - name: Restore dependencies
        run: dotnet restore

      - name: Build solution
        run: dotnet build --configuration Release --no-restore

      - name: Run tests
        run: dotnet test --configuration Release --no-build --verbosity normal

      - name: Validate configuration
        run: autoversion config --validate

      - name: Preview changelog
        run: autoversion changelog --dry-run

      - name: Lint commit messages (Linux only)
        if: runner.os == 'Linux'
        run: npx commitlint --from=HEAD~10 --to=HEAD
```

✅ **Deliverables**
- Green CI on all OSes  
- Verified config & changelog syntax  
- Enforced Conventional Commits

---

## 🧹 Lint & Docs Validation – `lint.yml`

Ensures consistent commits, valid JSON, and clean Markdown.

```yaml
name: Lint – Schema, Docs & Commits

on:
  push:
    branches: [ main, develop ]
  pull_request:

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: 20

      - name: Install commitlint
        run: npm install --no-save @commitlint/{config-conventional,cli}

      - name: Lint commit messages
        run: npx commitlint --from=HEAD~10 --to=HEAD

      - name: Validate JSON files
        run: |
          npm install -g jsonlint
          jsonlint -q autoversion.json

      - name: Markdown lint
        run: |
          npm install -g markdownlint-cli
          markdownlint "**/*.md" --ignore node_modules

      - name: PowerShell syntax check
        run: pwsh -Command "Get-ChildItem -Recurse -Filter *.ps1 | ForEach-Object { Write-Host Checking $_; pwsh -NoLogo -NoProfile -Command \"[System.Management.Automation.PSParser]::Tokenize((Get-Content $_ -Raw), [ref]$null)\" }"
```

✅ **Checks**
- Conventional Commit titles  
- Valid `autoversion.json` schema  
- Markdown formatting  
- PowerShell syntax

---

## 🚀 Automated Releases – `release-on-tag.yml`

Triggers when a semantic tag (e.g., `v1.0.0`) is pushed.

```yaml
name: Release on Tag

on:
  push:
    tags:
      - "v*.*.*"

jobs:
  release:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Extract version
        id: vars
        run: echo "version=${GITHUB_REF#refs/tags/}" >> $GITHUB_ENV

      - name: Generate changelog preview
        run: autoversion changelog --dry-run

      - name: Create GitHub Release
        uses: softprops/action-gh-release@v2
        with:
          tag_name: ${{ env.version }}
          name: "AutoVersion Lite ${{ env.version }}"
          body_path: CHANGELOG.md
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

✅ **Outcome**
- Reads version from Git tag  
- Generates changelog preview  
- Publishes GitHub Release automatically

---

## 🔧 Commit Lint Configuration – `.commitlintrc.json`

```json
{
  "extends": ["@commitlint/config-conventional"],
  "rules": {
    "type-enum": [
      2,
      "always",
      [
        "feat", "fix", "docs", "style",
        "refactor", "perf", "test",
        "build", "ci", "chore", "revert"
      ]
    ],
    "header-max-length": [2, "always", 100]
  }
}
```

Place this file at the repository root.  
Every push or PR will be validated automatically by the Lint workflow.

---

## 🧠 Best Practices

- Use **feature branches** → PR → merge → tag for release.  
- Keep changelogs human-readable; use `autoversion changelog --dry-run` before committing.  
- Include `[skip ci]` when AutoVersion commits bump versions to avoid recursive runs.  
- Always test your `release-on-tag.yml` with a dummy tag before production.

---

## 📁 Related Files

- `/docs/CONFIG.md` – Config schema  
- `/docs/FAQ.md` – Troubleshooting  
- `/docs/TEMPLATES.md` – Release templates  
- `/docs/README.md` – Documentation hub  

---

**End of Workflows Guide (v0.8.0 – Automated Trust)**
