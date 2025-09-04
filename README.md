# BankStream


## Getting Started


## How I Built This Project: 

### Add An Aspire Project
```powershell 
> dotnet new install Aspire.ProjectTemplates
> dotnet new aspire-apphost -o BankStream.AppHost
> dotnet sln add BankStream.AppHost
> dotnet add .\BankStream.AppHost\BankStream.AppHost.csproj reference .\BankStream\BankStream.csproj
```
Add a service defaults which provides Open Telemetry: 

``` powershell
> dotnet new aspire-servicedefaults -o BankStream.ServiceDefaults
> dotnet sln add BankStream.ServiceDefaults
> dotnet add .\BankStream.AppHost\BankStream.AppHost.csproj reference .\BankStream.ServiceDefaults\BankStream.ServiceDefaults.csproj
```

Then add this to the apphost:
```csharp 
// apphost.cs
builder.AddServiceDefaults();
```

## References

[Telerik Blog - event sourcing aspnet core how store changes events](https://www.telerik.com/blogs/event-sourcing-aspnet-core-how-store-changes-events?ref=dailydev#creating-the-controller-class)

``