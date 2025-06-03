"use client";

import React from "react";
import { Table, TableHeader, TableColumn, TableBody, TableRow, TableCell } from "@heroui/table";
import { DropshippingWidget } from "../../components/EnhancedDropshippingWidget";

// Hilfsfunktionen für Punkte
function getPointsColor(points) {
  if (points > 0) return "text-green-700 bg-green-100";
  if (points < 0) return "text-red-700 bg-red-100";
  return "text-yellow-700 bg-yellow-100";
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
  // ... weitere Einträge
];


export default function TransactionHistory() {

  //TODO: Replace with real data fetching logic
  const transactions = testTransactions; 

  return (
    <div className="bg-white dark:bg-zinc-900 shadow-lg rounded-2xl p-6 mb-8">
      <h2 className="text-xl font-semibold mb-4">Transaktionsverlauf</h2>
      <Table aria-label="Transaktionsverlauf">
        <TableHeader>
          <TableColumn width={150}>Datum</TableColumn>
          <TableColumn>Quelle</TableColumn>
          <TableColumn>Ressource</TableColumn>
          <TableColumn align="end">Punkte</TableColumn>
        </TableHeader>
        <TableBody
          emptyContent={"Keine Transaktionen gefunden."}
          items={transactions}
        >
          {(tx) => (
            <TableRow key={tx.Transaction_ID}>
              <TableCell>{new Date(tx.Date).toLocaleDateString()}</TableCell>
              <TableCell>{tx.Point_Source}</TableCell>
              <TableCell>{tx.Point_Ressource}</TableCell>
              <TableCell className="text-right">
                <span
                  className={`inline-block px-3 py-1 rounded-full font-semibold ${getPointsColor(
                    tx.Points_Gained
                  )}`}
                >
                  {getPointsPrefix(tx.Points_Gained)}
                  {Math.abs(tx.Points_Gained)}
                </span>
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </div>
  );
}
