# ⚙️ WORKFLOWS — AutoVersion Lite v1.3.x

AutoVersion Lite v1.1.x has **no required CI workflow** and does not depend on external validation tasks.

Earlier versions referenced large matrix CI pipelines — these have been removed for Lite.

---

## 🧩 Minimal Suggested Workflow (Optional)

Even though AutoVersion Lite has no built‑in CI requirements, you may adopt a simple workflow:

```yaml
name: Build & Test

on: [push, pull_request]

jobs:
  build-test:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.0.x

      - name: Build
        run: dotnet build --configuration Release

      - name: Test
        run: dotnet test --configuration Release
```

This ensures your project builds and passes tests.

---

## ❌ Removed from AutoVersion
The following should **not** be included in AutoVersion usage:

- commitlint  
- config validation  
- changelog preview  
- Unity workflows  
- Git tagging  
- release pipelines  
- artifact renaming  

These belong to *future or Pro features*.

---

## [Planned Feature — Not available in AutoVersion now]
### Advanced CI/CD Capabilities (reference only)
AutoVersion will support:
- version=aware release pipelines  
- config validation   

---

**End of Workflows**
