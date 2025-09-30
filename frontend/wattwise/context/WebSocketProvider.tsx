"use client"

import React, { useEffect, useRef, useCallback } from 'react';
import { addToast } from "@heroui/toast";
import { useRewardPoints } from "@/context/RewardPointsContext";
import { getBaseUrl } from '@/utils/api';
import { storage } from '@/utils/capacitor';

// Custom hook to listen to storage changes
const useAuthListener = () => {
  const [token, setToken] = React.useState<string | null>(null);

  useEffect(() => {
    // Set initial value from Capacitor storage or localStorage
    const getInitialToken = async () => {
      try {
        const storedToken = await storage.get('token');
        setToken(storedToken);
      } catch (error) {
        console.error('Error getting token from storage:', error);
      }
    };
    
    getInitialToken();
    
    // Listen for storage events (for web browser)
    const handleStorageChange = (e: StorageEvent) => {
      if (e.key === 'token') {
        setToken(e.newValue ? JSON.parse(e.newValue) : null);
      }
    };

    // Listen for custom event triggered after login
    const handleLogin = async () => {
      try {
        const storedToken = await storage.get('token');
        setToken(storedToken);
      } catch (error) {
        console.error('Error getting token after login:', error);
      }
    };

    window.addEventListener('storage', handleStorageChange);
    window.addEventListener('userLoggedIn', handleLogin as EventListener);

    return () => {
      window.removeEventListener('storage', handleStorageChange);
      window.removeEventListener('userLoggedIn', handleLogin as EventListener);
    };
  }, []);

  return token;
};

export const WebSocketProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { setRewardPoints } = useRewardPoints();
  const isFirstRender = useRef(true);
  const prevRewardPoints = useRef<number | null>(null);
  const socketRef = useRef<WebSocket | null>(null);
  const token = useAuthListener();

  const connectWebSocket = useCallback(() => {
    // Close existing connection if any
    if (socketRef.current) {
      socketRef.current.close();
      socketRef.current = null;
    }

    if (!token) {
      return;
    }

    try {
      const socket = new WebSocket(`${getBaseUrl()}/ws/rewardpoints?access_token=${token}`);
      console.log("WebSocket connection established");
      socketRef.current = socket;

    socket.onmessage = (event) => {
      try {

        const data = JSON.parse(event.data);
        const newPoints: number = data.points;
        console.log("WebSocket message received:", data);
        if (!isFirstRender.current && newPoints !== prevRewardPoints.current) {
          if (prevRewardPoints.current !== null) {
            if (newPoints > (prevRewardPoints.current ?? 0)) {
              // Success toast if points increased
              addToast({
                title: "Punkte erhöht",
                description: `Du hast jetzt ${newPoints} Punkte!`,
                color: "success" as const,
              });
            } else if (newPoints < (prevRewardPoints.current ?? 0)) {
              // Warning toast if points decreased
              addToast({
                title: "Punkte reduziert",
                description: `Du hast jetzt ${newPoints} Punkte!`,
                color: "warning" as const,
              });
            }
          }
        }
        setRewardPoints(newPoints);
        prevRewardPoints.current = newPoints;
        isFirstRender.current = false;
      } catch (err) {
        console.error("Invalid WebSocket message:", err);
      }
    };

      return () => {
        if (socket.readyState === WebSocket.OPEN) {
          socket.close();
        }
      };
    } catch (error) {
      console.error("WebSocket connection error:", error);
    }
  }, [token, setRewardPoints]);

  // Connect when token changes
  useEffect(() => {
    connectWebSocket();
    
    return () => {
      if (socketRef.current) {
        socketRef.current.close();
        socketRef.current = null;
      }
    };
  }, [connectWebSocket]);

  return <>{children}</>;
};
