import { useContext } from "react";
import { Navigate } from "react-router-dom";
import { KeycloakContext } from "./keycloak-provider";

export function ProtectedRoute({ children }) {
  const keycloak = useContext(KeycloakContext);

  if (!keycloak) return <p>Loading...</p>;

  // If not logged in, redirect to login
  if (!keycloak.authenticated) {
    keycloak.login();
    return <p>Redirecting to login...</p>;
  }

  // Authenticated → render the protected content
  return children;
}
