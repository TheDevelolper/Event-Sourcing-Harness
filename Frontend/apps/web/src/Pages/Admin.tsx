"use client";

import { useState } from "react";
import { Route, Routes } from "react-router-dom";

import {
  CalendarIcon,
  ChartPieIcon,
  DocumentDuplicateIcon,
  FolderIcon,
  HomeIcon,
  UsersIcon,
} from "@heroicons/react/24/outline";
import Sidebar from "./Admin/Sidebar";

import Hello from "./Admin/Dashboard";
import World from "./Admin/World";

const navigation = [
  { name: "Dashboard", href: "/admin", icon: HomeIcon, current: true },
  { name: "Team", href: "/admin/world", icon: UsersIcon, current: false },
  { name: "Projects", href: "#", icon: FolderIcon, current: false },
  { name: "Calendar", href: "#", icon: CalendarIcon, current: false },
  { name: "Documents", href: "#", icon: DocumentDuplicateIcon, current: false },
  { name: "Reports", href: "#", icon: ChartPieIcon, current: false },
];
const teams = [
  { id: 1, name: "Heroicons", href: "#", initial: "H", current: false },
  { id: 2, name: "Tailwind Labs", href: "#", initial: "T", current: false },
  { id: 3, name: "Workcation", href: "#", initial: "W", current: false },
];

function classNames(...classes) {
  return classes.filter(Boolean).join(" ");
}

export default function Admin() {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div>
      <div className="flex w-full">
        <Sidebar navigation={navigation}></Sidebar>

        <Routes>
          <Route path="" element={<Hello />} />
          <Route path="world" element={<World />} />
        </Routes>
      </div>
    </div>
  );
}
