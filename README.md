# 🚀 WattWise Frontend

This is the **WattWise Frontend**, built using **Next.js** and **Hero UI**.

## 📚 Folder Structure

```
frontend/
│-- wattwise/
│   │-- app/          # Next.js App Router (if applicable)
│   │-- components/   # Reusable UI components
│   │-- styles/       # Global CSS & Tailwind styles
│   │-- public/       # Static assets (images, etc.)
│   ├── package.json  # Project dependencies
│   ├── tailwind.config.js # Tailwind configuration
│   ├── next.config.js # Next.js configuration
```

---

## 🛠️ **Installation**

Make sure you have **Node.js** installed (recommended: **LTS version**).

1. **Navigate to the frontend directory:**

   ```bash
   cd frontend/wattwise
   ```

2. **Install dependencies:**

   ```bash
   npm install
   ```

---

## 🚀 **Run the Development Server**

After installing dependencies, start the development server:

```bash
npm run dev
```

- The app will be available at ``.

<!-- 
## ✅ **Environment Variables**

If required, create a `.env.local` file in the `frontend/wattwise/` directory and add:

```env
NEXT_PUBLIC_API_URL=https://your-backend-api.com
```

Make sure to restart the server after adding environment variables.

--- -->

<!-- ## ⚡ **Build for Production**

To build and start the production version:

```bash
npm run build
npm start
``` -->

---
## 🛠 **Troubleshooting**

- If you encounter errors, try:
  ```bash
  rm -rf node_modules package-lock.json
  npm install
  npm run dev
  ```



## 🐜 **License**

This project is licensed under the [MIT License](LICENSE).

