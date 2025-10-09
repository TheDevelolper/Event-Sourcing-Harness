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
          window.location.origin + window.location.pathname
        );
      }

      keycloak
        .init({
          onLoad: "check-sso",
          checkLoginIframe: false,
          // redirectUri: window.location.href,
          flow: "implicit",
        })
        .then(() => {
          setAuthenticated(keycloak.authenticated ?? false);
          setInitialized(true);
        })
        .catch((err: Error) => {
          console.error("Keycloak init failed:", err);
          setInitialized(true);
        });
    }
  }, []);

  if (!initialized) return <div></div>;

  return (
    <KeycloakContext.Provider value={keycloak}>
      {children}
    </KeycloakContext.Provider>
  );
};
