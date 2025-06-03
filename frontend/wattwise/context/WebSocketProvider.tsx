// components/WebSocketProvider.tsx
"use client"

import React, { useEffect, useRef } from 'react';
import { addToast } from "@heroui/toast";
import { useRewardPoints } from "@/context/RewardPointsContext";

export const WebSocketProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { setRewardPoints } = useRewardPoints();
  const isFirstRender = useRef(true);
  const prevRewardPoints = useRef<number | null>(null);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) return;

    const socket = new WebSocket(`ws://localhost:5137/ws/rewardpoints?access_token=${token}`);

    socket.onmessage = (event) => {
      try {
        const data = JSON.parse(event.data);
        const newPoints: number = data.points;
        if (!isFirstRender.current && newPoints !== prevRewardPoints.current) {
          addToast({
            title: "Punkte aktualisiert",
            description: `Du hast jetzt ${newPoints} Punkte!`,
            color: "success" as const,
            timeout: 5000,
          });
        }
        setRewardPoints(newPoints);
        prevRewardPoints.current = newPoints;
        isFirstRender.current = false;
      } catch (err) {
        console.error("Invalid WebSocket message:", err);
      }
    };

    return () => {
      socket.close();
    };
  }, [setRewardPoints]);

  return <>{children}</>;
};
