"use client";

import {
  Table,
  TableHeader,
  TableColumn,
  TableBody,
  TableRow,
  TableCell,
} from "@heroui/table";

import { DropshippingWidget } from "../dropshipping/page";

// Beispiel-Daten für Stromspar-Rangliste
const rows = [
  {
    key: "1",
    name: "Marc Reutz",
    avatar: "/avatars/AvatarMan.png",
    gold: 14,
    silver: 7,
    bronze: 5,
    total: 26,
    savedPower: "120 P.",
  },
  {
    key: "2",
    name: "Jacob Faller",
    avatar: "/avatars/AvatarMan.png",
    gold: 13,
    silver: 7,
    bronze: 10,
    total: 30,
    savedPower: "110 P.",
  },
  {
    key: "3",
    name: "Luca Greinecker",
    avatar: "/avatars/AvatarMan.png",
    gold: 9,
    silver: 8,
    bronze: 6,
    total: 23,
    savedPower: "95 P.",
  },
  {
    key: "4",
    name: "Manuel Raich",
    avatar: "/avatars/AvatarMan.png",
    gold: 9,
    silver: 6,
    bronze: 8,
    total: 23,
    savedPower: "80 P.",
  },
];

function getMedalSrc(idx: number) {
  if (idx === 0) return "/rang/Gold.png";
  if (idx === 1) return "/rang/Silber.png";
  if (idx === 2) return "/rang/Bronze.png";
  return "/rang/Rest.png"; // z.B. für alle weiteren Plätze
}


export default function StromsparMedalsTable() {
  return (
    <div
      className="min-h-screen flex flex-col items-center justify-center"
      style={{
        padding: "2rem",
        paddingBottom: "8rem",
      }}
    >
      <div className="max-w-md w-full bg-white rounded-2xl shadow-lg p-6">
        <h1 className="text-3xl font-bold text-center mb-6 tracking-widest text-black">
          STROMSPAR-RANGLISTE
        </h1>
        <Table aria-label="Stromspar-Rangliste" removeWrapper className="mt-2">
          <TableHeader>
            <TableColumn className="text-white">#</TableColumn>
            <TableColumn className="text-white">NAME</TableColumn>
            <TableColumn className="text-white">MEDAL</TableColumn>
            <TableColumn className="text-white">POINT INCREASE</TableColumn>
          </TableHeader>
          <TableBody>
            {rows.map((row, idx) => (
              <TableRow key={row.key}>
                <TableCell className="text-center font-bold text-black">{idx + 1}</TableCell>
                <TableCell>
                  <div className="flex items-center gap-2 text-black">
                    <img
                      src={row.avatar}
                      alt={row.name}
                      className="w-7 h-7 rounded-full border"
                    />
                    <span className="whitespace-nowrap">{row.name}</span>
                  </div>
                </TableCell>
                <TableCell className="text-center text-lg">
                  <img src={getMedalSrc(idx)} alt="Medaille" className="w-7 h-7 mx-auto" />
                </TableCell>
                <TableCell className="text-center font-semibold text-green-700">{row.savedPower}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
      <div className="mt-10 w-full">
        <DropshippingWidget />
      </div>
    </div>
  );
}
