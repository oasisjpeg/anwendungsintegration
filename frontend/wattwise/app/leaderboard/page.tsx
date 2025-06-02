"use client";

import { Table, TableHeader, TableColumn, TableBody, TableRow, TableCell } from "@heroui/table";

// Beispiel-Daten für Stromspar-Rangliste
const rows = [
  {
    key: "1",
    name: "Familie Müller",
    avatar: "/avatars/AvatarMan.png",
    gold: 14,    // kWh gespart (Gold)
    silver: 7,   // Euro gespart (Silber)
    bronze: 5,   // Prozent gespart (Bronze)
    total: 26
  },
  {
    key: "2",
    name: "WG Sonnenstraße",
    avatar: "/avatars/AvatarMan.png",
    gold: 13,
    silver: 7,
    bronze: 10,
    total: 30
  },
  {
    key: "3",
    name: "Herr Schmidt",
    avatar: "/avatars/AvatarMan.png",
    gold: 9,
    silver: 8,
    bronze: 6,
    total: 23
  },
  {
    key: "4",
    name: "Familie Yilmaz",
    avatar: "/avatars/AvatarMan.png",
    gold: 9,
    silver: 6,
    bronze: 8,
    total: 23
  }
];

export default function StromsparMedalsTable() {
  return (
    <div
      className="min-h-screen flex flex-col items-center justify-center"
      style={{
        padding: "2rem"
      }}
    >
      <div className="max-w-md w-full bg-white/80 rounded-2xl shadow-lg p-6">
        <h1 className="text-3xl font-bold text-center mb-6 tracking-widest text-gray-800">
          STROMSPAR-RANGLISTE
        </h1>
        <div className="flex justify-center gap-6 mb-2">
          <span className="flex items-center gap-1">
            <img src="/icons/gold-medal.svg" alt="Gold" className="w-5 h-5" /> kWh
          </span>
          <span className="flex items-center gap-1">
            <img src="/icons/silver-medal.svg" alt="Silber" className="w-5 h-5" /> €
          </span>
          <span className="flex items-center gap-1">
            <img src="/icons/bronze-medal.svg" alt="Bronze" className="w-5 h-5" /> %
          </span>
          <span className="font-semibold">Total</span>
        </div>
        <Table aria-label="Stromspar-Rangliste" removeWrapper className="mt-2">
          <TableHeader>
            <TableColumn> </TableColumn>
            <TableColumn>NAME</TableColumn>
            <TableColumn>
              <img src="/icons/gold-medal.svg" alt="Gold" className="w-4 h-4 inline" />
            </TableColumn>
            <TableColumn>
              <img src="/icons/silver-medal.svg" alt="Silber" className="w-4 h-4 inline" />
            </TableColumn>
            <TableColumn>
              <img src="/icons/bronze-medal.svg" alt="Bronze" className="w-4 h-4 inline" />
            </TableColumn>
            <TableColumn>TOTAL</TableColumn>
          </TableHeader>
          <TableBody>
            {rows.map((row, idx) => (
              <TableRow key={row.key}>
                <TableCell className="font-bold text-gray-500">{idx + 1}</TableCell>
                <TableCell>
                  <div className="flex items-center gap-2">
                    <img
                      src={row.avatar}
                      alt={row.name}
                      className="w-7 h-7 rounded-full border border-gray-300"
                    />
                    <span>{row.name}</span>
                  </div>
                </TableCell>
                <TableCell className="text-yellow-600 font-bold">{row.gold}</TableCell>
                <TableCell className="text-gray-600">{row.silver}</TableCell>
                <TableCell className="text-orange-600">{row.bronze}</TableCell>
                <TableCell className="font-semibold">{row.total}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
