import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "https://localhost:8443",
  realm: "saas-realm",
  clientId: "saas-dashboard",
});

/**
 * Decode a JWT token
 * @param {string} token - JWT token string
 * @returns {object} Decoded payload
 */
export function decodeToken(token: string) {
  if (!token) return null;
  const base64Url = token.split(".")[1];
  const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
  const jsonPayload = decodeURIComponent(
    atob(base64)
      .split("")
      .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
      .join("")
  );
  return JSON.parse(jsonPayload);
}

/**
 * Get user data from the current Keycloak token
 * @returns {object|null} User info from token
 */
export function getTokenData() {
  // Use the ID token for user info
  return decodeToken(keycloak.idToken);
}

export default keycloak;
