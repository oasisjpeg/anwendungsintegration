"use client";

import type { ThemeProviderProps } from "next-themes";

import * as React from "react";
import { useEffect } from "react";
import { HeroUIProvider } from "@heroui/system";
import { useRouter } from "next/navigation";
import { ThemeProvider as NextThemesProvider } from "next-themes";
import {ToastProvider} from "@heroui/toast";
import { initCapacitor, getNetworkStatus, addNetworkListener } from "@/utils/capacitor";

export interface ProvidersProps {
  children: React.ReactNode;
  themeProps?: ThemeProviderProps;
}

declare module "@react-types/shared" {
  interface RouterConfig {
    routerOptions: NonNullable<
      Parameters<ReturnType<typeof useRouter>["push"]>[1]
    >;
  }
}

export function Providers({ children, themeProps }: ProvidersProps) {
  const router = useRouter();
  const [isOnline, setIsOnline] = React.useState(true);

  // Initialize Capacitor when the app starts
  useEffect(() => {
    const initApp = async () => {
      // Initialize Capacitor plugins
      await initCapacitor();
      
      // Check network status
      const networkStatus = await getNetworkStatus();
      setIsOnline(networkStatus);
      
      // Listen for network changes
      addNetworkListener((connected) => {
        setIsOnline(connected);
        if (!connected) {
          // Show offline toast or notification
          console.warn('Network connection lost');
        } else {
          console.log('Network connection restored');
        }
      });
    };
    
    initApp();
  }, []);

  return (
    <HeroUIProvider navigate={router.push}>
      <ToastProvider 
        placement="top-center" 
        maxVisibleToasts={3}
        toastOffset={8}
        toastProps={{
          classNames: {
            base: "mt-safe-area",
            content: "rounded-xl shadow-lg",
            title: "text-base font-semibold",
            description: "text-sm",
            closeButton: "opacity-100 absolute right-4 top-1/2 -translate-y-1/2",
          },
          timeout: 3000,
          shouldShowTimeoutProgress: true,
        }}
      />
      {!isOnline && (
        <div className="fixed top-0 left-0 right-0 bg-red-500 text-white py-1 text-center text-sm z-50">
          Du bist offline. Einige Funktionen sind möglicherweise nicht verfügbar.
        </div>
      )}
      <NextThemesProvider {...themeProps}>{children}</NextThemesProvider>
    </HeroUIProvider>
  );
}
