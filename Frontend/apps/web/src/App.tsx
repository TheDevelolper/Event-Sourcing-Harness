import { Routes, Route, Link } from "react-router-dom";
import { ProtectedRoute } from "./auth/protexted-route";
import Admin from "./Pages/Admin";
import Home from "./Pages/Home";

import "./App.css";

function App() {
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
