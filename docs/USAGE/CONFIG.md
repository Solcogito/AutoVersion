# ⚙️ CONFIG — AutoVersion Lite

Configuration reference for `autoversion.json`.  
Defines how AutoVersion detects, updates, and propagates version numbers across your project files.

---

## 🧩 Overview

`autoversion.json` controls every aspect of the tool’s behavior:  
- Which files contain version numbers  
- How to replace or bump them  
- How changelogs and Git tags are generated  
- What artifacts should be renamed during release

This file lives at your **project root** and can be customized per repository.

---

## 🧱 Basic Example

```json
{
  "versionFile": "Directory.Build.props",
  "files": [
    { "path": "package.json", "type": "json", "key": "version" },
    { "path": "Directory.Build.props", "type": "xml", "xpath": "/Project/PropertyGroup/Version" },
    { "path": "AssemblyInfo.cs", "type": "regex", "pattern": "AssemblyVersion\\(\"(.*?)\"\\)" }
  ],
  "artifacts": [
    { "path": "Builds/Product.unitypackage", "rename": "Product_{version}.unitypackage" }
  ],
  "git": {
    "tagPrefix": "v",
    "push": true
  },
  "changelog": {
    "sections": {
      "feat": "Features",
      "fix": "Fixes",
      "docs": "Documentation",
      "chore": "Maintenance"
    }
  }
}
```

---

## 🧩 Schema Reference

Each property is optional unless noted as **Required**.

| Key | Type | Required | Description |
|------|------|-----------|-------------|
| `versionFile` | string | No | Fallback version source file used if no tag is found. |
| `files` | array | No | List of files to update with the new version. |
| `artifacts` | array | No | Files to rename after bump. |
| `git` | object | No | Defines Git tagging and push behavior. |
| `changelog` | object | No | Changelog generation options. |

---

### 🔹 files[]

Defines all files where version numbers are stored.

| Field | Type | Required | Description |
|--------|------|-----------|-------------|
| `path` | string | ✅ Yes | File path relative to project root. |
| `type` | string | ✅ Yes | File type: `json`, `xml`, `regex`, or `text`. |
| `key` | string | No | JSON key or XML element to modify. |
| `xpath` | string | No | XPath query for XML version nodes. |
| `pattern` | string | No | Regex pattern for custom replacement. |
| `replace` | string | No | Replacement string (optional custom token logic). |

Example:
```json
"files": [
  { "path": "package.json", "type": "json", "key": "version" },
  { "path": "src/Config.xml", "type": "xml", "xpath": "/Root/Meta/Version" },
  { "path": "include/version.h", "type": "regex", "pattern": "#define APP_VERSION\\s+\"(.*?)\"" }
]
```

---

### 🔹 artifacts[]

Used to rename build artifacts when a release occurs.

| Field | Type | Required | Description |
|--------|------|-----------|-------------|
| `path` | string | ✅ Yes | Input file path. |
| `rename` | string | ✅ Yes | Target file name with `{version}` placeholder. |
| `overwrite` | bool | No | If true, overwrites destination file. |

Example:
```json
"artifacts": [
  { "path": "Builds/Product.unitypackage", "rename": "Product_{version}.unitypackage" }
]
```

---

### 🔹 git

Controls how AutoVersion interacts with Git.

| Field | Type | Required | Description |
|--------|------|-----------|-------------|
| `tagPrefix` | string | No | Prefix for tags (e.g. `v1.0.0`). |
| `push` | bool | No | If true, pushes tag to remote automatically. |
| `allowDirty` | bool | No | Allows tagging with uncommitted changes. |

Example:
```json
"git": {
  "tagPrefix": "v",
  "push": true,
  "allowDirty": false
}
```

---

### 🔹 changelog

Controls Conventional Commit parsing and markdown generation.

| Field | Type | Required | Description |
|--------|------|-----------|-------------|
| `sections` | object | No | Mapping of commit types to markdown headers. |
| `template` | string | No | Custom markdown template path. |
| `includeScope` | bool | No | Whether to include commit scopes (feat(ui): ...). |
| `maxCommits` | int | No | Maximum commits to parse per release. |

Example:
```json
"changelog": {
  "sections": {
    "feat": "Features",
    "fix": "Bug Fixes",
    "docs": "Documentation",
    "perf": "Performance",
    "chore": "Maintenance"
  },
  "includeScope": true
}
```

---

## 🧾 Full Schema Example (autoversion.schema.json)

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "AutoVersion Configuration",
  "type": "object",
  "properties": {
    "versionFile": { "type": "string" },
    "files": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "path": { "type": "string" },
          "type": { "type": "string", "enum": ["json", "xml", "regex", "text"] },
          "key": { "type": "string" },
          "xpath": { "type": "string" },
          "pattern": { "type": "string" },
          "replace": { "type": "string" }
        },
        "required": ["path", "type"]
      }
    },
    "artifacts": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "path": { "type": "string" },
          "rename": { "type": "string" },
          "overwrite": { "type": "boolean" }
        },
        "required": ["path", "rename"]
      }
    },
    "git": {
      "type": "object",
      "properties": {
        "tagPrefix": { "type": "string" },
        "push": { "type": "boolean" },
        "allowDirty": { "type": "boolean" }
      }
    },
    "changelog": {
      "type": "object",
      "properties": {
        "sections": { "type": "object" },
        "template": { "type": "string" },
        "includeScope": { "type": "boolean" },
        "maxCommits": { "type": "integer" }
      }
    }
  },
  "additionalProperties": false
}
```

---

## 🧠 Best Practices

- Always validate your config using:
  ```
  autoversion config --validate
  ```
- Use **dry-run mode** to preview file changes before committing:
  ```
  autoversion bump patch --dry-run
  ```
- Store `autoversion.json` in source control, next to your `.csproj` or Unity project.
- Use consistent JSON formatting (2 spaces indentation recommended).
- Reference examples in `/samples/` for working setups.

---

## 🧾 Tips for Multi-Project Repositories

For monorepos or Unity packages, you can:
- Place separate configs in subdirectories.  
- Use `--config` flag to target each:
  ```
  autoversion bump minor --config ./modules/core/autoversion.json
  ```
- Add root-level workflow to iterate over all configs automatically.

---

## 📁 Related Files

- `/docs/QUICKSTART.md` – Setup instructions  
- `/docs/WORKFLOWS.md` – CI/CD integration  
- `/docs/UNITY.md` – Unity-specific usage  
- `/docs/FAQ.md` – Common troubleshooting  

---

**End of Config Reference**
