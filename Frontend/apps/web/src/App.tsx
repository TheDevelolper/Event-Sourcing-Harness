import { useContext } from "react";
import { KeycloakContext } from "./auth/keycloak-provider";
import { Routes, Route, Link } from "react-router-dom";
import { ProtectedRoute } from "./auth/protexted-route";
import Admin from "./Pages/Admin";
import Home from "./Pages/Home";

import "./App.css";


function App() {
  const keycloak = useContext(KeycloakContext);

  if (!keycloak) return null;


     
  return (
    <div>
      <nav>
        <Link to="/">Home</Link> | <Link to="/admin">Admin</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route
          path="/admin"
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
