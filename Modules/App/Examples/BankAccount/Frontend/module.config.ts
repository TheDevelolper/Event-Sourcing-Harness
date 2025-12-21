import {BankView, ProtectedView } from "./Views/bank-view";

type AppView = {
  route: string
  component: React.ComponentType<any> // or more specific if you want
  auth?: boolean
}

export type ModuleConfig = {
  views: {
     app: AppView[]
  }
}

export const moduleConfig: ModuleConfig = {
  views: {
    app: [
      { route: "/bank", component: BankView } as AppView,
      { route: "/protected",  component: ProtectedView, auth: true } as AppView,
    ],
  },
};

