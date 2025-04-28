"use client";
import {
  HomeIcon,
  DevicePhoneMobileIcon,
  GiftIcon,
  EllipsisHorizontalIcon,
  BookOpenIcon
} from "@heroicons/react/24/outline";
import { usePathname, useRouter } from "next/navigation";

const Navbar = () => {
  const pathname = usePathname();
  const router = useRouter();

  const handleNavigate = (path: string) => {
    router.push(path);
  };

  return (
    <nav className="fixed bottom-0 left-0 w-full bg-white dark:bg-zinc-900 shadow-md rounded-t-3xl z-50 px-12 py-4">
      <div className="flex justify-between items-center">
        <NavItem
          icon={<HomeIcon className="w-6 h-6" />}
          label="Home"
          active={pathname === "/"}
          onClick={() => handleNavigate("/")}
        />
        <NavItem
          icon={<BookOpenIcon className="w-6 h-6" />}
          label="Wissen"
          active={pathname === "/knowledge"}
          onClick={() => handleNavigate("/knowledge")}
        />
        <NavItem
          icon={<GiftIcon className="w-6 h-6" />}
          label="Gutscheine"
        //   onClick={() => handleNavigate("/gutscheine")}
        />
        <NavItem
          icon={<EllipsisHorizontalIcon className="w-6 h-6" />}
          label="Mehr"
          onClick={() => handleNavigate("/settings")}
          active={pathname === "/settings"}
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
      className="flex flex-col items-center text-xs focus:outline-none"
    >
      <div className={`text-gray-600 dark:text-gray-300 ${active ? "text-indigo-600 dark:text-indigo-400" : ""}`}>
        {icon}
      </div>
      <span className={`mt-1 ${active ? "font-semibold text-indigo-600 dark:text-indigo-400" : "text-gray-500 dark:text-gray-400"}`}>
        {label}
      </span>
    </button>
  );
};

export default Navbar;
