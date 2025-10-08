"use client";

import { useEffect, useState } from "react";
import { Route, Routes } from "react-router-dom";

import { useAdminDashboardStore } from "../store/useAdminDashboardStore";
import { useAuthStore } from "../store/useAuthStore";

import { HomeIcon, UsersIcon } from "@heroicons/react/24/outline";
import Sidebar from "./Admin/Sidebar";

import Dashboard from "./Admin/Dashboard";
import Media from "./Admin/Media";
import MenuItems from "./Admin/MenuItems";
import Menus from "./Admin/Menus";

const navigation = [
  { name: "Dashboard", href: "/admin", icon: HomeIcon },
  { name: "Menu Items", href: "/admin/menu-items", icon: UsersIcon },
  { name: "Menus", href: "/admin/menus", icon: UsersIcon },
  { name: "Media", href: "/admin/media", icon: UsersIcon },
];

export default function Admin() {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const { currentPage, setCurrentPage } = useAdminDashboardStore();
  const { user } = useAuthStore();

  useEffect(() => {
    const routes: Record<string, string> = {
      "/admin": "dashboard",
      "/admin/menu-items": "menu items",
      "/admin/menus": "menu",
      "/admin/media": "media",
    };

    setCurrentPage(routes[location.pathname] ?? "dashboard");
  }, [location.pathname, setCurrentPage]);

  return (
    <div>
      <div className="flex w-full">
        <Sidebar
          navigation={navigation}
          user={user}
          currentPage={currentPage}
        ></Sidebar>

        <Routes>
          <Route path="" element={<Dashboard />} />
          <Route path="menu-items" element={<MenuItems />} />
          <Route path="menus" element={<Menus />} />
          <Route path="media" element={<Media />} />
        </Routes>
      </div>
    </div>
  );
}
