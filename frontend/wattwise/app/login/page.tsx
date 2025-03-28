"use client";

import { useState } from "react";
import { Input } from "@heroui/input";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { useRouter } from "next/navigation";

export default function AuthPage() {
  const [isLogin, setIsLogin] = useState(true);

  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const router = useRouter();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (isLogin) {
      console.log("Logging in:", { email, password });
      localStorage.setItem("email", email);
    } else {
      console.log("Registering:", { name, email, password });
      localStorage.setItem("email", email);
    }

    router.push("/");
    // TODO: Connect to backend
  };

  return (
    <div className="max-w-md mx-auto p-6 pt-24 pb-32">
      <h1 className="text-2xl font-bold text-center mb-6">
        {isLogin ? "Login" : "Registrieren"}
      </h1>

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
