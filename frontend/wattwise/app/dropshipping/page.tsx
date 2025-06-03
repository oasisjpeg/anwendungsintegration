"use client";

import React, { useState } from "react";
import {
  ShoppingCartIcon,
  BoltIcon,
  EyeIcon,
  StarIcon,
  ArrowTopRightOnSquareIcon,
} from "@heroicons/react/24/outline";

const products = [
  {
    name: "Smarte Steckdose",
    image: "/drop/steckdose.webp",
    description: "Automatisches Abschalten von Geräten – spart Energie.",
    price: "€24.99",
    originalPrice: "€39.99",
    rating: 4.8,
    reviews: 156,
    savings: "80% Energie sparen",
    link: "https://www.temu.com/at-en/smart-socket--smart-wifi-socket-with-20a-16a-plug-european-standard--remote-control-voice-control-via--google-home-timer-function-electricity--statistics-compatible-with-smartphones-g-601100636863449.html",
  },
  {
    name: "LED-Lampen Set",
    image: "/drop/led.webp",
    description: "Bis zu 80% weniger Stromverbrauch als herkömmliche Lampen.",
    price: "€19.99",
    originalPrice: "€49.99",
    rating: 4.6,
    reviews: 89,
    savings: "6er Pack",
    link: "https://www.temu.com/at-en/6-pack--controlled-hexagonal-led-wall-lights-diy-assembly-energy--neutral--with-touch-night-light-plastic-material-wall-mounted-up-light--usb-powered-no-battery-included-g-601099727247303.html",
  },
  {
    name: "Strom-Messgerät",
    image: "/drop/messgerät.webp",
    description: "Finde Stromfresser in deinem Haushalt.",
    price: "€15.99",
    originalPrice: "€29.99",
    rating: 4.7,
    reviews: 203,
    savings: "Digital Display",
    link: "https://www.temu.com/at-en/1pc-multimeter-tester--digital-multimeter-with--ac-voltmeter-and-ohm-volt-amp-meter-measures-voltage-current--tests--continuity-g-601099564008562.html",
  },
  // Add a 4th product if you want a perfect 2x2 grid, or remove this if you want a 2x2 with empty space
  {
    name: "Smart Plug (Example)",
    image: "/drop/steckdose.webp", // replace with your image
    description: "Steuere deine Geräte von überall.",
    price: "€29.99",
    originalPrice: "€44.99",
    rating: 4.5,
    reviews: 120,
    savings: "Energie sparen",
    link: "#",
  },
];

export default function EnhancedDropshippingWidget({
  title = "⚡ Energie Sparen Leicht Gemacht",
  productsList = products,
}) {
  const [hoveredProduct, setHoveredProduct] = useState<number | null>(null);
  const [favorites, setFavorites] = useState<Set<number>>(new Set());

  const toggleFavorite = (index: number, e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    const newFavorites = new Set(favorites);
    newFavorites.has(index) ? newFavorites.delete(index) : newFavorites.add(index);
    setFavorites(newFavorites);
  };

  return (
    <div className="min-h-screen flex flex-col items-center pb-20">
      <div className="w-full max-w-2xl sm:max-w-2xl md:max-w-2xl lg:max-w-2xl shadow-lg">
        {/* Header with gradient */}
        <div
          className="relative overflow-hidden dark:bg-zinc-900 rounded-t-3xl p-4 sm:p-6 lg:p-8 text-center"
        >
          <div className="relative z-10">
            <h2 className="text-xl sm:text-2xl lg:text-3xl font-bold text-black dark:text-white mb-2 tracking-wide leading-tight">
              {title}
            </h2>
            <p className="text-black dark:text-white text-xs sm:text-sm">Smarte Lösungen für deinen Haushalt</p>
          </div>
        </div>

        {/* Product grid */}
        <div className="bg-white dark:bg-zinc-900 rounded-b-3xl shadow-2xl p-3 sm:p-4 lg:p-6">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            {productsList.map((product, index) => (
              <div
                key={index}
                className="group relative overflow-hidden rounded-xl border border-gray-100 dark:border-gray-700 transition-all duration-300 hover:shadow-xl hover:scale-[1.02] cursor-pointer"
                onMouseEnter={() => setHoveredProduct(index)}
                onMouseLeave={() => setHoveredProduct(null)}
                onClick={() => window.open(product.link, "_blank")}
              >
                <div
                  className={`absolute inset-0 bg-gradient-to-r from-blue-50 to-purple-50 dark:from-blue-900/30 dark:to-purple-900/30 opacity-0 transition-opacity duration-300 ${
                    hoveredProduct === index ? "opacity-100" : ""
                  }`}
                ></div>
                <div className="relative p-3 sm:p-4">
                  <div className="flex flex-col sm:flex-row sm:items-center gap-3">
                    <div className="relative flex-shrink-0">
                      <div className="w-16 h-16 rounded-lg overflow-hidden bg-gray-50 dark:bg-gray-800 flex items-center justify-center group-hover:scale-110 transition-transform duration-300">
                        <img
                          src={product.image}
                          alt={product.name}
                          className="w-full h-full object-contain"
                          onError={(e) => {
                            const target = e.target as HTMLImageElement;
                            target.style.display = "none";
                            const fallback = target.nextElementSibling as HTMLElement;
                            fallback.style.display = "flex";
                          }}
                        />
                        <div className="hidden w-full h-full items-center justify-center bg-gradient-to-br from-gray-100 to-gray-200 dark:from-gray-700 dark:to-gray-600">
                          <BoltIcon className="w-6 h-6 text-gray-400" />
                        </div>
                      </div>
                    </div>
                    <div className="flex-1 min-w-0">
                      <div className="flex items-start justify-between">
                        <h3 className="font-bold text-gray-900 dark:text-white text-base group-hover:text-blue-600 dark:group-hover:text-blue-400 transition-colors truncate">
                          {product.name}
                        </h3>
                      </div>
                      <p className="text-gray-600 dark:text-gray-300 text-sm leading-relaxed">
                        {product.description}
                      </p>
                      <div className="flex items-center gap-1 mt-1">
                        <div className="flex text-yellow-400">
                          {[...Array(5)].map((_, i) => (
                            <StarIcon
                              key={i}
                              className={`w-3 h-3 ${i < Math.floor(product.rating) ? "fill-current" : ""}`}
                            />
                          ))}
                        </div>
                        <span className="text-xs font-medium text-gray-700 dark:text-gray-300">
                          {product.rating}
                        </span>
                        <span className="text-xs text-gray-500 dark:text-gray-400">
                          ({product.reviews})
                        </span>
                      </div>
                      <div className="flex items-center justify-between mt-2">
                        <div className="flex items-center gap-2">
                          <span className="text-lg font-bold text-gray-900 dark:text-white">
                            {product.price}
                          </span>
                          <span className="text-xs text-gray-500 dark:text-gray-400 line-through">
                            {product.originalPrice}
                          </span>
                        </div>
                        <button className="bg-gray-100 dark:bg-gray-800 text-gray-600 dark:text-gray-300 p-2 rounded-lg hover:bg-gray-200 dark:hover:bg-gray-700 transition-colors group-hover:scale-110 duration-200">
                          <ArrowTopRightOnSquareIcon className="w-4 h-4" />
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
                <div
                  className={`absolute bottom-0 left-0 h-1 bg-gradient-to-r from-blue-500 to-purple-600 transition-all duration-300 ${
                    hoveredProduct === index ? "w-full" : "w-0"
                  }`}
                ></div>
              </div>
            ))}
          </div>
          <div className="text-center pt-4 border-t border-gray-100 dark:border-gray-700 mt-4">
            <p className="text-gray-600 dark:text-gray-300 text-xs sm:text-sm mb-3">
              💡 Spare bis zu <span className="font-bold text-green-600 dark:text-green-400">200€</span> pro Jahr bei deiner Stromrechnung
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
