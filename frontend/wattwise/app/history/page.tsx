"use client";

import React from "react";

//Typ-Definition für eine Transaktion
interface Transaction {
  Transaction_ID: number;
  Date: string; // ISO-String
  Points_Gained: number;
  Point_Source: string;
  Point_Ressource: string;
  User_ID: number;
}

//Hardcodierte Testdaten <-- Entfernen
const testTransactions: Transaction[] = [
  {
    Transaction_ID: 1,
    Date: "2025-04-29T13:45:00Z",
    Points_Gained: 10,
    Point_Source: "QUIZ",
    Point_Ressource: "Duck Curve Quiz",
    User_ID: 1,
  },
  {
    Transaction_ID: 2,
    Date: "2025-04-28T09:30:00Z",
    Points_Gained: -1,
    Point_Source: "QUIZ_NOT_COMPLETED",
    Point_Ressource: "Solar Basics Quiz",
    User_ID: 1,
  },
  {
    Transaction_ID: 3,
    Date: "2025-04-27T17:00:00Z",
    Points_Gained: 0,
    Point_Source: "INFO",
    Point_Ressource: "System Info",
    User_ID: 1,
  },
];

//Hilfsfunktionen mit Typen
function getPointsColor(points: number): string {
  if (points > 0) 
    return "text-green-700 bg-green-100";
  if (points < 0) 
    return "text-red-700 bg-red-100";
  return "text-yellow-700 bg-yellow-100";
}

function getPointsPrefix(points: number): string {
  if (points > 0) 
    return "+";
  if (points < 0) 
    return "-";
  return "";
}

async function fetchTransactions(): Promise<Transaction[]> {
  const res = await fetch("/api/transactions");
  if (!res.ok) throw new Error("Fehler beim Laden der Daten");
  return res.json();
}

//Komponente
export default function TransactionHistory() {
  const transactions = testTransactions; // <-- Hardcodiert muss danach entfernt werden

  return (
    <div className="bg-white dark:bg-zinc-900 shadow-lg rounded-2xl p-6 mb-8">
      <h2 className="text-xl font-semibold mb-4">Transaktionsverlauf</h2>
      {transactions.length === 0 ? (
        <div className="text-center py-8 text-gray-500">Keine Transaktionen gefunden.</div>
      ) : (
        <div className="overflow-x-auto">
          <table className="min-w-full text-sm">
            <thead>
              <tr>
                <th className="px-4 py-2 text-left">Datum</th>
                <th className="px-4 py-2 text-left">Quelle</th>
                <th className="px-4 py-2 text-left">Ressource</th>
                <th className="px-4 py-2 text-right">Punkte</th>
              </tr>
            </thead>
            <tbody>
              {transactions.map((tx) => (
                <tr key={tx.Transaction_ID} className="border-t">
                  <td className="px-4 py-2">{new Date(tx.Date).toLocaleDateString()}</td>
                  <td className="px-4 py-2">{tx.Point_Source}</td>
                  <td className="px-4 py-2">{tx.Point_Ressource}</td>
                  <td className={`px-4 py-2 text-right font-semibold`}>
                    <span
                      className={`inline-block px-3 py-1 rounded-full font-semibold ${getPointsColor(
                        tx.Points_Gained
                      )}`}
                    >
                      {getPointsPrefix(tx.Points_Gained)}
                      {Math.abs(tx.Points_Gained)}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
