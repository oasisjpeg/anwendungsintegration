"use client";
import {
  Avatar,
  AvatarGroup,
  AvatarIcon
} from "@heroui/avatar";
import {
  Table,
  TableHeader,
  TableColumn,
  TableBody,
  TableRow,
  TableCell,
} from "@heroui/table";
import EnhancedDropshippingWidget from "@/components/EnhancedDropshippingWidget";

// Beispiel-Daten für Stromspar-Rangliste
const rows = [
  {
    key: "1",
    name: "Marc Reutz",
    avatar: "/avatars/Male1.png",
    gold: 14,
    silver: 7,
    bronze: 5,
    total: 26,
    savedPower: "120 P.",
  },
  {
    key: "2",
    name: "Jacob Faller",
    avatar: "/avatars/Male2.webp",
    gold: 13,
    silver: 7,
    bronze: 10,
    total: 30,
    savedPower: "110 P.",
  },
  {
    key: "3",
    name: "Luca Greinecker",
    avatar: "/avatars/Woman1.webp",
    gold: 9,
    silver: 8,
    bronze: 6,
    total: 23,
    savedPower: "95 P.",
  },
  {
    key: "4",
    name: "Manuel Raich",
    avatar: "/avatars/Woman2.jpg",
    gold: 9,
    silver: 6,
    bronze: 8,
    total: 23,
    savedPower: "80 P.",
  }
];

function getMedalSrc(idx: number) {
  if (idx === 0) return "/rang/Gold.png";
  if (idx === 1) return "/rang/Silber.png";
  if (idx === 2) return "/rang/Bronze.png";
  return "/rang/Rest.png"; // z.B. für alle weiteren Plätze
}

export default function StromsparMedalsTable() {
  return (
    <div className="min-h-screen flex flex-col items-center px-4 py-8 sm:px-6 lg:px-8">
      <div className="w-full max-w-xs sm:max-w-md md:max-w-lg lg:max-w-xl xl:max-w-2xl bg-white rounded-2xl shadow-lg p-4 sm:p-6">
        <h1 className="text-xl sm:text-2xl md:text-3xl font-bold text-center mb-4 sm:mb-6 tracking-wide sm:tracking-widest text-black">
          STROMSPAR-RANGLISTE
        </h1>
        
        {/* Desktop Table View */}
        <div className="hidden sm:block">
          <Table aria-label="Stromspar-Rangliste" removeWrapper className="mt-2">
            <TableHeader className="relative bg-gradient-to-r from-gray-900 via-gray-800 to-gray-900 text-white py-2 rounded-lg shadow-lg overflow-hidden">
              <TableColumn className="text-white font-bold text-xs sm:text-sm">#</TableColumn>
              <TableColumn className="text-white font-bold text-xs sm:text-sm">NAME</TableColumn>
              <TableColumn className="text-white font-bold text-xs sm:text-sm text-center">MEDAL</TableColumn>
              <TableColumn className="text-white font-bold text-xs sm:text-sm text-center">POINT INCREASE</TableColumn>
            </TableHeader>
            <TableBody>
              {rows.map((row, idx) => (
                <TableRow key={row.key}>
                  <TableCell className="text-center font-bold text-black text-xs sm:text-sm">{idx + 1}</TableCell>
                  <TableCell>
                    <div className="flex items-center gap-2 text-black">
                      <Avatar
                        src={row.avatar}
                        name={row.name}
                        size="sm"
                        classNames={{
                          base: "w-6 h-6 sm:w-7 sm:h-7",
                        }}
                        icon={<AvatarIcon/>}
                      />
                      <span className="whitespace-nowrap text-xs sm:text-sm">{row.name}</span>
                    </div>
                  </TableCell>
                  <TableCell className="text-center text-lg">
                    <img src={getMedalSrc(idx)} alt="Medaille" className="w-6 h-6 sm:w-7 sm:h-7 mx-auto" />
                  </TableCell>
                  <TableCell className="text-center font-semibold text-green-700 text-xs sm:text-sm">{row.savedPower}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>

        {/* Mobile Card View */}
        <div className="sm:hidden space-y-3">
          {rows.map((row, idx) => (
            <div key={row.key} className="bg-gray-50 rounded-lg p-4 border border-gray-200">
              <div className="flex items-center justify-between mb-2">
                <div className="flex items-center gap-3">
                  <span className="bg-gray-900 text-white rounded-full w-6 h-6 flex items-center justify-center text-sm font-bold">
                    {idx + 1}
                  </span>
                  <Avatar
                    src={row.avatar}
                    name={row.name}
                    size="sm"
                    classNames={{
                      base: "w-8 h-8",
                    }}
                    icon={<AvatarIcon/>}
                  />
                  <span className="font-medium text-black text-sm">{row.name}</span>
                </div>
                <img src={getMedalSrc(idx)} alt="Medaille" className="w-8 h-8" />
              </div>
              <div className="text-right text-center">
                <span className="text-green-700 font-semibold text-sm">{row.savedPower}</span>
              </div>
            </div>
          ))}
        </div>

        <AvatarGroup className="pt-4 sm:pt-5 justify-center">
          <Avatar src="/avatars/Katze.jpg" classNames={{ base: "w-8 h-8 sm:w-10 sm:h-10" }} />
          <Avatar src="/avatars/Dog.jpg" classNames={{ base: "w-8 h-8 sm:w-10 sm:h-10" }} />
          <Avatar src="/avatars/Vogel.webp" classNames={{ base: "w-8 h-8 sm:w-10 sm:h-10" }} />
          <Avatar src="/avatars/Eidechse.webp" classNames={{ base: "w-8 h-8 sm:w-10 sm:h-10" }} />
          <Avatar src="/avatars/Maus.jpg" classNames={{ base: "w-8 h-8 sm:w-10 sm:h-10" }} />
          <Avatar src="/avatars/AvatarMan.png" classNames={{ base: "w-8 h-8 sm:w-10 sm:h-10" }} />
        </AvatarGroup>
      </div>
    </div>
  );
}
