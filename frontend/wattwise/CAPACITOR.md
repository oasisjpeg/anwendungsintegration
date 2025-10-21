# WattWise Capacitor Integration Guide

This document provides information about the Capacitor integration in the WattWise app, explaining how the app has been configured to work as both a web application and a native mobile app.

## Overview

WattWise has been integrated with Capacitor to enable running the Next.js application as a native mobile app on iOS and Android. This integration includes:

- Static export configuration for Next.js
- Client-side routing compatible with Capacitor
- Safe storage and API handling for mobile environments
- Native device features through Capacitor plugins

## Project Structure

The project follows a standard Next.js structure with additional Capacitor-specific files:

- `capacitor.config.ts` - Main Capacitor configuration
- `android/` - Android platform code
- `ios/` - iOS platform code
- `components/CapacitorRouter.tsx` - Custom router for Capacitor compatibility
- `components/CapacitorLink.tsx` - Custom link component for Capacitor
- `utils/capacitor.ts` - Utilities for Capacitor features
- `utils/navigation.ts` - Navigation utilities for Capacitor
- `utils/api.ts` - API utilities that work in both web and mobile environments

## Capacitor Configuration

The Capacitor configuration is defined in `capacitor.config.ts`:

```typescript
import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'io.wattwise.app',
  appName: 'WattWise',
  webDir: 'out',
  server: {
    androidScheme: 'https',
    cleartext: true
  },
  plugins: {
    SplashScreen: {
      launchShowDuration: 2000
    }
  }
};

export default config;
```

## Next.js Configuration

The Next.js configuration has been updated to support static exports for Capacitor:

```javascript
const nextConfig = {
  output: 'export',  // Enable static exports for Capacitor
  distDir: 'out',    // Output directory for the static build
  images: {
    unoptimized: true, // Needed for static export
  },
  trailingSlash: true,
  eslint: {
    ignoreDuringBuilds: true,
  },
};
```

## Routing System

### CapacitorRouter

A custom router wrapper has been created to handle navigation in both web and Capacitor environments:

- `CapacitorRouterProvider` - Context provider for Capacitor-compatible routing
- `useCapacitorRouter` - Hook to access navigation functions

### CapacitorLink

A custom Link component that works in both web and Capacitor environments:

- Handles dynamic routes by converting them to query parameters in Capacitor
- Maintains normal Next.js routing in web environments

## Storage and API

### Storage

A unified storage API has been implemented that works in both web and Capacitor environments:

```typescript
// Usage example
import { storage } from '@/utils/capacitor';

// Store data
await storage.set('key', value);

// Retrieve data
const value = await storage.get('key');

// Remove data
await storage.remove('key');
```

### API Calls

API calls have been updated to work in both web and mobile environments:

```typescript
// Usage example
import { apiGet, apiPost } from '@/utils/api';

// GET request with type safety
const response = await apiGet<DataType>('/api/endpoint');

// POST request with type safety
const response = await apiPost<ResponseType>('/api/endpoint', data);
```

## Native Features

The following Capacitor plugins have been integrated:

1. **SplashScreen** - Handles the app splash screen
2. **StatusBar** - Controls the device status bar
3. **Preferences** - Provides secure storage for user preferences
4. **Device** - Provides device information
5. **Network** - Monitors network connectivity

These can be accessed through the utilities in `utils/capacitor.ts`:

```typescript
import { 
  initCapacitor, 
  getDeviceInfo, 
  getNetworkStatus, 
  addNetworkListener,
  storage 
} from '@/utils/capacitor';

// Initialize Capacitor features
await initCapacitor();

// Check network status
const isOnline = await getNetworkStatus();

// Listen for network changes
addNetworkListener((connected) => {
  console.log(`Network is ${connected ? 'online' : 'offline'}`);
});
```

## Building and Running

### Web Development

```bash
npm run dev
```

### Building for Capacitor

```bash
# Build the Next.js app for static export
npm run export

# Sync with Capacitor platforms
npm run cap:sync

# Open in Xcode (iOS)
npm run cap:ios

# Open in Android Studio
npm run cap:android
```

## Common Issues and Solutions

### Server-Side Rendering (SSR)

Capacitor requires static exports, which means SSR features aren't available. Always check for browser environment before using browser-specific APIs:

```typescript
if (typeof window !== 'undefined') {
  // Browser-only code
}
```

### Dynamic Routes

Dynamic routes in Next.js (e.g., `/product/[id]`) need special handling in Capacitor. Use the `CapacitorLink` component or `useCapacitorRouter` hook for navigation.

### API Endpoints

For mobile builds, ensure API endpoints are accessible from the device. Consider:
- Using relative URLs when possible
- Configuring the API base URL based on the environment
- Handling CORS properly for web environments

## Future Improvements

1. **Deep Linking** - Implement deep linking for both platforms
2. **Push Notifications** - Add push notification support
3. **Offline Mode** - Enhance offline capabilities
4. **Native UI Components** - Use more native UI components where appropriate
5. **App Store Optimization** - Prepare assets and metadata for store submissions

## Resources

- [Capacitor Documentation](https://capacitorjs.com/docs)
- [Next.js Static Export](https://nextjs.org/docs/app/building-your-application/deploying/static-exports)
- [Capacitor Plugins](https://capacitorjs.com/docs/plugins)
