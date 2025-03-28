"use client";

import { useEffect, useState } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import axios, { Axios } from "axios";
import { useRouter } from "next/navigation";
import { Spinner } from "@heroui/spinner";
import { Button } from "@heroui/button";

export default function Home() {
  const [data, setData] = useState({ records: [], total: 0 });
  const [loading, setLoading] = useState(false);
  const [email, setEmail] = useState("");
  const [loggedIn, setLoggedIn] = useState(false);

  const router = useRouter();

  async function checkAuth() {
    const storedEmail = localStorage.getItem("email");
    if (storedEmail) {
      setEmail(storedEmail);
      setLoggedIn(true);
      return true
    } else {
      setLoggedIn(false);
      console.warn("No user data found in localStorage");
      return false
    }
  }


  useEffect(() => {
    async function fetchData() {
      setLoading(true);
      const isLoggedIn = await checkAuth();
  
      if (!isLoggedIn) {
        console.warn("User is not logged in!");
        setLoading(false);
        return;
      }

      try {
        const response = await axios.get(
          "http://localhost:5137/api/consumption-records/1"
        );
        console.log(response.data);
        const result = response.data;
        const now = new Date();
        const currentHour = now.getHours();
        const filteredData = result.filter(
          (record) => new Date(record.timestamp).getHours() <= currentHour
        );
        console.log("Filtered Data:", filteredData);

        const totalKwh = filteredData.reduce(
          (sum, record) => sum + record.kWValue,
          0
        );

        console.log("Total kWh:", totalKwh);
        setData({
          records: filteredData,
          total: totalKwh,
        });
        setLoading(false);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    fetchData();
  }, []);

  if (!loggedIn) {
    return (
      <main className="min-h-screen bg-white dark:bg-black text-foreground">
        <section className="px-4 pb-24 pt-4 max-w-md mx-auto">
          {/* Alert */}
          <div className="mb-4 rounded-lg border border-yellow-400 bg-yellow-100 px-4 py-3 text-yellow-800 dark:border-yellow-600 dark:bg-yellow-900 dark:text-yellow-100">
            ⚠️ Bitte logge dich ein, um fortzufahren.
          </div>

          {/* Login Box */}
          <Button
            onPress={() => router.push("/logi")}
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
            Ersparnis
          </h2>
          <p className="text-3xl font-bold text-indigo-600 dark:text-indigo-400">
            2500 P
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
                {data.total}W
              </p>
              <p className="text-xs text-green-500 font-semibold">+12.75%</p>
            </div>
          </div>

          <div className="w-full h-64">
            {!loading ? (
              <ResponsiveContainer width="100%" height="100%">
                <LineChart
                  data={data.records}
                  margin={{ top: 15, right: 20, left: -15, bottom: 5 }}
                >
                  <CartesianGrid
                    strokeDasharray="3 3"
                    vertical={false}
                    stroke="#e2e8f0"
                  />
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
                      backgroundColor: "var(--tooltip-bg, #ffffff)", // fallback white
                      border: "1px solid #e5e7eb", // light border
                      borderRadius: "0.5rem",
                      color: "var(--tooltip-text, #111827)", // fallback dark text
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
                    dataKey="kWValue"
                    stroke="#6366F1"
                    strokeWidth={3}
                    dot={{ r: 4 }}
                  />
                </LineChart>
              </ResponsiveContainer>
            ) : (
              <div className="flex items-center justify-center h-full">
                <Spinner variant="dots" size="lg" />
              </div>
            )}
          </div>
        </div>

        <div className="bg-white dark:bg-zinc-900 shadow-lg rounded-3xl p-6">
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
        </div>
      </section>
    </main>
  );
}
