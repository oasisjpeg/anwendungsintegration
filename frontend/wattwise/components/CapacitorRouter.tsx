"use client";

import React, { createContext, useContext, useState, useEffect } from 'react';
import { usePathname, useRouter } from 'next/navigation';
import { isCapacitor } from '@/utils/navigation';

// Create a context for the router
interface CapacitorRouterContextType {
  navigateTo: (path: string, params?: Record<string, string>) => void;
  currentPath: string;
  isCapacitorEnv: boolean;
}

const CapacitorRouterContext = createContext<CapacitorRouterContextType>({
  navigateTo: () => {},
  currentPath: '',
  isCapacitorEnv: false
});

// Hook to use the router
export const useCapacitorRouter = () => useContext(CapacitorRouterContext);

// Provider component
export const CapacitorRouterProvider: React.FC<{children: React.ReactNode}> = ({ children }) => {
  const router = useRouter();
  const pathname = usePathname();
  const [currentPath, setCurrentPath] = useState('');
  const [isCapacitorEnv, setIsCapacitorEnv] = useState(false);
  const [pathHistory, setPathHistory] = useState<string[]>([]);

  useEffect(() => {
    const isCapEnv = isCapacitor();
    setIsCapacitorEnv(isCapEnv);
    
    // In Capacitor, we need to track path changes manually
    if (pathname) {
      setCurrentPath(pathname);
      
      // Keep track of navigation history
      setPathHistory(prev => [...prev, pathname]);
      
      // Track path changes in Capacitor environment
      if (isCapEnv) {
        // Path updated
      }
    }
  }, [pathname]);
  
  // Additional effect to handle login path specifically
  useEffect(() => {
    // Ensure login paths are properly detected
    if (pathname === '/login' || pathname?.startsWith('/login/')) {
      // Login page detected, navbar will be hidden by LayoutShell
    }
  }, [pathname]);

  // Navigation function that works in both web and Capacitor
  const navigateTo = (path: string, params?: Record<string, string>) => {
    // Update current path immediately for better UX in Capacitor
    setCurrentPath(path);
    if (isCapacitorEnv) {
      // For Capacitor, convert dynamic routes to query parameters
      let finalPath = path;
      
      // If we have parameters, add them as query params
      if (params && Object.keys(params).length > 0) {
        const queryParams = new URLSearchParams();
        Object.entries(params).forEach(([key, value]) => {
          queryParams.append(key, value);
        });
        
        // Replace dynamic path segments with their values
        Object.entries(params).forEach(([key, value]) => {
          finalPath = finalPath.replace(`[${key}]`, value);
        });
        
        // If we still have dynamic segments, use query params
        if (finalPath.includes('[') && finalPath.includes(']')) {
          // Extract the base path without dynamic segments
          finalPath = finalPath.split('/').filter(segment => !segment.includes('[')).join('/');
          finalPath = `${finalPath}?${queryParams.toString()}`;
        }
      }
      
      router.push(finalPath);
    } else {
      // For web, use Next.js router normally
      let finalPath = path;
      
      // Replace dynamic path segments with their values
      if (params) {
        Object.entries(params).forEach(([key, value]) => {
          finalPath = finalPath.replace(`[${key}]`, value);
        });
      }
      
      router.push(finalPath);
    }
  };

  return (
    <CapacitorRouterContext.Provider value={{ navigateTo, currentPath, isCapacitorEnv }}>
      {children}
    </CapacitorRouterContext.Provider>
  );
};
