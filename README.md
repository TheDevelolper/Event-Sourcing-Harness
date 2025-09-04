# SaasFactory

This is just a quick example of how to use Event Sourcing without DDD. I just wanted something simple where I can try 
out different transport layers and tooling. 

## Getting Started

Run the Aspire Host Application, this will start the project and any required containers. 

There's a .http file in the ./Http diectory, use it to deposit money into an account
Then go to: `api/accounts/<account id>/balance` to view the balance of that account. 

If you wish to deposit more money into that account then ensure you change the transaction ID in the http file.

## Notes and Scribbles: 

### Add An Aspire Project
```powershell 
> dotnet new install Aspire.ProjectTemplates
> dotnet new aspire-apphost -o SaasFactory.AppHost
> dotnet sln add SaasFactory.AppHost
> dotnet add .\SaasFactory.AppHost\SaasFactory.AppHost.csproj reference .\SaasFactory\SaasFactory.csproj
```
Add a service defaults which provides Open Telemetry: 

``` powershell
> dotnet new aspire-servicedefaults -o SaasFactory.ServiceDefaults
> dotnet sln add SaasFactory.ServiceDefaults
> dotnet add .\SaasFactory.AppHost\SaasFactory.AppHost.csproj reference .\SaasFactory.ServiceDefaults\SaasFactory.ServiceDefaults.csproj
```

Then add this to the apphost:
```csharp 
// apphost.cs
builder.AddServiceDefaults();
```

## References

[Telerik Blog - event sourcing aspnet core how store changes events](https://www.telerik.com/blogs/event-sourcing-aspnet-core-how-store-changes-events?ref=dailydev#creating-the-controller-class)
