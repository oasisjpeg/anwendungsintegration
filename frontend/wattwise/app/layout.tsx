import "@/styles/globals.css";
import { Metadata, Viewport } from "next";
import clsx from "clsx";

import { Providers } from "./providers";
import { siteConfig } from "@/config/site";
import { fontSans } from "@/config/fonts";
import { LayoutShell } from "@/components/layout-shell";
import { WebSocketProvider } from "@/context/WebSocketProvider";
import { RewardPointsProvider } from "@/context/RewardPointsContext";
import { CapacitorRouterProvider } from "@/components/CapacitorRouter";
import { UIStateProvider } from "@/context/UIStateContext";
import { PushNotificationInit } from "@/components/PushNotificationInit";


export const metadata: Metadata = {
  title: {
    default: siteConfig.name,
    template: `%s | ${siteConfig.name}`,
  },
  description: siteConfig.description,
  icons: {
    icon: "/favicon.ico",
  },
};
export const viewport: Viewport = {
  width: 'device-width',
  initialScale: 1,
  maximumScale: 1,
  userScalable: false,
  viewportFit: 'cover',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html suppressHydrationWarning lang="en">
      <head />
      <body
        className={clsx(
          "min-h-screen bg-background font-sans antialiased ios-fix",
          fontSans.variable
        )}
      >
        <Providers themeProps={{ attribute: "class", defaultTheme: "dark" }}>
          <PushNotificationInit />
          <UIStateProvider>
            <CapacitorRouterProvider>
              <RewardPointsProvider>
                <WebSocketProvider>
                  <LayoutShell>{children}</LayoutShell>
                </WebSocketProvider>
              </RewardPointsProvider>
            </CapacitorRouterProvider>
          </UIStateProvider>
        </Providers>
      </body>
    </html>
  );
}
