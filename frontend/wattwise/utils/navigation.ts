import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';

// Check if we're running in Capacitor
export const isCapacitor = (): boolean => {
  return typeof window !== 'undefined' && 
    window.location.protocol !== 'http:' && 
    window.location.protocol !== 'https:';
};

// Custom navigation hook that works in both web and Capacitor contexts
export const useAppNavigation = () => {
  const router = useRouter();
  const [isCapacitorEnv, setIsCapacitorEnv] = useState(false);
  
  useEffect(() => {
    setIsCapacitorEnv(isCapacitor());
  }, []);

  const navigate = (path: string) => {
    // Use Next.js router for navigation
    router.push(path);
  };

  return {
    navigate,
    isCapacitorEnv
  };
};

// Parse query parameters from URL for Capacitor static exports
export const parseQueryParams = (): Record<string, string> => {
  if (typeof window === 'undefined') return {};
  
  const params = new URLSearchParams(window.location.search);
  const result: Record<string, string> = {};
  
  params.forEach((value, key) => {
    result[key] = value;
  });
  
  return result;
};

// Get dynamic path parameter for static exports
export const getDynamicParam = (paramName: string): string | null => {
  if (typeof window === 'undefined') return null;
  
  const params = parseQueryParams();
  return params[paramName] || null;
};
