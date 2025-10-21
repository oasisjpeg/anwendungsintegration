"use client";

import React, { createContext, useContext, useState, useCallback } from 'react';

interface UIStateContextType {
  isModalOpen: boolean;
  openModal: () => void;
  closeModal: () => void;
}

const UIStateContext = createContext<UIStateContextType>({
  isModalOpen: false,
  openModal: () => {},
  closeModal: () => {},
});

export const useUIState = () => useContext(UIStateContext);

export const UIStateProvider: React.FC<{children: React.ReactNode}> = ({ children }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  // Use useCallback to ensure function references are stable
  const openModal = useCallback(() => {
    console.log('Opening modal');
    setIsModalOpen(true);
  }, []);

  const closeModal = useCallback(() => {
    console.log('Closing modal');
    setIsModalOpen(false);
  }, []);

  // Create a stable value object
  const value = React.useMemo(() => ({
    isModalOpen,
    openModal,
    closeModal
  }), [isModalOpen, openModal, closeModal]);

  return (
    <UIStateContext.Provider value={value}>
      {children}
    </UIStateContext.Provider>
  );
};
