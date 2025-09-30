import axios, { AxiosRequestConfig, AxiosResponse } from 'axios';
import { isCapacitor } from './navigation';
import { storage } from './capacitor';

// Create an axios instance with default config
const api = axios.create({
  timeout: 10000, // 10 seconds
  headers: {
    'Content-Type': 'application/json',
  },
});

// Function to get the base URL based on environment
export const getBaseUrl = (): string => {
  if (isCapacitor()) {
    console.log('Using Capacitor base URL');
    // For Capacitor apps, we need to use the actual server IP/domain
    // This should be configured based on your deployment environment
    //return 'https://api.wattwise.io'; // Replace with your production API
    
    // For development with local server:
    // return 'http://10.0.2.2:5137'; // Android local development (10.0.2.2 points to host's localhost)
    return 'http://192.168.0.164:5137'; // Use the same IP as in capacitor.config.ts but with port 5137
  }
  
  // For web development
  return 'http://192.168.0.164:5137';
};

// Add request interceptor to add auth token
api.interceptors.request.use(
  async (config) => {
    try {
      const token = await storage.get('token');
      if (token) {
        config.headers['Authorization'] = `Bearer ${token}`;
      }
    } catch (error) {
      console.error('Error getting token for API request:', error);
    }
    
    // Set the base URL dynamically based on environment
    config.baseURL = getBaseUrl();
    
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add response interceptor to handle common errors
api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    // Handle token expiration
    if (error.response && error.response.status === 401) {
      // Clear auth data
      try {
        await storage.remove('token');
        await storage.remove('email');
        await storage.remove('name');
      } catch (storageError) {
        console.error('Error clearing storage:', storageError);
      }
      
      // Redirect to login page
      // Note: We can't use the router here, so we'll use window.location
      window.location.href = '/login';
    }
    
    return Promise.reject(error);
  }
);

// Typed API request methods
export const apiGet = <T>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> => {
  return api.get<T>(url, config);
};

export const apiPost = <T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> => {
  return api.post<T>(url, data, config);
};

export const apiPut = <T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> => {
  return api.put<T>(url, data, config);
};

export const apiDelete = <T>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> => {
  return api.delete<T>(url, config);
};

export const apiPatch = <T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> => {
  return api.patch<T>(url, data, config);
};

export default api;
