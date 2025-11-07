## 👷🏽‍♂️ Managing .NET Build Properties

This guide explains how to control .NET Global build properties, in this multi-project solution.

---

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