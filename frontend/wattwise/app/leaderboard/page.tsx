"use client";
import { useState, useEffect } from "react";
import { Avatar, AvatarGroup, AvatarIcon } from "@heroui/avatar";
import { Card, CardHeader, CardBody, CardFooter } from "@heroui/card";
import { Spinner } from "@heroui/spinner";
import axios from "axios";

type LeaderboardUser = {
  userName: string;
  pointIncreaseValue: number;
};

export default function StromsparMedalsTable() {
  const [leaderboard, setLeaderboard] = useState<LeaderboardUser[]>([]);
  const [currentUserScore, setCurrentUserScore] = useState(0);
  const currentUserName = localStorage.getItem("name") || "Gast";
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    axios.get("http://localhost:5137/api/users/leaderboard", {
      headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
    })
      .then((response) => {
        setLeaderboard(response.data.leaderboard);
        setCurrentUserScore(response.data.currentUserScore);
        setIsLoading(false);

        // Optionally: Add current user to leaderboard if not present
        if (!response.data.leaderboard.some((u: { userName: string; }) => u.userName === currentUserName)) {
          setLeaderboard(prev => [
            ...prev,
            { userName: currentUserName, pointIncreaseValue: response.data.currentUserScore }
          ]);
        }
      })
      .catch((error) => {
        console.error("Error fetching user data:", error);
      });
  }, []);

  const sortedLeaderboard = [...leaderboard].sort((a, b) => b.pointIncreaseValue - a.pointIncreaseValue);

  interface AvatarAndMedal {
    avatar: string;
    medal: string;
    color: string;
    text: string;
  }

  const getAvatarAndMedal = (index: number): AvatarAndMedal => {
    const avatars: string[] = [
      "/avatars/Male1.png",
      "/avatars/Male2.webp",
      "/avatars/Woman1.webp",
      "/avatars/Woman2.jpg"
    ];
    const medals: string[] = [
      "/rang/Gold.png",
      "/rang/Silber.png",
      "/rang/Bronze.png",
      "/rang/Rest.png"
    ];
    const colors: string[] = [
      "bg-gradient-to-br from-blue-600 to-blue-400",
      "bg-gradient-to-br from-yellow-500 to-yellow-300",
      "bg-gradient-to-br from-orange-500 to-orange-300",
      "bg-gradient-to-br from-red-500 to-red-300"
    ];
    return {
      avatar: avatars[index % avatars.length],
      medal: medals[Math.min(index, medals.length - 1)],
      color: colors[Math.min(index, colors.length - 1)],
      text: "text-white"
    };
  };

  return (
    <div className="min-h-screen flex flex-col items-center px-4">
      <div className="w-full max-w-sm mx-auto">
        <Card className="rounded-2xl shadow-xl overflow-hidden">
          <CardHeader className="px-6 py-4">
            <div className="text-center">
              <h1 className="text-xl font-bold tracking-wide text-black dark:text-white">
                STROMSPAR-RANGLISTE
              </h1>
              <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                Der Punktestand sind die gesammelten Punkte der letzten 7 Tage.
              </p>
            </div>

          </CardHeader>
          <CardBody className="px-4 py-3 flex flex-col gap-3.5">
            {isLoading && (
              <div className="flex items-center justify-center h-32">
                <Spinner variant="wave" size="lg" color="white"/>
              </div>
            )}
            {!isLoading && sortedLeaderboard.map((user, idx) => {
              const { avatar, medal, color, text } = getAvatarAndMedal(idx);
              const isCurrentUser = user.userName === currentUserName;
              return (
                <Card
                  key={user.userName}
                  radius="lg"
                  shadow="sm"
                  isPressable
                  className={`bg-gray-50 dark:bg-zinc-800 hover:scale-[1.02] transition-transform duration-200 ${isCurrentUser ? "ring-2 ring-blue-500 dark:ring-blue-400" : ""
                    }`}
                >
                  <CardBody className="px-4 py-3">
                    <div className="flex items-center justify-between">
                      <div className="flex items-center gap-3 flex-1 min-w-0">
                        <span className={`w-8 h-8 rounded-full flex items-center justify-center font-bold text-sm ${color} ${text} shadow-inner`}>
                          {idx + 1}
                        </span>
                        <Avatar
                          src={avatar}
                          name={user.userName}
                          size="sm"
                          classNames={{ base: "w-9 h-9" }}
                          icon={<AvatarIcon />}
                        />
                        <span className="font-medium text-gray-900 dark:text-gray-200 text-base truncate">
                          {user.userName}

                        </span>
                      </div>
                      <div className="flex flex-col items-end min-w-[60px]">
                        <img
                          src={medal}
                          alt="Medaille"
                          className="w-7 h-7 mb-1 drop-shadow-md"
                        />
                        <span className="text-green-600 dark:text-green-400 font-semibold text-sm">
                          {user.pointIncreaseValue} P.
                        </span>
                      </div>
                    </div>
                  </CardBody>
                </Card>
              );
            })}

          </CardBody>
          {/* <CardFooter className="px-4 py-4 justify-center bg-gray-100 dark:bg-gray-700/50 rounded-b-2xl">
            <AvatarGroup>
              <Avatar src="/avatars/Katze.jpg" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Dog.jpg" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Vogel.webp" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Eidechse.webp" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/Maus.jpg" classNames={{ base: "w-8 h-8" }} />
              <Avatar src="/avatars/AvatarMan.png" classNames={{ base: "w-8 h-8" }} />
            </AvatarGroup>
          </CardFooter> */}
        </Card>
      </div>
    </div>
  );
}
