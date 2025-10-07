import { Routes, Route, Link } from "react-router-dom";
import { ProtectedRoute } from "./auth/protexted-route";
import Admin from "./Pages/Admin";
import Home from "./Pages/Home";

import "./App.css";

function App() {
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
