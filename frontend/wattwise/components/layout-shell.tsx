"use client";

import { usePathname } from "next/navigation";
import Navbar from "@/components/navbar";
import { useUIState } from "@/context/UIStateContext";

export function LayoutShell({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const { isModalOpen } = useUIState();
  
  // Check if the current path is login or register or if a modal is open
  const hideNavbar = 
    pathname === "/login" || 
    pathname?.startsWith("/login/") || 
    pathname === "/register" || 
    pathname?.startsWith("/register/") ||
    isModalOpen;
    
  // Debug log
  console.log("Layout shell - Modal open:", isModalOpen, "Hide navbar:", hideNavbar);

  return (
    <div className="relative flex flex-col h-screen overflow-hidden bg-white dark:bg-black">
      <div className="w-full h-[env(safe-area-inset-top,0)] bg-white dark:bg-black"></div>
      <main className="container mx-auto max-w-md px-4 flex-grow overflow-hidden pt-[calc(1rem+env(safe-area-inset-top,0))]">
        <div className={`h-full ${isModalOpen ? '' : 'overflow-y-auto'} ${hideNavbar ? 'pb-[env(safe-area-inset-bottom,0)]' : 'pb-[calc(5rem+env(safe-area-inset-bottom,0))]'}`}>
          <div className="pt-2">
            {children}
          </div>
        </div>
      </main>
      {!hideNavbar && <Navbar />}
    </div>
  );
}
