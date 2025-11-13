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
- Include `[skip ci]` when AutoVersion commits bump versions to avoid recursive runs. 

---

## 📁 Related Files

- `/docs/USAGE/CONFIG.md` – Config schema  
- `/docs/FAQ.md` – Troubleshooting  
- `/docs/TEMPLATES.md` – Release templates  
- `/docs/README.md` – Documentation hub  

---

**End of Workflows Guide (v1.1.3 – Automated Trust)**
