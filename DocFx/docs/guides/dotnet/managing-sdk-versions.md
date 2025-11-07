## 🛠️ Controlling .NET SDK Versions

This guide explains how to control .NET SDK versions, in this multi-project solution.

---

The **`global.json`** file in your repo root ensures all projects use a specific SDK version.

```
{
  "sdk": {
    "version": "9.0.305",
    "rollForward": "disable",
    "allowPrerelease": false
  }
}
```

### Key Options

- `version`: Pin the exact SDK version to use.
- `rollForward`:
  - `disable`: Only allow the exact version.
  - `latestPatch`: Allow newer patch versions.
  - `major`: Allow higher major versions if necessary.
- `allowPrerelease`: `true` to allow prerelease SDKs, `false` to disallow.

---