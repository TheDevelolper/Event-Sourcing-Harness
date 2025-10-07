import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "https://localhost:8443",
  realm: "Menuota",
  clientId: "menu-management",
});

export default keycloak;
