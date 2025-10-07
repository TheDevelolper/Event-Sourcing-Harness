import { useContext } from "react";
import { KeycloakContext } from "./auth/keycloak-provider";

import "./App.css";
import { RestaurantMenu } from "@micro-frontends";

function App() {
  const keycloak = useContext(KeycloakContext);

  if (!keycloak) return null;

  return (
    <div>
      <RestaurantMenu></RestaurantMenu>
    </div>
  );
}

export default App;
