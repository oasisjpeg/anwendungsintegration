"use client";

import { useEffect, useState } from "react";
import { Input } from "@heroui/input";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { ThemeSwitch } from "@/components/theme-switch";
import { useRouter } from "next/navigation";
import {
  Modal,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
} from "@heroui/modal";
import { ClockIcon } from "@heroicons/react/24/outline";
import { storage } from "@/utils/capacitor";
import { apiDelete, apiPatch, getBaseUrl } from "@/utils/api";


export default function SettingsPage() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPasswordModal, setShowPasswordModal] = useState(false);

  const router = useRouter();

  const handleUpdate = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const currentEmail = await storage.get("email");
      const token = await storage.get("token");

      if (!currentEmail || !token) {
        alert("Nicht eingeloggt.");
        return;
      }
      const response = await apiPatch<{ message: string }>(`${getBaseUrl()}/api/users/update`,
        {
          newName: name || undefined,
          newEmail: email !== currentEmail ? email : undefined,
          newPassword: password || undefined,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      console.log("Update successful", response.data);
      if (response.data.message) {
        await storage.set("email", email);
      }

      if (password !== "" || email !== currentEmail) {
        setIsModalOpen(true);
        await storage.remove("email");
        await storage.remove("token");
        setTimeout(() => {
          router.push("/");
        }, 2000);
        return;
      } else {
        setIsUpdateModalOpen(true);
        setTimeout(() => {
          setIsUpdateModalOpen(false);
          window.location.reload();
        }, 2000);
      }
    } catch (error: any) {
      console.error("Update failed", error);
      alert(error.response?.data?.message || "Fehler beim Aktualisieren.");
    }
  };

  const handleDeleteAccount = async (confirmPassword: string) => {
    try {
      const email = await storage.get("email");
      const token = await storage.get("token");

      if (!email || !token) {
        return;
      }
      
      // Include the authorization header and the required data
      const response = await apiDelete<{ message: string }>(`${getBaseUrl()}/api/users/delete`, {
        headers: { Authorization: `Bearer ${token}` },
        data: { email, password: confirmPassword }
      });
      if (response.status === 200) {
        console.log("Delete successful", response.data);
        setShowDeleteModal(false);
        setIsModalOpen(true);
        await storage.remove("email");
        await storage.remove("token");
        setTimeout(() => {
          router.push("/login");
        }, 2000);
      }

    } catch (error: any) {
      console.error("Delete failed", error);
      alert(error.response?.data?.message || "Fehler beim Löschen.");
    }
  };

  async function handleLogout() {
    try {
      await storage.remove("email");
      await storage.remove("token");
      setTimeout(() => {
        router.push("/login");
      }, 200);
    } catch (error) {
      console.error('Error during logout:', error);
    }
  }

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const storedEmail = await storage.get("email");
        const storedName = await storage.get("name");
        if (storedName) {
          setName(storedName);
        }
        if (storedEmail) {
          setEmail(storedEmail);
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };
    
    fetchUserData();
  }, []);
  
  return (
    <div className="max-w-md mx-auto p-6 pb-24">
      <div className="absolute top-16 right-8 z-50">
        <ThemeSwitch />
      </div>
      <div className="absolute top-16 left-8 z-50">
        <button
          onClick={() => router.push("/history")}
          className="rounded-full p-2 bg-transparent text-gray-800 dark:text-white hover:bg-gray-300 dark:hover:bg-gray-600"
        >
          <ClockIcon className="w-8 h-8" />
        </button>
      </div>
          
      <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-6 text-center">
        Einstellungen von {email}
      </h1>
      <Card className="bg-white dark:bg-zinc-900 shadow-xl rounded-2xl p-6">
        <form onSubmit={handleUpdate} className="space-y-6">
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
        <div className="mt-4 flex flex-row gap-x-4">
          <Button onPress={handleLogout} color="warning" className="flex-1">
            Ausloggen
          </Button>
          <Button
            onPress={() => setShowDeleteModal(true)}
            color="danger"
            className="flex-1"
          >
            Account löschen
          </Button>
        </div>
      </Card>

      <div style={{ position: "relative", zIndex: 600 }}>
        <Modal
          backdrop="blur"
          isOpen={isUpdateModalOpen}
          onClose={() => setIsUpdateModalOpen(false)}
        >
          <ModalContent className="md-4 pb-5">
            <ModalHeader>
              <h3>Erfolgreich!</h3>
            </ModalHeader>
            <ModalBody>
              <p>Deine Einstellungen wurden gespeichert</p>
            </ModalBody>
          </ModalContent>
        </Modal>
      </div>
      <div style={{ position: "relative", zIndex: 600 }}>
        <Modal
          backdrop="blur"
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
        >
          <ModalContent className="md-4 pb-5">
            <ModalHeader>
              <h3>Du wurdest ausgelogt!</h3>
            </ModalHeader>
            <ModalBody>
              <p>Du wirst zum Login weitergeleitet</p>
            </ModalBody>
          </ModalContent>
        </Modal>
      </div>

      <div style={{ position: "relative", zIndex: 600 }}>
        <Modal
          backdrop="blur"
          isOpen={showDeleteModal}
          onClose={() => setShowDeleteModal(false)}
        >
          <ModalContent className="md-4 pb-5">
            <ModalHeader>Bist du dir sicher?</ModalHeader>
            <ModalBody>
              <p>
                Möchtest du deinen Account löschen? Diese Aktion kann nicht
                rückgängig gemacht werden.
              </p>
              <ul
                style={{
                  marginLeft: "20px",
                  paddingLeft: "0",
                  listStyleType: "disc",
                }}
              >
                <li>Dein Profil wird gelöscht</li>
              </ul>
            </ModalBody>
            <ModalFooter>
              <Button color="default" onPress={() => setShowDeleteModal(false)}>
                Abbrechen
              </Button>
              <Button
                color="danger"
                onPress={() => {
                  setShowDeleteModal(false);
                  setShowPasswordModal(true);
                }}
              >
                Ja
              </Button>
            </ModalFooter>
          </ModalContent>
        </Modal>
      </div>

      <Modal
        backdrop="blur"
        isOpen={showPasswordModal}
        onClose={() => {
          setShowPasswordModal(false);
          setConfirmPassword("");
        }}
      >
        <ModalContent className="md-4 pb-5">
          <ModalHeader>Passwort bestätigen</ModalHeader>
          <ModalBody>
            <p className="mb-2">Bitte gib dein Passwort zur Bestätigung ein:</p>
            <Input
              type="password"
              placeholder="••••••••"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />
          </ModalBody>
          <ModalFooter>
            <Button
              color="default"
              onPress={() => {
                setShowPasswordModal(false);
                setConfirmPassword("");
              }}
            >
              Abbrechen
            </Button>
            <Button
              color="primary"
              onPress={async () => {
                if (!confirmPassword) return;

                await handleDeleteAccount(confirmPassword);

                setShowPasswordModal(false);
                setConfirmPassword("");
              }}
            >
              Bestätigen
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </div>
  );
}
