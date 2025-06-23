"use client";
import React, { useEffect, useState } from "react";
import axios from "axios";

// Hilfsfunktionen für Punkte
function getPointsColor(points: number) {
  if (points > 0) return "text-green-700 bg-green-100 dark:text-green-300 dark:bg-green-900/40";
  if (points < 0) return "text-red-700 bg-red-100 dark:text-red-300 dark:bg-red-900/40";
  return "text-yellow-700 bg-yellow-100 dark:text-yellow-300 dark:bg-yellow-900/40";
}

function getPointsPrefix(points: number) {
  if (points > 0) return "+";
  if (points < 0) return "-";
  return "";
}

// Map pointSourceType to string
function getPointSource(pointSourceType: any) {
  switch (pointSourceType) {
    case 0:
      return "Artikel gelesen";
    case 1:
      return "Quiz abgeschlossen";
    default:
      return "Anderer Punktesource";
  }
}

type Transaction = {
  id: string | number;
  created: string;
  pointsGained: number;
  pointSourceType: number;
  userId: string | number;
};

export default function TransactionHistory() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);

  useEffect(() => {
    axios.get("http://localhost:5137/api/users/transactions/50", {
      headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
    })
      .then(response => {
        setTransactions(response.data);
      })
      .catch(error => {
        console.error("Error fetching transactions:", error);
      });
  }, []);

  return (
    <div className="shadow-lg rounded-2xl p-4 mb-8 pb-24">
      <h2 className="text-xl font-semibold mb-4">Transaktionsverlauf</h2>
      {transactions.length === 0 ? (
        <div className="text-center py-8 text-gray-500 dark:text-gray-400">
          Keine Transaktionen gefunden.
        </div>
      ) : (
        <div className="space-y-3">
          {transactions.map((tx) => (
            <div
              key={tx.id}
              className="bg-gray-50 dark:bg-zinc-800 rounded-xl p-4 border border-gray-200 dark:border-zinc-700"
            >
              <div className="flex justify-between items-center mb-2">
                <span className="text-sm text-gray-700 dark:text-gray-300">
                  {new Date(tx.created).toLocaleDateString()}
                </span>
                <span
                  className={`inline-block px-3 py-1 rounded-full font-semibold text-sm ${getPointsColor(
                    tx.pointsGained
                  )}`}
                >
                  {getPointsPrefix(tx.pointsGained)}
                  {Math.abs(tx.pointsGained)}
                </span>
              </div>
              <div className="text-sm font-medium text-gray-900 dark:text-gray-200">
                {getPointSource(tx.pointSourceType)}
              </div>
              <div className="text-sm text-gray-600 dark:text-gray-400">
                User ID: {tx.userId}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
