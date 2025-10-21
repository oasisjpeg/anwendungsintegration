/** @type {import('next').NextConfig} */
const nextConfig = {
  output: 'export',  // Enable static exports for Capacitor
  distDir: 'out',    // Output directory for the static build
  images: {
    unoptimized: true, // Needed for static export
  },
  // Disable server-side features that aren't compatible with static exports
  trailingSlash: true,
  // Disable ESLint during build to avoid issues
  eslint: {
    ignoreDuringBuilds: true,
  },
  // Note: The async rewrites() function below will only work in development mode
  // and not in the static export. For production, you'll need to configure CORS
  // on your backend server.
  
};

module.exports = nextConfig;
