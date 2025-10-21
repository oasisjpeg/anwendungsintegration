"use client";

import { useEffect, useState, useRef } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import { apiGet } from "@/utils/api";
import { useCapacitorRouter } from '@/components/CapacitorRouter';
import { Spinner } from "@heroui/spinner";
import { Button } from "@heroui/button";
import { useRewardPoints } from "@/context/RewardPointsContext";
import { jwtDecode } from "jwt-decode";
import { storage } from "@/utils/capacitor";

interface ConsumptionRecord {
  id: string;
  userId: string;
  timestamp: string;
  kwValue: string;
}

interface RecommendationRecord {
  id: string;
  userId: string;
  created: string;
  kwValue: string;
}

interface DataPoint {
  timestamp: string;
  kwValue: string | null;
  recommended: string | null;
}

export default function Home() {
  const [data, setData] = useState<{ records: ConsumptionRecord[], total: number }>({ records: [], total: 0 });
  const [loading, setLoading] = useState(true); // Start with loading true
  const [authChecked, setAuthChecked] = useState(false); // Add state to track if auth check is complete
  const [email, setEmail] = useState("");
  const [loggedIn, setLoggedIn] = useState(false);
  const [recommendations, setRecommendations] = useState<RecommendationRecord[]>([]);
  const [mergedData, setMergedData] = useState<DataPoint[]>([]);

  const { navigateTo } = useCapacitorRouter();
  const { rewardPoints } = useRewardPoints();



  async function checkAuth() {
    try {
      const storedEmail = await storage.get("email");
      const token = await storage.get("token"); // assuming you store the JWT as "token"

      if (storedEmail && token) {
        try {
          const { exp } = jwtDecode(token); // exp is in seconds
          const now = Date.now() / 1000; // current time in seconds
          
          if (exp && exp > now) {
            setEmail(storedEmail);
            setLoggedIn(true);
            return true;
          } else {
            // Token expired
            setLoggedIn(false);
            await storage.remove("token");
            await storage.remove("email");
            console.warn("Token expired");
            return false;
          }
        } catch (e) {
          setLoggedIn(false);
          await storage.remove("token");
          await storage.remove("email");
          console.warn("Invalid token");
          return false;
        }
      } else {
        setLoggedIn(false);
        console.warn("No user data found in storage");
        return false;
      }
    } catch (error) {
      console.error("Error checking authentication:", error);
      setLoggedIn(false);
      return false;
    }
  }



  useEffect(() => {
    async function fetchData() {
      setLoading(true);
      const isLoggedIn = await checkAuth();
      setAuthChecked(true); // Mark auth check as complete

      if (!isLoggedIn) {
        setLoading(false);
        navigateTo("/login");
        return;
      }

      try {
        const [consumptionRes, recommendationsRes] = await Promise.all([
          apiGet<ConsumptionRecord[]>("/api/consumption-records/me"),
          apiGet<RecommendationRecord[]>("/api/recommend-records/me"),
        ]);

        const consumptionData = consumptionRes.data as ConsumptionRecord[];
        const recommendationData = recommendationsRes.data as RecommendationRecord[];

        const merged = mergeData(
          consumptionData,
          recommendationData
        );

        const totalKwh = consumptionData.reduce(
          (sum: number, record: ConsumptionRecord) => sum + Number(record.kwValue),
          0
        );

        setData({
          records: consumptionData,
          total: totalKwh,
        });
        setRecommendations(recommendationData);
        setMergedData(merged);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    fetchData();
  }, []);

  const generate24HourTimeline = () => {
    const timeline = [];
    const now = new Date();
    const currentHour = now.getHours();

    // Generate 24-hour timeline with current hour centered
    for (let i = -11; i <= 12; i++) {
      const hour = new Date(now);
      hour.setHours(currentHour + i);
      hour.setMinutes(0, 0, 0);
      timeline.push(hour.toISOString());
    }

    return timeline;
  };

  const mergeData = (consumption: any[], recommendations: any[]) => {
    const timeline = generate24HourTimeline();
    const now = new Date();

    return timeline.map((timestamp) => {
      const hour = new Date(timestamp).getHours();
      const isFuture = new Date(timestamp) > now;

      const consumptionRecord = consumption.find(
        (r) => new Date(r.timestamp).getHours() === hour
      );

      const recommendation = recommendations.find(
        (r) => new Date(r.created).getHours() === hour
      );
      return {
        timestamp,
        kwValue: consumptionRecord && !isFuture
          ? Number(consumptionRecord.kwValue).toFixed(2)
          : null,
        recommended: recommendation
          ? Number(recommendation.kwValue).toFixed(2)
          : null,
      };
    });
  };



  // Show loading spinner until auth check is complete
  if (!authChecked || loading) {
    return (
      <main className="min-h-screen bg-white dark:bg-black text-foreground flex items-center justify-center">
        <Spinner variant="wave" size="lg" color="white"/>
      </main>
    );
  }
  
  // Only show login message if auth is checked and user is not logged in
  if (authChecked && !loggedIn) {
    return (
      <main className="min-h-screen bg-white dark:bg-black text-foreground">
        <section className="px-4 pb-24 pt-4 max-w-md mx-auto">
          {/* Alert */}
          <div className="mb-4 rounded-lg border border-yellow-400 bg-yellow-100 px-4 py-3 text-yellow-800 dark:border-yellow-600 dark:bg-yellow-900 dark:text-yellow-100">
            ⚠️ Bitte logge dich ein, um fortzufahren.
          </div>

          {/* Login Box */}
          <Button
            onPress={() => navigateTo("/login")}
            className="bg-indigo-600 dark:bg-indigo-500 text-white font-semibold rounded-xl p-6 text-sm shadow-md"
            variant="flat"
          >
            Zum Login
          </Button>
        </section>
      </main>
    );
  }
  return (
    <main className="min-h-screen  dark:bg-black text-foreground">
      <section className="px-4 pb-24 pt-4 max-w-md mx-auto">
        <div className="bg-white dark:bg-zinc-900 shadow-lg rounded-3xl p-6 text-center mb-6">
          <h2 className="text-sm text-gray-500 dark:text-gray-400">
            Punktestand
          </h2>

          <p className="text-3xl font-bold text-indigo-600 dark:text-indigo-400">
            {rewardPoints} Punkte
          </p>
        </div>

        <div className="bg-white dark:bg-zinc-900 shadow-lg rounded-3xl p-6 mb-6">
          <div className="flex justify-between items-center mb-4">
            <div>
              <h3 className="text-sm text-gray-500 dark:text-gray-400">
                Statistik
              </h3>

              <h2 className="text-lg font-semibold text-gray-900 dark:text-white">
                Stromverbrauch
              </h2>
            </div>
            <div className="text-right">
              <p className="text-sm font-medium text-gray-800 dark:text-white">
                {data.total.toFixed(2)}Wh
              </p>
              <p className="text-xs text-green-500 font-semibold">+12.75%</p>
            </div>
          </div>

          <div className="w-full h-64">
            {!loading ? (
              <ResponsiveContainer width="100%" height="100%">
                <LineChart
                  data={mergedData}
                  margin={{ top: 15, right: 20, left: -15, bottom: 5 }}
                >
                  <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#e2e8f0" />
                  <XAxis
                    dataKey="timestamp"
                    tickFormatter={(time) =>
                      new Date(time).toLocaleTimeString([], {
                        hour: "2-digit",
                        minute: "2-digit",
                      })
                    }
                    tick={{ fontSize: 12, fill: "#9ca3af" }}
                  />
                  <YAxis
                    domain={[0, 3]}
                    tickCount={4}
                    tickFormatter={(tick) => `${tick} kWh`}
                    tick={{ fontSize: 12, fill: "#9ca3af" }}
                  />
                  <Tooltip
                    contentStyle={{
                      backgroundColor: "var(--tooltip-bg, #ffffff)",
                      border: "1px solid #e5e7eb",
                      borderRadius: "0.5rem",
                      color: "var(--tooltip-text, #111827)",
                    }}
                    labelStyle={{ color: "inherit" }}
                    labelFormatter={(label) =>
                      new Date(label).toLocaleTimeString([], {
                        hour: "2-digit",
                        minute: "2-digit",
                      })
                    }
                  />
                  <Line
                    type="monotone"
                    dataKey="kwValue"
                    stroke="#6366F1"
                    strokeWidth={3}
                    dot={{ r: 4 }}
                    connectNulls
                  />
                  <Line
                    type="monotone"
                    dataKey="recommended"
                    stroke="#32a852"
                    strokeWidth={2}
                    strokeDasharray="5 5"
                    dot={false}
                  />
                </LineChart>
              </ResponsiveContainer>
            ) : (
              <div className="flex items-center justify-center h-full">
                <Spinner variant="wave" size="lg" color="white"/>
              </div>
            )}
          </div>
        </div>

        {/* <div className="bg-white dark:bg-zinc-900 shadow-lg rounded-3xl p-6">
          <h3 className="text-sm font-semibold mb-1 text-gray-800 dark:text-white">
            Punkte sammeln
          </h3>
          <div className="flex justify-between items-center text-sm">
            <div>
              <p className="font-medium text-gray-800 dark:text-white">
                Energy Curtailment{" "}
                <span className="text-indigo-600 dark:text-indigo-400 font-semibold">
                  20p
                </span>
              </p>
              <p className="text-gray-500 dark:text-gray-400">
                Lese den Artikel und beantworte 3 Fragen um deine Punkte zu
                verdienen
              </p>
            </div>
            <Button
              className="bg-indigo-600 dark:bg-indigo-500 text-white font-semibold rounded-xl p-6 text-sm shadow-md"
              variant="flat"
            >
              Zum Artikel
            </Button>
          </div>
        </div> */}
      </section>
    </main>
  );
}
