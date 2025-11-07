## 📦 .NET Central Package Management

This guide explains how to control package versions in this multi-project solution.

---

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
