// components/RewardPointsContext.tsx
"use client"

import React, { createContext, useContext, useState } from "react";

interface RewardPointsContextType {
  rewardPoints: number | null;
  setRewardPoints: (points: number) => void;
}

const RewardPointsContext = createContext<RewardPointsContextType>({
  rewardPoints: null,
  setRewardPoints: () => {},
});

export const useRewardPoints = () => useContext(RewardPointsContext);

export function RewardPointsProvider({ children }: { children: React.ReactNode }) {
  const [rewardPoints, setRewardPoints] = useState<number | null>(null);

  return (
    <RewardPointsContext.Provider value={{ rewardPoints, setRewardPoints }}>
      {children}
    </RewardPointsContext.Provider>
  );
}
