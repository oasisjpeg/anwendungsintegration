import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'io.wattwise.app',
  appName: 'WattWise',
  webDir: 'out',
  server: {
    androidScheme: 'https',
    // For development only - remove for production
    url: 'http://192.168.0.164:5137', // Using port 5137 to match the API server
    cleartext: true
  },
  plugins: {
    SplashScreen: {
      launchShowDuration: 2000
    }
  }
};

export default config;
