import { useContext } from "react";
import { useAuthStore } from "../store/useAuthStore";
import { KeycloakContext } from "./keycloak-provider";

export function ProtectedRoute({ children }) {
  const keycloak = useContext(KeycloakContext);

  if (!keycloak) return <p>Loading...</p>;

  // If not logged in, redirect to login
  if (!keycloak.authenticated) {
    keycloak.login();
    return <p>Redirecting to login...</p>;
  }

  // Authenticated → update store and render the protected content
  if(keycloak.idToken) {
      useAuthStore.getState().setUserToken(keycloak.idToken);
  }
  
  return children;
}
