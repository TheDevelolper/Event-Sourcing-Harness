import { ModuleConfig, AppView } from "@modules-common";
import { AuthView } from "./Views/Auth";

export const moduleConfig: ModuleConfig = {
    views: {
        app: [
            { route: "/auth", component: AuthView } as AppView,
        ],
    },
};
