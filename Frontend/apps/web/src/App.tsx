import { useContext } from "react";
import { KeycloakContext } from "./auth/keycloak-provider";
import { ProtectedRoute } from "./auth/protected-route";
import Admin from "./Pages/Admin";
import Home from "./Pages/Home";

import "./App.css";

function App() {
  const keycloak = useContext(KeycloakContext);

  if (!keycloak) return null;

  return (
    <div>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route
          path="/admin/*"
          element={
            <ProtectedRoute>
              <Admin />
            </ProtectedRoute>
          }
        />
      </Routes>
    </div>
  );
}

export default App;
