import { SplashScreen } from '@capacitor/splash-screen';
import { StatusBar, Style } from '@capacitor/status-bar';
import { Preferences } from '@capacitor/preferences';
import { Device } from '@capacitor/device';
import { Network } from '@capacitor/network';
import { isCapacitor } from './navigation';

// Initialize Capacitor plugins
export const initCapacitor = async (): Promise<void> => {
  if (!isCapacitor()) return;

  try {
    // Hide the splash screen with a fade animation
    await SplashScreen.hide({
      fadeOutDuration: 500
    });

    // Set the status bar style
    await StatusBar.setStyle({ style: Style.Dark });
    await StatusBar.setBackgroundColor({ color: '#000000' });
  } catch (error) {
    console.error('Error initializing Capacitor:', error);
  }
};

// Device information
export const getDeviceInfo = async (): Promise<any> => {
  if (!isCapacitor()) return null;
  
  try {
    return await Device.getInfo();
  } catch (error) {
    console.error('Error getting device info:', error);
    return null;
  }
};

// Network status
export const getNetworkStatus = async (): Promise<boolean> => {
  if (!isCapacitor()) return navigator.onLine;
  
  try {
    const status = await Network.getStatus();
    return status.connected;
  } catch (error) {
    console.error('Error getting network status:', error);
    return navigator.onLine;
  }
};

// Listen for network status changes
export const addNetworkListener = (callback: (status: boolean) => void): void => {
  if (!isCapacitor()) {
    window.addEventListener('online', () => callback(true));
    window.addEventListener('offline', () => callback(false));
    return;
  }
  
  Network.addListener('networkStatusChange', (status) => {
    callback(status.connected);
  });
};

// Storage utilities using Capacitor Preferences
export const storage = {
  async set(key: string, value: any): Promise<void> {
    if (isCapacitor()) {
      await Preferences.set({
        key,
        value: JSON.stringify(value)
      });
    } else {
      localStorage.setItem(key, JSON.stringify(value));
    }
  },
  
  async get(key: string): Promise<any> {
    if (isCapacitor()) {
      const { value } = await Preferences.get({ key });
      return value ? JSON.parse(value) : null;
    } else {
      const value = localStorage.getItem(key);
      return value ? JSON.parse(value) : null;
    }
  },
  
  async remove(key: string): Promise<void> {
    if (isCapacitor()) {
      await Preferences.remove({ key });
    } else {
      localStorage.removeItem(key);
    }
  },
  
  async clear(): Promise<void> {
    if (isCapacitor()) {
      await Preferences.clear();
    } else {
      localStorage.clear();
    }
  }
};
