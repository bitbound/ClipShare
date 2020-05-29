# ClipShare

A Blazor WebAssembly app for sharing your clipboard across devices.


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