"use client";
import React from "react";

// Hilfsfunktionen für Punkte
function getPointsColor(points) {
  if (points > 0) return "text-green-700 bg-green-100 dark:text-green-300 dark:bg-green-900/40";
  if (points < 0) return "text-red-700 bg-red-100 dark:text-red-300 dark:bg-red-900/40";
  return "text-yellow-700 bg-yellow-100 dark:text-yellow-300 dark:bg-yellow-900/40";
}

function getPointsPrefix(points) {
  if (points > 0) return "+";
  if (points < 0) return "-";
  return "";
}

const testTransactions = [
  {
    Transaction_ID: 1,
    Date: "2025-04-29T13:45:00Z",
    Points_Gained: 20,
    Point_Source: "quiz taken",
    Point_Ressource: "Duck Curve Quiz",
    User_ID: 1,
  },
  {
    Transaction_ID: 2,
    Date: "2025-04-28T09:30:00Z",
    Points_Gained: 10,
    Point_Source: "article read",
    Point_Ressource: "Solar Basics Article",
    User_ID: 1,
  },
  {
    Transaction_ID: 3,
    Date: "2025-04-27T17:00:00Z",
    Points_Gained: -100,
    Point_Source: "amazon coupon activated",
    Point_Ressource: "Amazon 10€ Coupon",
    User_ID: 1,
  },
  {
    Transaction_ID: 4,
    Date: "2025-04-26T15:20:00Z",
    Points_Gained: 20,
    Point_Source: "quiz taken",
    Point_Ressource: "Wind Power Quiz",
    User_ID: 1,
  },
  {
    Transaction_ID: 5,
    Date: "2025-04-25T11:00:00Z",
    Points_Gained: 10,
    Point_Source: "article read",
    Point_Ressource: "Battery Storage Article",
    User_ID: 1,
  },
  {
    Transaction_ID: 6,
    Date: "2025-04-24T19:55:00Z",
    Points_Gained: -100,
    Point_Source: "amazon coupon activated",
    Point_Ressource: "Amazon 25€ Coupon",
    User_ID: 1,
  }
];

export default function TransactionHistory() {
  const transactions = testTransactions; // TODO: Replace with real data

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
              key={tx.Transaction_ID}
              className="bg-gray-50 dark:bg-zinc-800 rounded-xl p-4 border border-gray-200 dark:border-zinc-700"
            >
              <div className="flex justify-between items-center mb-2">
                <span className="text-sm text-gray-700 dark:text-gray-300">
                  {new Date(tx.Date).toLocaleDateString()}
                </span>
                <span
                  className={`inline-block px-3 py-1 rounded-full font-semibold text-sm ${getPointsColor(
                    tx.Points_Gained
                  )}`}
                >
                  {getPointsPrefix(tx.Points_Gained)}
                  {Math.abs(tx.Points_Gained)}
                </span>
              </div>
              <div className="text-sm font-medium text-gray-900 dark:text-gray-200">
                {tx.Point_Source}
              </div>
              <div className="text-sm text-gray-600 dark:text-gray-400">
                {tx.Point_Ressource}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
