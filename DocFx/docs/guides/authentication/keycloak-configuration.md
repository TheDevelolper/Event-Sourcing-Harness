## 🔐 Keycloak Configuration

### Overview
Keycloak configuration for this project is managed through a mounted JSON file that defines realm settings.  
This approach ensures consistency across environments and allows configuration changes to be tracked in version control.

> 🧠 **Note:**  
> The imported configuration does **not** include any pre-defined user accounts or client secrets for security reasons.  
> You'll need to manually set these up after the realm has been imported.

Please follow the guide below for detailed steps:

👉 [Creating User Accounts](./authentication-client-secret-setup.md)

---

### 📄 Configuration File

The Keycloak realm configuration is defined in:

`Backend/Features/Authentication/SaasFactory.Authentication/Mounts/Keycloak/data/import/realm-export.json`

When Aspire launches, this file is automatically loaded into Keycloak, initializing the realm configuration.  
You can modify realm settings in **two ways**:

###  Modify via Keycloak Admin Console

You can modify realm settings using the Keycloak Admin Console:

`https://localhost:8443/admin/master/console/`

After making changes through the UI, **export the updated realm configuration** and overwrite the existing file in your project so that changes can be committed to Git.

---

> [!IMPORTANT]
> Do **not** commit any sensitive information—such as user credentials, secrets, or environment-specific configuration—to Git.  
> Always use your environment's **secret management system** (or Environment Variables) to handle sensitive data securely.

---
### 🧑🏽‍💻 Changing the Realm Name

Try to avoid renaming the realm itself as it's not required. You can simply change the display name in the Keycloak UI.
> To do this go to Realm Settings > Display Name

### 👤 Setting Up User Accounts

For security reasons, the default realm import **does not include any user accounts**.  
You'll need to manually create user accounts (for example, a developer or admin account) in the **Keycloak Admin Console** after the realm is initialized.

Please follow the guide below for detailed steps:

👉 [Creating User Accounts](./authentication-client-secret-setup.md)

---

### 🔑 Setting the Client Secret

The client secret is not stored in `realm-export.json` and must be configured manually.  
Please follow the guide below for detailed steps:

👉 [Setting up the Authentication Client Secret](./authentication-client-secret-setup.md)

---
