import { create } from "zustand";

export type Pages =
    | "Dashboard"
    | "MenuItems"
    | "Menus"
    | "Media";

export type AdminDashboardStoreState = {
  currentPage: Pages;
  setCurrentPage: (page: Pages) => void;
}
export const useAdminDashboardStore= create<AdminDashboardStoreState>((set) => ({
  currentPage: "Dashboard",
  setCurrentPage: (page: Pages) => set({ currentPage: page }),
}));
