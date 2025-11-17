'use client';

import { useEffect } from 'react';
import { initializePushNotifications } from '@/utils/pushNotifications';

export function PushNotificationInit() {
  useEffect(() => {
    // Initialize push notifications when the app loads
    initializePushNotifications();
  }, []);

  // This component doesn't render anything
  return null;
}
