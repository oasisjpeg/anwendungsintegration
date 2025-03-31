"use client";

import { useState } from "react";
import { Input } from "@heroui/input";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { useRouter } from "next/navigation";
import axios from "axios";

export default function AuthPage() {
  const [isLogin, setIsLogin] = useState(true);
  const [registerCompleted, setRegisterCompleted] = useState(false);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const router = useRouter();

  const handleLogin = async () => {
    try {
      const response = await axios.post("http://localhost:5137/api/users/login", {
        email: email.trim(),
        password,
      });

      console.log("Login successful", response.data);

      localStorage.setItem("email", response.data.email);
      localStorage.setItem("token", response.data.token);
      router.push("/");
    } catch (error: any) {
      console.error("Login failed", error);
      alert(error.response?.data?.message || "Login fehlgeschlagen.");
    }
  };

  const handleRegister = async () => {
    try {
      const response = await axios.post("http://localhost:5137/api/users/register", {
        name,
        email: email.trim(),
        password,
      });

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
    <div className="max-w-md mx-auto p-6 pt-24 pb-32">
      <h1 className="text-2xl font-bold text-center mb-6">
        {isLogin ? "Login" : "Registrieren"}
      </h1>

      {registerCompleted && (
        <div className="mb-4 rounded-lg border border-yellow-400 bg-yellow-100 px-4 py-3 text-yellow-800 dark:border-yellow-600 dark:bg-yellow-900 dark:text-yellow-100">
          ⚠️ Bitte logge dich ein, um fortzufahren.
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
