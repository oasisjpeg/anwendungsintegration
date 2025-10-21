"use client";

import React from 'react';
import NextLink from 'next/link';
import { isCapacitor } from '@/utils/navigation';

interface CapacitorLinkProps {
  href: string;
  children: React.ReactNode;
  className?: string;
  onClick?: (e: React.MouseEvent<HTMLAnchorElement>) => void;
}

const CapacitorLink: React.FC<CapacitorLinkProps> = ({ 
  href, 
  children, 
  className,
  onClick
}) => {
  // For dynamic routes in Capacitor, we need to convert them to query parameters
  const getCapacitorHref = (path: string) => {
    // Check if this is a dynamic route with parameters
    if (path.includes('[') && path.includes(']')) {
      // Extract the base path and parameter
      const basePath = path.split('/').slice(0, -1).join('/');
      const paramName = path.split('/').pop()?.replace(/[\[\]]/g, '');
      
      // Convert to a query parameter format
      return `${basePath}?${paramName}=`;
    }
    
    return path;
  };

  const handleClick = (e: React.MouseEvent<HTMLAnchorElement>) => {
    if (onClick) {
      onClick(e);
    }
  };

  const capacitorMode = isCapacitor();
  const finalHref = capacitorMode ? getCapacitorHref(href) : href;

  return (
    <NextLink href={finalHref} className={className} onClick={handleClick}>
      {children}
    </NextLink>
  );
};

export default CapacitorLink;
