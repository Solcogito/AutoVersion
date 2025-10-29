# 🧾 TEMPLATES — AutoVersion Lite

This document provides ready-to-use text templates for:
- CHANGELOG sections  
- GitHub release notes  
- Gumroad updates  
- Social posts (Twitter, Discord, Reddit)  

They are designed for both **manual use** and **automated generation** via future AutoVersion Pro features.

---

## 🧱 1. CHANGELOG Templates

### 🟦 Default (Lite)
Used by default for all releases generated through the CLI.

```
## [${version}] – ${date}

### Added
- New features or functionality.

### Changed
- Updated or improved existing systems.

### Fixed
- Bug fixes and stability improvements.

### Removed
- Deprecated or removed features.
```

---

### 🟩 Extended (Pro)
A more expressive template, used when richer formatting is enabled.

```
## 🚀 Release ${version} — ${codename}
**Date:** ${date}

### ✨ Highlights
- ${summary}

### 🔧 Technical
${changes_list}

### 🧩 Integration Notes
${integration_notes}

### 🧠 Contributors
${contributors}

---
**Release checksum:** ${commit_hash}
```

---

### 🟨 Compact (CI Logs)
Minimal format for console or CI output.

```
[v${version}] ${date} — ${summary}
```

---

## 📦 2. GitHub Release Notes Templates

These are used for `release-on-tag.yml` automation.

### 🟦 Standard
```
# 🚀 AutoVersion Lite ${version}

**Date:** ${date}

### Highlights
${summary}

### Commits
${commit_list}

---
🔧 Generated automatically via AutoVersion Lite.
```

---

### 🟩 With Changelog Embed
```
# AutoVersion Lite ${version}

Below is the changelog extracted from CHANGELOG.md:

${changelog_content}

---
✅ Generated and released via AutoVersion Lite CI.
```

---

### 🟥 For Pre-Releases
```
# ⚠️ Pre-Release ${version}

This version is intended for testing only.
Expect breaking changes and incomplete documentation.

### Key Additions
${summary}

---
🧪 Released as an early preview — feedback welcome!
```

---

## 🪄 3. Gumroad Product Update Templates

Used when publishing a new version on Gumroad.  
You can copy these directly into the product’s “Update” tab.

### 🟦 New Version Announcement
```
🚀 AutoVersion Lite ${version} is live!

What’s new:
${summary}

Highlights:
- Improved stability and CLI feedback
- Smarter changelog generation
- Unity Editor menu polish

Download the latest build here:
${gumroad_download_link}

Thank you for supporting AutoVersion Lite!
```

---

### 🟩 First Release Announcement
```
✨ Introducing AutoVersion Lite!

Automate your Unity and .NET versioning, changelogs, and Git tagging —
all in one lightweight open-source tool.

🎮 Unity Editor integration  
⚙️ CLI automation  
📄 CHANGELOG generation  

Get it free on Gumroad:
${gumroad_download_link}
```

---

### 🟨 Pro Version Teaser
```
💡 Coming Soon — AutoVersion Pro

Everything you love about Lite, with:
- Multi-project support
- Custom changelog templates
- GUI “Release Window”
- Webhooks (Discord / Slack / Gumroad)

Stay tuned for updates!
```

---

## 📣 4. Social Media Templates

### 🟦 Twitter / X
```
🚀 AutoVersion Lite v${version} released!

✅ SemVer automation
✅ CHANGELOG generation
✅ Unity Editor integration

Free download:
${github_release_url}

#gamedev #unity3d #dotnet #automation
```

---

### 🟩 Discord / Reddit
```
🎉 New Release — AutoVersion Lite v${version}

Features:
• One-click version bump
• Auto-generated changelog
• Unity Editor integration

Download: ${github_release_url}
Docs: https://github.com/solcogito/AutoVersion/tree/main/docs

Let me know what you think — feedback welcome!
```

---

### 🟥 DevLog Format
```
🧰 DevLog #${devlog_number}
✅ Finished implementing version bump logic
✅ CHANGELOG generator now groups commits by type
🧩 Next: Git tag automation + Unity Editor window

#devlog #unity #dotnet #opensource
```

---

## 🔄 5. Email Update Template (Optional)

```
Subject: [Update] AutoVersion Lite v${version} Released

Hi everyone,

Version ${version} of AutoVersion Lite is now available!

Here’s what’s new:
${summary}

You can download the update here:
${gumroad_download_link}

Thanks for your continued support,  
— The Solcogito Team
```

---

## 🧰 6. Variables Reference

| Variable | Description |
|-----------|--------------|
| `${version}` | Version number (e.g. 1.0.0) |
| `${date}` | Release date (YYYY-MM-DD) |
| `${summary}` | Short summary of major changes |
| `${changelog_content}` | Extracted Markdown from CHANGELOG.md |
| `${commit_list}` | List of commits since last tag |
| `${gumroad_download_link}` | URL to Gumroad download page |
| `${github_release_url}` | URL to GitHub Release |
| `${codename}` | Optional release codename |
| `${contributors}` | Author names from commit history |
| `${integration_notes}` | Extra notes for Unity or CI |
| `${commit_hash}` | Latest Git commit SHA |

---

## 🧠 7. Best Practices

- Keep summaries under **10 lines** for quick readability.  
- Always include one emoji in social posts — boosts engagement.  
- Use versioned image banners (e.g., `v1.0.0_cover.png`) in Gumroad updates.  
- Combine AutoVersion with GitHub Actions for fully automated posting.  
- Keep CHANGELOG entries human-readable — avoid commit spam.  

---

## 📁 Related Files

- `/docs/CHANGELOG.md` — actual generated log  
- `/docs/WORKFLOWS.md` — release automation details  
- `/docs/QUICKSTART.md` — setup & build instructions  
- `/docs/UNITY.md` — Unity Editor integration guide  

---

**End of Templates Guide**
