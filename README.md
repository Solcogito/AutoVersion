# AutoVersion Lite

[![Build](https://github.com/Solcogito/AutoVersion/actions/workflows/ci.yml/badge.svg)](https://github.com/Solcogito/AutoVersion/actions/workflows/ci.yml)
[![Release](https://img.shields.io/github/v/release/Solcogito/AutoVersion)](https://github.com/Solcogito/AutoVersion/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Downloads](https://img.shields.io/github/downloads/Solcogito/AutoVersion/total)](https://github.com/Solcogito/AutoVersion/releases)


**AutoVersion Lite** is a lightweight, deterministic semantic-versioning CLI tool built with .NET 8.  
It provides simple, predictable version bumping with zero configuration and cross-platform support.

---

## 🚀 Features

- Semantic Versioning (SemVer 2.0.0)
- Version bumping: **major**, **minor**, **patch**, **prerelease**
- Safe write operations with `--dry-run`
- Reads from `version.json` and/or `version.txt`
- Automatically selects the **highest** version
- Fully cross-platform (.NET 8)

AutoVersion Lite does **not** include configuration files, changelog automation, Git tagging, Unity integration, or multi-file version updates.

---

## 📦 Installation

Requirements:
- .NET 8 SDK  
- PowerShell 7+ or Bash

Clone and build:
```bash
git clone https://github.com/Solcogito/AutoVersion.git
cd AutoVersion
pwsh _Infrastructure/build.ps1
```

## 🧭 Usage

Check current version:
```bash
autoversion current
```

Set a version:
```bash
autoversion set 1.2.3
```

Bump versions:
```bash
autoversion bump patch
autoversion bump minor
autoversion bump major
```

Prerelease example:
```bash
autoversion bump pre -p alpha
```

Dry-run:
```bash
autoversion bump patch --dry-run
```
## 📚 Documentation

See the /docs folder for:

- Quickstart

- CLI reference

- Architecture overview

- FAQ

## 📄 License
```
MIT License
© 2025 Solcogito S.E.N.C.
```
---