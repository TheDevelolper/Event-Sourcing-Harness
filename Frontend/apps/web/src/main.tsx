import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { KeycloakProvider } from "./auth/keycloak-provider";
import { BrowserRouter } from "react-router-dom";
import "../../../libs/ui-components/styles/tailwind.css";
import "./index.css";

import App from "./App";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <KeycloakProvider>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </KeycloakProvider>
  </StrictMode>
);
