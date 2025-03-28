"use client";

import { usePathname } from "next/navigation";
import Navbar from "@/components/navbar";

export function LayoutShell({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const hideNavbar = ["/login", "/register"].includes(pathname);

  return (
    <div className="relative flex flex-col h-screen">
      <main className="container bg-stone-100 dark:bg-black mx-auto max-w-7xl pt-8 px-6 flex-grow">
        {children}
      </main>
      {!hideNavbar && <Navbar />}
    </div>
  );
}
