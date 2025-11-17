import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.wattwise.app2',
  appName: 'WattWise',
  webDir: 'out',
  server: {
    androidScheme: 'https'
    // Note: For iOS simulator, remove the 'url' property to use the built app
    // The API URL is configured in utils/api.ts based on platform
  },
  plugins: {
    SplashScreen: {
      launchShowDuration: 2000
    }
  }
};

export default config;
