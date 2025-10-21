"use client";
import {
  HomeIcon,
  ClockIcon,
  EllipsisHorizontalIcon,
  BookOpenIcon,
  ShoppingBagIcon,
  TrophyIcon
} from "@heroicons/react/24/outline";
import { usePathname } from "next/navigation";
import { useCapacitorRouter } from "./CapacitorRouter";
import { isCapacitor } from "@/utils/navigation";

const Navbar = () => {
  // Get pathname from Next.js for web and currentPath from CapacitorRouter for Capacitor
  const nextPathname = usePathname();
  const { navigateTo, currentPath } = useCapacitorRouter();
  
  // Use the appropriate path based on environment
  const isCapacitorApp = isCapacitor();
  const pathname = isCapacitorApp ? currentPath : nextPathname;
  
  
  // Helper function to check if a path matches, handling trailing slashes and index paths
  const isPathActive = (path: string): boolean => {
    if (!pathname) return false;
    
    // Normalize paths by removing trailing slashes
    const normalizedCurrentPath = pathname.endsWith('/') ? pathname.slice(0, -1) : pathname;
    const normalizedPath = path.endsWith('/') ? path.slice(0, -1) : path;
    
    // Check for exact match or index path match
    const isActive = normalizedCurrentPath === normalizedPath || 
           normalizedCurrentPath === `${normalizedPath}/index` ||
           (normalizedPath === '' && (normalizedCurrentPath === '' || normalizedCurrentPath === '/'));
    
    return isActive;
  };
  
  const handleNavigate = (path: string) => {
    navigateTo(path);
  };

  return (
    <nav className="fixed bottom-0 left-0 w-full bg-white dark:bg-zinc-900 shadow-md rounded-t-3xl z-50">
      <div className="flex justify-between items-center max-w-md mx-auto px-6 py-4 pb-[calc(0.5rem+env(safe-area-inset-bottom,0))]">


        <NavItem
          icon={<HomeIcon className="w-6 h-6" />}
          label="Home"
          active={isPathActive("/")}
          onClick={() => handleNavigate("/")}
        />
        <NavItem
          icon={<BookOpenIcon className="w-6 h-6" />}
          label="Wissen"
          active={isPathActive("/knowledge")}
          onClick={() => handleNavigate("/knowledge")}
        />
        <NavItem
          icon={<TrophyIcon className="w-6 h-6" />}
          label="Bestenliste"
          active={isPathActive("/leaderboard")}
          onClick={() => handleNavigate("/leaderboard")}
        />
        <NavItem
          icon={<ShoppingBagIcon className="w-6 h-6" />}
          label="Geräte"
          active={isPathActive("/dropshipping")}
          onClick={() => handleNavigate("/dropshipping")}
          />
        <NavItem
          icon={<EllipsisHorizontalIcon className="w-6 h-6" />}
          label="Mehr"
          onClick={() => handleNavigate("/settings")}
          active={isPathActive("/settings")}
        />
      </div>
    </nav>
  );
};

const NavItem = ({
  icon,
  label,
  active = false,
  onClick,
}: {
  icon: React.ReactNode;
  label: string;
  active?: boolean;
  onClick?: () => void;
}) => {
  return (
    <button
      onClick={onClick}
      className="flex flex-col items-center justify-center text-xs focus:outline-none w-16"
    >
      <div className={`text-gray-600 dark:text-gray-300 ${active ? "text-indigo-600 dark:text-indigo-400" : ""}`}>
        {icon}
      </div>
      <span className={`mt-1 truncate ${active ? "font-semibold text-indigo-600 dark:text-indigo-400" : "text-gray-500 dark:text-gray-400"}`}>
        {label}
      </span>
    </button>
  );
};

export default Navbar;
