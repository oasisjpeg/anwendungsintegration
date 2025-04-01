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
import axios from "axios";

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

    const currentEmail = localStorage.getItem("email");
    const token = localStorage.getItem("token");

    if (!currentEmail || !token) {
      alert("Nicht eingeloggt.");
      return;
    }

    try {
      const response = await axios.patch(
        "http://localhost:5137/api/users/update",
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

      if (response.data.user?.email) {
        localStorage.setItem("email", response.data.user.email);
      }

      if (password !== "" || email !== currentEmail) {
        alert(
          "Passwort oder E-Mail geändert. Sie werden zum Login weitergeleitet."
        );
        setIsModalOpen(true);
        localStorage.removeItem("email");
        localStorage.removeItem("token");
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
    const email = localStorage.getItem("email");
    const token = localStorage.getItem("token");

    if (!email || !token) {
      return;
    }

    try {
      const response = await axios.delete(
        "http://localhost:5137/api/users/delete",
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
          data: {
            email,
            password: confirmPassword,
          },
        }
      );
      setShowDeleteModal(false);
      setIsModalOpen(true);
      localStorage.removeItem("email");
      localStorage.removeItem("token");
      setTimeout(() => {
        router.push("/login");
      }, 2000);
    } catch (error: any) {
      console.error("Delete failed", error);
      alert(error.response?.data?.message || "Fehler beim Löschen.");
    }
  };

  function handleLogout() {
    localStorage.removeItem("email");
    localStorage.removeItem("token");
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
