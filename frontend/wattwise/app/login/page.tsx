"use client";

import { useState } from "react";
import { Input } from "@heroui/input";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { useCapacitorRouter } from "@/components/CapacitorRouter";
import { apiPost } from "@/utils/api";
import { storage } from "@/utils/capacitor";

interface AuthResponse {
  email: string;
  token: string;
  name: string;
}

export default function AuthPage() {
  const [isLogin, setIsLogin] = useState(true);
  const [registerCompleted, setRegisterCompleted] = useState(false);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const { navigateTo } = useCapacitorRouter();

  const handleLogin = async () => {
    try {
      const response = await apiPost<AuthResponse>(
        "/api/users/login",
        {
          email: email.trim(),
          password,
        }
      );

      const authData = response.data as AuthResponse;
      console.log("Login successful", authData);

      await storage.set("email", authData.email);
      await storage.set("token", authData.token);
      await storage.set("name", authData.name);
      
      // Trigger custom event to notify WebSocketProvider about login
      window.dispatchEvent(new Event('userLoggedIn'));
      
      navigateTo("/");
    } catch (error: any) {
      console.error("Login failed", error);
      alert(error.response?.data?.message || "Login fehlgeschlagen.");
    }
  };

  const handleRegister = async () => {
    try {
      const response = await apiPost(
        "/api/users/register",
        {
          name,
          email: email.trim(),
          password,
        }
      );

      console.log("Registration successful", response.data);

      setRegisterCompleted(true);
      setIsLogin(true);
    } catch (error: any) {
      console.error("Registration failed", error);
      alert(error.response?.data?.message || "Registrierung fehlgeschlagen.");
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    isLogin ? handleLogin() : handleRegister();
  };

  return (
    <div className="max-w-md mx-auto p-6 pt-12 pb-[calc(1rem+env(safe-area-inset-bottom,0))]">
      <h1 className="text-2xl font-bold text-center mb-6">
        {isLogin ? "Login" : "Registrieren"}
      </h1>

      {registerCompleted && (
        <div>
          <div className="mb-4 rounded-lg border border-green-400 bg-green-100 px-4 py-3 text-green-800 dark:border-green-600 dark:bg-green-900 dark:text-green-100">
            ✅ Registrierung erfolgreich.
          </div>
          <div className="mb-4 rounded-lg border border-yellow-400 bg-yellow-100 px-4 py-3 text-yellow-800 dark:border-yellow-600 dark:bg-yellow-900 dark:text-yellow-100">
            ⚠️ Bitte logge dich ein, um fortzufahren.
          </div>
        </div>
      )}

      <Card className="p-6 rounded-2xl shadow-lg bg-white dark:bg-zinc-900">
        <form onSubmit={handleSubmit} className="space-y-6">
          {!isLogin && (
            <Input
              isRequired
              label="Name"
              placeholder="Max Mustermann"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          )}

          <Input
            isRequired
            label="E-Mail"
            type="email"
            placeholder="email@example.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <Input
            isRequired
            label="Passwort"
            minLength={12}
            type="password"
            placeholder="••••••••"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <Button type="submit" color="primary" className="w-full">
            {isLogin ? "Einloggen" : "Registrieren"}
          </Button>
        </form>
      </Card>

      <div className="text-center mt-6 text-sm text-gray-600 dark:text-gray-400">
        {isLogin ? (
          <>
            Noch keinen Account?{" "}
            <button
              onClick={() => setIsLogin(false)}
              className="text-indigo-600 font-medium hover:underline"
            >
              Jetzt registrieren
            </button>
          </>
        ) : (
          <>
            Bereits registriert?{" "}
            <button
              onClick={() => setIsLogin(true)}
              className="text-indigo-600 font-medium hover:underline"
            >
              Zum Login
            </button>
          </>
        )}
      </div>
    </div>
  );
}
