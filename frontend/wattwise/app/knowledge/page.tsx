"use client";

import { useRouter } from "next/navigation";
import Image from "next/image";
import { Button } from "@heroui/button";

// Hardcoded articles JSON
const articles = [
  {
    id: 1,
    title: "Understanding the Duck Curve",
    meta: {
      tags: ["energy", "grid"],
      author: "Energy Team",
      description: "Learn what the duck curve is and how it affects your household energy consumption."
    },
    images: [
      {
        url: "/images/duck_curve.jpg",
        caption: "Typical duck curve graph"
      }
    ],
    points: 20
  },
  {
    id: 2,
    title: "Appliance Efficiency Tips",
    meta: {
      tags: ["savings", "appliances"],
      author: "Energy Team",
      description: "Discover how to save energy by using your appliances more efficiently."
    },
    images: [
      {
        url: "/images/duck_curve.jpg",
        caption: "Efficient home appliances"
      }
    ],
    points: 15
  }
];

export default function KnowledgePage() {
  const router = useRouter();

  return (
    <main className="min-h-screen bg-white dark:bg-black text-foreground">
      <section className="max-w-2xl mx-auto px-4 py-12">
        <h1 className="text-3xl font-bold mb-4 text-center">Wissensdatenbank</h1>
        <div className="space-y-6 mb-12">
          {articles.length === 0 && (
            <div className="text-center text-gray-500">Keine Artikel gefunden.</div>
          )}
          {articles.map((article) => (
            <div
              key={article.id}
              className="bg-white dark:bg-zinc-900 shadow-lg rounded-2xl p-6 flex flex-col md:flex-row items-center gap-4"
            >
              {article.images && article.images.length > 0 && (
                <div className="flex-shrink-0">
                  <Image
                    src={article.images[0].url}
                    alt={article.images[0].caption || article.title}
                    width={120}
                    height={80}
                    className="rounded-lg object-cover"
                  />
                </div>
              )}
              <div className="flex-1">
                <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-1">
                  {article.title}
                </h2>
                <p className="text-gray-600 dark:text-gray-300 mb-2">
                  {article.meta?.description || "Kein Beschreibungstext vorhanden."}
                </p>
                <div className="flex items-center gap-2 text-sm mb-2">
                  {article.meta?.tags &&
                    article.meta.tags.map((tag) => (
                      <span
                        key={tag}
                        className="bg-indigo-100 text-indigo-700 px-2 py-0.5 rounded"
                      >
                        {tag}
                      </span>
                    ))}
                </div>
                <div className="flex items-center gap-4">
                  <Button
                    className="bg-indigo-600 dark:bg-indigo-500 text-white font-semibold rounded-xl px-4 py-2 text-sm shadow-md"
                    variant="flat"
                    onPress={() => router.push(`/knowledge/${article.id}`)}
                  >
                    Zum Artikel
                  </Button>
                  {article.points && (
                    <span className="text-indigo-600 dark:text-indigo-400 font-semibold">
                      {article.points}p
                    </span>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>
    </main>
  );
}
