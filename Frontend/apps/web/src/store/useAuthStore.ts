// src/store/useAdminDashboardStore.js
import { create } from "zustand";
import { decodeToken } from "../auth/authentication-service";

// Define the Zustand store type
export interface AuthStore {
  user: UserData | null;
  setUser: (token: string) => void;
  clearUser: () => void;
}

// Define the shape of the decoded token
export interface UserData {
  sub: string;
  preferred_username?: string;
  email?: string;
  given_name?: string;
  family_name?: string;
  realm_access?: {
    roles: string[];
  };
  [key: string]: any; // For any additional claims
}

export const useAuthStore = create<AuthStore>((set) => ({
  user: null,
  setUser: (token: string) => set({ user: decodeToken(token) }),
  clearUser: () => set({ user: null }),
}));

