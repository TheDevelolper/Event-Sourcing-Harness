# .NET SDK Versions, Global Build Properties, and Package Management

This guide explains how to control .NET SDK versions, global build properties, and package versions in a multi-project solution.

---

## 1. Controlling .NET SDK Version

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

## 2. Global Build Properties

You can define properties for all projects in a solution using **`Directory.Build.props`**:

```
<Project>
  <PropertyGroup>
    <!-- Target framework for all projects -->
    <TargetFramework>net9.0</TargetFramework>
    
    <!-- Fallback for older frameworks -->
    <AssetTargetFallback>net8.0;net7.0;net6.0</AssetTargetFallback>
    
    <!-- Optional restore flags -->
    <RestoreIgnoreFailedSources>true</RestoreIgnoreFailedSources>
    <RestoreNoCache>true</RestoreNoCache>
  </PropertyGroup>
</Project>
```

### Benefits

- Avoid repeating the target framework in every project.
- Set global restore and build behaviors.
- Define fallback targets for compatibility with older packages.

---

## 3. Central Package Management

Instead of specifying versions in each `.csproj`, you can centrally manage package versions using **`Directory.Packages.props`**:

```
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

  <ItemGroup>
    <!-- Common packages with pinned versions -->
    <PackageVersion Include="Aspire.Hosting.AppHost" Version="9.4.2" />
    <PackageVersion Include="Microsoft.Extensions.Http.Resilience" Version="9.7.0" />
    <PackageVersion Include="OpenTelemetry.Extensions.Hosting" Version="1.12.0" />
  </ItemGroup>
</Project>
```

### How to Use in Projects

In `.csproj` files, omit the `Version`:

```
<ItemGroup>
  <PackageReference Include="Aspire.Hosting.AppHost" />
  <PackageReference Include="OpenTelemetry.Extensions.Hosting" />
</ItemGroup>
```

### Benefits

- Ensures consistent package versions across the solution.
- Simplifies upgrades: just update `Directory.Packages.props`.
- Reduces version conflicts and drift.

---

## 4. Best Practices

1. **Always pin SDK version** in `global.json` for CI/CD consistency.
2. **Use `Directory.Build.props`** for target frameworks and shared build properties.
3. **Use `Directory.Packages.props`** to centralize package versions.
4. **Keep prerelease versions minimal** unless needed for testing.
5. **Use `AssetTargetFallback`** carefully to allow compatibility with older packages.

---

> Centralized version control in .NET ensures that your builds are reproducible, predictable, and easier to maintain across multiple projects.
