import { useContext, Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { KeycloakContext } from "./auth/keycloak-provider";
import { ProtectedRoute } from "./auth/protected-route";
import Admin from "./Pages/Admin";
import Home from "./Pages/Home";
import { ModuleConfig } from "@modules-common";
import "./App.css";


type DynamicRoute = {
  path: string;
  Component: React.ComponentType<any>;
  auth: boolean
};

// moduleConfig (outside project, via alias)
// @ts-ignore
const moduleConfig: Record<string, { moduleConfig:ModuleConfig }> = import.meta.glob(
  "@modules/**/module.config.ts",
  { eager: true }
);

// ALL possible views (same alias, same root)
const dynamicRoutes: DynamicRoute[] = [];

for(const [_configPath, config] of Object.entries<{ moduleConfig:ModuleConfig }>(moduleConfig)) {
   for(const appView of config.moduleConfig.views.app) {
    dynamicRoutes.push({ 
      path: appView.route,
      Component: appView.component,
      auth: appView.auth ?? false
    })
   }
}

/* 
  =========================
   App
  ========================= 
*/

function App() {
  const keycloak = useContext(KeycloakContext);
  if (!keycloak) return null;

  return (
    <Suspense fallback={<div>Loading…</div>}>
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

        {dynamicRoutes.map(({ path, Component, auth }) => (
          <Route
            key={path}
            path={`/${path}`}
            element={(auth ? <ProtectedRoute><Component/></ProtectedRoute>: <Component />)}
          />
        ))}

      </Routes>
    </Suspense>
  );
}

export default App;
