import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { KeycloakProvider } from "./auth/keycloak-provider";
import "../../../libs/ui-components/styles/tailwind.css";
import "./index.css";

import App from "./App";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <KeycloakProvider>
      <App />
    </KeycloakProvider>
  </StrictMode>
);
