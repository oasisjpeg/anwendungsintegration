"use client";
import { Avatar, AvatarGroup, AvatarIcon } from "@heroui/avatar";
import { Card, CardHeader, CardBody, CardFooter } from "@heroui/card";

const rows = [
  {
    key: "1",
    name: "Marc Reutz",
    avatar: "/avatars/Male1.png",
    savedPower: "120 P.",
    medal: "/rang/Gold.png",
    color: "bg-gradient-to-br from-blue-600 to-blue-400",
    text: "text-white",
  },
  {
    key: "2",
    name: "Jacob Faller",
    avatar: "/avatars/Male2.webp",
    savedPower: "110 P.",
    medal: "/rang/Silber.png",
    color: "bg-gradient-to-br from-yellow-500 to-yellow-300",
    text: "text-white",
  },
  {
    key: "3",
    name: "Luca Greinecker",
    avatar: "/avatars/Woman1.webp",
    savedPower: "95 P.",
    medal: "/rang/Bronze.png",
    color: "bg-gradient-to-br from-orange-500 to-orange-300",
    text: "text-white",
  },
  {
    key: "4",
    name: "Manuel Raich",
    avatar: "/avatars/Woman2.jpg",
    savedPower: "80 P.",
    medal: "/rang/Rest.png",
    color: "bg-gradient-to-br from-red-500 to-red-300",
    text: "text-white",
  },
];

export default function StromsparMedalsTable() {
  return (
    <div className="min-h-screen flex flex-col items-center px-4  ">
      <div className="w-full max-w-sm mx-auto">
        <Card className="rounded-2xl shadow-xl  overflow-hidden">
          <CardHeader className="px-6 py-4 ">
            <h1 className="text-xl font-bold text-center tracking-wide text-black dark:text-white">
              STROMSPAR-RANGLISTE
            </h1>
          </CardHeader>
          <CardBody className="px-4 py-3 flex flex-col gap-3.5">
            {rows.map((row, idx) => (
              <Card
                key={row.key}
                radius="lg"
                shadow="sm"
                isPressable
                className="bg-gray-50 dark:bg-zinc-800 hover:scale-[1.02] transition-transform duration-200"
              >
                <CardBody className="px-4 py-3">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-3 flex-1 min-w-0">
                      <span
                        className={`w-8 h-8 rounded-full flex items-center justify-center font-bold text-sm ${row.color} ${row.text} shadow-inner`}
                      >
                        {idx + 1}
                      </span>
                      <Avatar
                        src={row.avatar}
                        name={row.name}
                        size="sm"
                        classNames={{ base: "w-9 h-9" }}
                        icon={<AvatarIcon />}
                      />
                      <span className="font-medium text-gray-900 dark:text-gray-200 text-base truncate">
                        {row.name}
                      </span>
                    </div>
                    <div className="flex flex-col items-end min-w-[60px]">
                      <img
                        src={row.medal}
                        alt="Medaille"
                        className="w-7 h-7 mb-1 drop-shadow-md"
                      />
                      <span className="text-green-600 dark:text-green-400 font-semibold text-sm">
                        {row.savedPower}
                      </span>
                    </div>
                  </div>
                </CardBody>
              </Card>
            ))}
          </CardBody>
          <CardFooter className="px-4 py-4 justify-center bg-gray-100 dark:bg-gray-700/50 rounded-b-2xl">
            <AvatarGroup>
              <Avatar src="/avatars/Katze.jpg" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Dog.jpg" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Vogel.webp" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Eidechse.webp" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Maus.jpg" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/AvatarMan.png" classNames={{ base: "w-8 h-8" }} />
            </AvatarGroup>
          </CardFooter>
        </Card>
      </div>
    </div>
  );
}
