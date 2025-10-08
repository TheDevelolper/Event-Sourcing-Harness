// src/store/useAdminDashboardStore.js
import { create } from "zustand";

export const useAdminDashboardStore = create((set) => ({
  currentPage: "Dashboard | MenuItems | Menus | Media",

  // Actions
  setCurrentPage: (page: string) => set({ currentPage: page }),
}));
