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
import { title, subtitle } from "@/components/primitives";
import { ThemeSwitch } from "@/components/theme-switch";

export default function Home() {
  const [data, setData] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(
          "http://localhost:5137/api/consumption-records/1"
        ); // Change to your API endpoint
        const result = await response.json();
        // Filter only values up to the current hour
        const now = new Date();
        const currentHour = now.getHours();
        const filteredData = result.filter(
          (record) => new Date(record.timestamp).getHours() <= currentHour
        );

        setData(filteredData);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    fetchData();
  }, []);

  return (
    <section className="flex flex-col items-center justify-center gap-4 py-8 md:py-10">
      <div className="inline-block max-w-xl text-center justify-center">
        <p className={title({ class: "mt-4" })}>Energy Consumption</p>
        <p className={subtitle({ class: "mt-4" })}>Hourly kWh Usage</p>
      </div>

      <div className="absolute top-0 left-0">
        <ThemeSwitch />
      </div>

      <div className="w-full max-w-3xl h-80 mt-8">
        <ResponsiveContainer width="100%" height="100%">
        <LineChart data={data} margin={{ top: 5, right: 20, left: 10, bottom: 5 }}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="timestamp"
              tickFormatter={(time) => new Date(time).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
            />
            <YAxis domain={[0, 3]} tickCount={4} tickFormatter={(tick) => `${tick} kWh`} interval={0} />
            <Tooltip labelFormatter={(label) => new Date(label).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} />
            <Line type="monotone" dataKey="kWValue" stroke="#8884d8" strokeWidth={2} />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </section>
  );
}
