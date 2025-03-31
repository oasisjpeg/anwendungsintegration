"use client";

import { useEffect, useState } from "react";
import { Input } from "@heroui/input";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { ThemeSwitch } from "@/components/theme-switch";
import { useRouter } from "next/navigation";

export default function SettingsPage() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter();
  const handleSubmit = (e: React.FormEvent) => {
    if (name || email || password !== "") {
      alert("Test");
    }
    e.preventDefault();
    console.log("Submitted", { name, email, password });
    // call update endpoint
  };

  function handleLogout() {
    localStorage.removeItem("email");
    setTimeout(() => {
      router.push("/login");
    }, 200);
  }

  useEffect(() => {
    const storedEmail = localStorage.getItem("email");
    if (storedEmail) {
      setEmail(storedEmail);
    } else {
      console.warn("No user data found in localStorage");
    }
  }, []);
  return (
    <div className="max-w-md mx-auto p-6 pb-24">
      <div className="absolute top-4 right-4 z-50">
        <ThemeSwitch />
      </div>
      <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-6 text-center">
        Einstellungen von {email}
      </h1>

      <Card className="bg-white dark:bg-zinc-900 shadow-xl rounded-2xl p-6">
        <form onSubmit={handleSubmit} className="space-y-6">
          <Input
            label="Name"
            placeholder="Max Mustermann"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />

          <Input
            label="E-Mail"
            type="email"
            placeholder="max@example.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <Input
            label="Passwort"
            type="password"
            placeholder="Neues Passwort"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <Button type="submit" color="primary" className="w-full">
            Änderungen speichern
          </Button>
        </form>
        <div className="mt-4">
          <Button onPress={handleLogout} color="danger" className="w-full">
            Ausloggen
          </Button>
        </div>
      </Card>
    </div>
  );
}
