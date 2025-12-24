import { ModuleConfig, AppView } from "@modules-common";
import { BankView, ProtectedView } from "./Views/bank-view";

export const moduleConfig: ModuleConfig = {
  views: {
    app: [
      { route: "/bank", component: BankView } as AppView,
      { route: "/protected",  component: ProtectedView, auth: true } as AppView,
    ],
  },
};
