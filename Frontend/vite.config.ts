import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import tailwindcss from "@tailwindcss/vite";
import tsconfigPaths from "vite-tsconfig-paths";
import path from "path";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss(), tsconfigPaths()],
  resolve: {
    alias: {
      "@micro-frontends": path.resolve(__dirname, "libs/micro-frontends/src"),
      "@ui-models": path.resolve(__dirname, "libs/ui-models/src"),
      "@ui-components": path.resolve(__dirname, "libs/ui-components/src"),
    },
  },
});
