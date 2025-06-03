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
          if (prevRewardPoints.current !== null) {
            if (newPoints > (prevRewardPoints.current ?? 0)) {
              // Success toast if points increased
              addToast({
                title: "Punkte erhöht",
                description: `Du hast jetzt ${newPoints} Punkte!`,
                color: "success" as const,
                timeout: 3000,
                shouldShowTimeoutProgress: true,
              });
            } else if (newPoints < (prevRewardPoints.current ?? 0)) {
              // Warning toast if points decreased
              addToast({
                title: "Punkte reduziert",
                description: `Du hast jetzt ${newPoints} Punkte!`,
                color: "warning" as const,
                timeout: 3000,
                shouldShowTimeoutProgress: true,
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
      socket.close();
    };
  }, [setRewardPoints]);

  return <>{children}</>;
};
