## 🔐 Authentication Client Secret Setup

This document explains how to configure the **authentication client secret** for the application.  

The app expects to read the client secret from an environment variable on the host machine.

---

### 1. Add the configuration setting

In your shared configuration file `Backend/Shared/SaasFactory.Shared.Config/appsettings.json`
add the following section under the root:

``` pwsh
"Authentication": {
  "ClientSecretEnvironmentVar": "AUTH_CLIENT_SECRET"
}
```

This setting tells the application which environment variable contains the Keycloak client secret.

---

### 2. Set the environment variable on the host system

Create a new environment variable named `AUTH_CLIENT_SECRET` or whatever you've defined in the app settings.
and assign it the **client secret** from Keycloak.

You can find this secret in **Keycloak** under:

> Clients → _Your Client (e.g. `saas-dashboard`)_ → **Credentials (Tab)** → Client Secret

#### 🪟 PowerShell (permanent for current user)
``` pwsh
[System.Environment]::SetEnvironmentVariable('AUTH_CLIENT_SECRET', 'paste-your-keycloak-client-secret-here', 'User')
```

#### 🧑‍💻 Bash / Linux (permanent)
Put the client secret in your profile startup (e.g. .bashrc or .zshrc)
``` bash
export AUTH_CLIENT_SECRET="paste-your-keycloak-client-secret-here"
```

> 💡 Restart your IDE (e.g. Rider or VS Code) after setting the variable  
> so it inherits the new environment variable.

---

### 3. Verification

You can verify the variable is visible by running:

#### PowerShell
``` pwsh
Get-ChildItem Env:AUTH_CLIENT_SECRET
```

#### Bash
``` bash
echo $AUTH_CLIENT_SECRET
```

> If you are still getting authentication errors please follow the [Keycloak Setup Guide](keycloak-configuration.md)