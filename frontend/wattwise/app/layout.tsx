import "@/styles/globals.css";
import { Metadata, Viewport } from "next";
import clsx from "clsx";

import { Providers } from "./providers";
import { siteConfig } from "@/config/site";
import { fontSans } from "@/config/fonts";
import { LayoutShell } from "@/components/layout-shell";
import { WebSocketProvider } from "@/context/WebSocketProvider";
import { RewardPointsProvider } from "@/context/RewardPointsContext";


export const metadata: Metadata = { /* ... */ };
export const viewport: Viewport = { /* ... */ };

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
          "min-h-screen bg-background font-sans antialiased",
          fontSans.variable
        )}
      >
        <Providers themeProps={{ attribute: "class", defaultTheme: "dark" }}>
          <RewardPointsProvider>
            <WebSocketProvider>
              <LayoutShell>{children}</LayoutShell>
            </WebSocketProvider>
          </RewardPointsProvider>
        </Providers>
      </body>
    </html>
  );
}
