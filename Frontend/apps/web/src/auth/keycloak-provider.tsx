import React, { useEffect, useState, ReactNode } from "react";
import keycloak from "./authentication-service";

interface KeycloakProviderProps {
  children: ReactNode;
}

export const KeycloakContext = React.createContext<typeof keycloak | null>(
  null
);

export const KeycloakProvider = ({ children }: KeycloakProviderProps) => {
  const [initialized, setInitialized] = useState(false);
  const [authenticated, setAuthenticated] = useState(false);

  useEffect(() => {
    if (!(window as any).keycloakInitialized) {
      (window as any).keycloakInitialized = true;

      // If returning from Keycloak with code, reload without query to finalize auth
      if (window.location.search.includes("code=")) {
        window.history.replaceState(
          {},
          document.title,
          window.location.origin + "/"
        );
      }

      keycloak
        .init({
          onLoad: "login-required",
          checkLoginIframe: false,
          redirectUri: window.location.origin + "/",
          flow: "implicit",
        })
        .then((auth) => {
          console.log("Keycloak auth:", auth, keycloak.authenticated);
          setAuthenticated(keycloak.authenticated ?? false);
          setInitialized(true);
        })
        .catch((err) => {
          console.error("Keycloak init failed:", err);
          setInitialized(true);
        });
    }
  }, []);

  if (!initialized) return <div>Loading authentication...</div>;
  if (!authenticated) return <div>Not authenticated</div>;

  return (
    <KeycloakContext.Provider value={keycloak}>
      {children}
    </KeycloakContext.Provider>
  );
};
