"use client";

import { useState, useEffect } from "react";
import { Route, Routes } from "react-router-dom";
import { useAdminDashboardStore } from "../store/useAdminDashboardStore";

import {
  CalendarIcon,
  ChartPieIcon,
  DocumentDuplicateIcon,
  FolderIcon,
  HomeIcon,
  UsersIcon,
} from "@heroicons/react/24/outline";
import Sidebar from "./Admin/Sidebar";

import Dashboard from "./Admin/Dashboard";
import Team from "./Admin/Team";

const navigation = [
  { name: "Dashboard", href: "/admin", icon: HomeIcon },
  { name: "Team", href: "/admin/team", icon: UsersIcon },
];

function classNames(...classes) {
  return classes.filter(Boolean).join(" ");
}

export default function Admin() {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const { currentPage, setCurrentPage } = useAdminDashboardStore();

  useEffect(() => {
    const routes: Record<string, string> = {
      "/admin": "dashboard",
      "/admin/team": "team",
      "/admin/projects": "projects",
      "/admin/calendar": "calendar",
      "/admin/documents": "documents",
      "/admin/reports": "reports",
    };

    setCurrentPage(routes[location.pathname] ?? "dashboard");
  }, [location.pathname, setCurrentPage]);

  return (
    <div>
      <div className="flex w-full">
        <Sidebar navigation={navigation} currentPage={currentPage}></Sidebar>

        <Routes>
          <Route path="" element={<Dashboard />} />
          <Route path="team" element={<Team />} />
        </Routes>
      </div>
    </div>
  );
}
