# 💼 GUMROAD — Licensing & Pro Preview

This guide outlines how **AutoVersion Pro** manages licensing and feature activation through Gumroad.

AutoVersion Pro extends the Lite edition with encrypted key verification, online license validation, and optional remote feature control.

---

## 🧩 Overview

AutoVersion Pro introduces advanced automation and publishing features:

| Feature | Description |
|----------|-------------|
| 🔐 Encrypted license key validation | Ensures valid purchases through Gumroad’s API |
| 🌐 Remote version locking | Restricts access to the latest authorized Pro build |
| ⚙️ Automatic Pro feature gating | Unlocks Pro-only tools and templates dynamically |
| 🪄 Webhook support *(future)* | Allows Discord or Slack notifications on license events |

These additions make AutoVersion Pro suitable for studios, CI pipelines, and enterprise distribution.

---

## 🧾 Example License JSON

Below is an example of a license payload returned from the Gumroad validation API.

```json
{
  "licenseKey": "XXXX-XXXX-XXXX-XXXX",
  "verified": true,
  "plan": "Pro",
  "features": ["multiProject", "customTemplates", "releaseWindow"],
  "expires": "2026-12-31T23:59:59Z"
}
```

Licenses are validated via a lightweight HTTPS request to Gumroad’s licensing endpoint.

If validation succeeds, Pro features are unlocked automatically within the CLI or Unity Editor.

## ⚙️ Integration Workflow

1. User enters license key in the Pro configuration window or CLI (autoversion license set).

2. AutoVersion sends a secure HTTPS POST to Gumroad’s validation API.

3. Response is cached locally (encrypted).

4. Pro-only components (e.g., Release Window, Custom Templates) are enabled.

## 🧠 Tips

- Keep your license key private — it is tied to your Gumroad account.

- Offline builds remain functional using the last validated license cache.

- When upgrading to a new major version, AutoVersion will re-check the license.

- For enterprise or CI environments, use environment variable:

```bash
AUTOVERSION_LICENSE=XXXX-XXXX-XXXX-XXXX
```

## 📁 Related Files

/docs/WORKFLOWS.md — CI/CD integration

/docs/CONFIG.md — Configuration schema reference

/docs/TEMPLATES.md — Pro-enabled templates

/docs/FAQ.md — Troubleshooting and contact info

**End of Gumroad Licensing Guide**