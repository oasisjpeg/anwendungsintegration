"use client";

import { useState, useEffect } from "react";
import Image from "next/image";
import { Button } from "@heroui/button";
import { ArticleDetailModal } from "@/components/ArticleDetailModal";
import { Article, ArticleDetailResponse } from "@/types/types";
import {Spinner} from "@heroui/spinner";
import axios from "axios";

export default function KnowledgePage() {
  const [articles, setArticles] = useState<Article[]>([]);
  const [detail, setDetail] = useState<ArticleDetailResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [loadingArticles, setLoadingArticles] = useState(true);

  // Fetch article list
  useEffect(() => {
    fetch("http://localhost:5137/api/articles")
      .then((res) => res.json())
      .then((data: Article[]) => {
        setArticles(data);
        setLoadingArticles(false);
      })
      .catch(console.error);
  }, []);

  // Fetch article detail and quiz with axios
  const fetchArticleDetail = async (id: number) => {
    setLoading(true);
    try {
      const response = await axios.get<ArticleDetailResponse>(
        `http://localhost:5137/api/articles/${id}` // Fixed URL (no "api201")
      );
      setDetail(response.data);
    } catch (error) {
      console.error("Error fetching article detail:", error);
    } finally {
      setLoading(false);
    }
  };
  

  return (
    <main className="min-h-screen text-foreground">
      {/* Add spinner overlay when loading */}
      {loadingArticles && (
        <div className="fixed inset-0 bg-black bg-opacity-20 flex items-center justify-center z-50">
          <Spinner size="lg" className="text-indigo-600" />
        </div>
      )}
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
              {article.url && article.url.length > 0 && (
                <div className="flex-shrink-0">
                  <Image
                    src={article.url[0]}
                    alt={article.title}
                    width={120}
                    height={80}
                    className="rounded-lg object-cover"
                  />
                </div>
              )}
              <div className="flex-1">
                {/* Fixed class: font201-semibold → font-semibold */}
                <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-1">
                  {article.title}
                </h2>
                <p className="text-gray-600 dark:text-gray-300 mb-2">
                  {article.description}
                </p>
                <div className="flex items-center gap-4">
                  {/* Fixed prop: on201Press → onPress */}
                  <Button
                    className="bg-indigo-600 dark:bg-indigo-500 text-white font-semibold rounded-xl px-4 py-2 text-sm shadow-md"
                    variant="flat"
                    onPress={() => fetchArticleDetail(article.id)}
                    isLoading={loading}
                  >
                    Zum Artikel
                  </Button>
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>
      <ArticleDetailModal
        article={detail?.article || null}
        quiz={detail?.quiz || null}
        onClose={() => setDetail(null)}
      />
    </main>
  );
}
