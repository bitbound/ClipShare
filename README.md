# ClipShare

A Blazor WebAssembly SPA app for sharing your clipboard across devices.

[![Build Status](https://dev.azure.com/translucency/ClipShare/_apis/build/status/Prod%20Deploy?branchName=master)](https://dev.azure.com/translucency/ClipShare/_build/latest?definitionId=20&branchName=master)

## Building

* Clone the repo: `git clone https://github.com/lucent-sea/ClipShare`.
* Build and run `ClipShare.Server` in Visual Studio 2019.
    * If using VS Code, open a terminal in the `ClipShare.Server` project folder.
    * Run `dotnet build`, then `dotnet run`.
* Site can be accessed at `https://localhost:5001`.

## Deploying
* In development environment, a built-in certificate is used for signing tokens that are used by the browser for authenticating/authorizing against the API endpoints.
* In production, you will need to supply a certiicate in `appsettings.json` or `appsettings.Production.json`.
    * This certificate can be self-signed.
* Official docs: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-3.1#deploy-to-production

Here's an example appsettings.Production.json file that uses the site's TLS/SSL certificate:
```
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "IdentityServer": {
        "Key": {
            "Type": "Store",
            "StoreName": "WebHosting",
            "StoreLocation": "LocalMachine",
            "Name": "CN=clipshare.example.com"
        }
    }
}
```

## Blazor Component Examples
I decided not to use any component libraries so I could learn how I might create them by hand.  Here are some examples to look at in the projects.

#### Toasts
I created a singleton service for showing toast notifications.  They are temporarily cached in the service and rendered by the component.

Files:
* ClipShare.Client/Services/ToastService.cs
* ClipShare.Client/Components/ToastHarness.cs
* ClipShare.Client/Models/Toast.cs.

#### Modals
Modals are used directly as components.  They have parameters for header, child content, and functions for when the modal is cancelled/okayed.

Files:
* ClipShare.Client/Components/Modal.razor.

#### Database Logger
The built-in logging for ASP.NET Core via the ILogger interface has logging providers for Debug, Console, and Windows Event Log.
To add file or database logging, you have to implement your own provider.

The Server project shows how to implement a logger that writes to an Entity Framework Core ApplicationDbContext.

Notice in the implemented DbLogger.Log method, `IServiceProvider.CreateScope` is used.  This is necessary to consume
the scoped `IDataService` (which depends on ApplicationDbContext) within a singleton service.

Files:
* ClipShare.Server/Services/DbLogger.cs.
* ClipShare.Server/Services.DbLoggerProvider.cs.