import { PushNotifications } from '@capacitor/push-notifications';
import type { Token, ActionPerformed, PushNotificationSchema } from '@capacitor/push-notifications';
import { isCapacitor } from './navigation';

export const initializePushNotifications = async () => {
  // Only initialize for Capacitor (native apps)
  if (!isCapacitor()) {
    console.log('Push notifications only available in native apps');
    return;
  }

  try {
    // Request permission to use push notifications
    const permStatus = await PushNotifications.requestPermissions();
    
    if (permStatus.receive === 'granted') {
      // Register with Apple / Google to receive push via APNS/FCM
      await PushNotifications.register();
      console.log('Push notifications registered successfully');
    } else {
      console.log('Push notification permission denied');
    }

    // On success, we should be able to receive notifications
    PushNotifications.addListener('registration', (token: Token) => {
      console.log('Push registration success, token: ' + token.value);
      // You can send this token to your backend if needed
    });

    // Some issue with our setup and push will not work
    PushNotifications.addListener('registrationError', (error: any) => {
      console.error('Error on registration: ' + JSON.stringify(error));
    });

    // Show us the notification payload if the app is open on our device
    PushNotifications.addListener(
      'pushNotificationReceived',
      (notification: PushNotificationSchema) => {
        console.log('Push notification received: ', notification);
        // You can show a custom UI here or just let the system handle it
        alert(`${notification.title}: ${notification.body}`);
      }
    );

    // Method called when tapping on a notification
    PushNotifications.addListener(
      'pushNotificationActionPerformed',
      (notification: ActionPerformed) => {
        console.log('Push notification action performed', notification);
        // Handle navigation based on notification data
        // For example: router.push('/specific-page');
      }
    );

  } catch (error) {
    console.error('Error initializing push notifications:', error);
  }
};

// Call this function to get the current notification permissions
export const checkNotificationPermissions = async () => {
  if (!isCapacitor()) {
    return null;
  }
  
  try {
    const permStatus = await PushNotifications.checkPermissions();
    return permStatus;
  } catch (error) {
    console.error('Error checking permissions:', error);
    return null;
  }
};
