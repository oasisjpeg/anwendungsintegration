"use client";

import React, { useState } from "react";
import {
  ShoppingCartIcon,
  BoltIcon,
  EyeIcon,
  StarIcon,
  ArrowTopRightOnSquareIcon
} from "@heroicons/react/24/outline";

interface Product {
  name: string;
  image: string;
  description: string;
  price: string;
  originalPrice: string;
  rating: number;
  reviews: number;
  savings: string;
  link: string;
}

interface DropshippingWidgetProps {
  title?: string;
  productsList?: Product[];
}

const products: Product[] = [
  {
    name: "Smarte Steckdose",
    image: "/drop/steckdose.webp",
    description: "Automatisches Abschalten von Geräten – spart Energie.",
    price: "€24.99",
    originalPrice: "€39.99",
    rating: 4.8,
    reviews: 156,
    savings: "80% Energie sparen",
    link: "https://www.temu.com/at-en/smart-socket--smart-wifi-socket-with-20a-16a-plug-european-standard--remote-control-voice-control-via--google-home-timer-function-electricity--statistics-compatible-with-smartphones-g-601100636863449.html"
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
    link: "https://www.temu.com/at-en/6-pack--controlled-hexagonal-led-wall-lights-diy-assembly-energy--neutral--with-touch-night-light-plastic-material-wall-mounted-up-light--usb-powered-no-battery-included-g-601099727247303.html"
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
    link: "https://www.temu.com/at-en/1pc-multimeter-tester--digital-multimeter-with--ac-voltmeter-and-ohm-volt-amp-meter-measures-voltage-current--tests--continuity-g-601099564008562.html"
  }
];

export default function EnhancedDropshippingWidget({ 
  title = "⚡ Energie Sparen Leicht Gemacht", 
  productsList = products 
}: DropshippingWidgetProps) {
  const [hoveredProduct, setHoveredProduct] = useState<number | null>(null);
  const [favorites, setFavorites] = useState<Set<number>>(new Set<number>());

  const toggleFavorite = (index: number, e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    e.stopPropagation();
    const newFavorites = new Set(favorites);
    if (newFavorites.has(index)) {
      newFavorites.delete(index);
    } else {
      newFavorites.add(index);
    }
    setFavorites(newFavorites);
  };

  return (
  <div className="min-h-screen flex flex-col items-center">
    <div className="max-w-md w-full shadow-lg">
      <div className="relative overflow-hidden rounded-t-3xl p-8 text-center"
           style={{
             background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
             boxShadow: "0 20px 40px rgba(102, 126, 234, 0.3)"
           }}>
        <div className="absolute inset-0 bg-gradient-to-r from-white/10 to-transparent animate-pulse"></div>
        <div className="relative z-10">
          <h2 className="text-3xl font-bold text-white mb-2 tracking-wide">
            {title}
          </h2>
          <p className="text-white/80 text-sm">Smarte Lösungen für deinen Haushalt</p>
        </div>
        <div className="absolute top-4 left-4 w-8 h-8 bg-white/20 rounded-full animate-bounce"></div>
        <div className="absolute bottom-4 right-4 w-6 h-6 bg-white/20 rounded-full animate-bounce delay-300"></div>
      </div>
      <div className="bg-white rounded-b-3xl shadow-2xl p-6 space-y-4">
        {productsList.map((product, index) => (
          <div
            key={index}
            className="group relative overflow-hidden rounded-2xl border border-gray-100 transition-all duration-300 hover:shadow-xl hover:scale-[1.02] cursor-pointer"
            onMouseEnter={() => setHoveredProduct(index)}
            onMouseLeave={() => setHoveredProduct(null)}
            onClick={() => window.open(product.link, '_blank')}
          >
            <div className={`absolute inset-0 bg-gradient-to-r from-blue-50 to-purple-50 opacity-0 transition-opacity duration-300 ${
              hoveredProduct === index ? 'opacity-100' : ''
            }`}></div>
            
            <div className="relative p-4 flex items-center gap-4">
              <div className="relative flex-shrink-0">
                <div className="w-20 h-20 rounded-xl overflow-hidden bg-gray-50 flex items-center justify-center group-hover:scale-110 transition-transform duration-300">
                  <img
                    src={product.image}
                    alt={product.name}
                    className="w-full h-full object-contain"
                    onError={(e) => {
                      const target = e.target as HTMLImageElement;
                      target.style.display = 'none';
                      const nextElement = target.nextSibling as HTMLElement;
                      if (nextElement) {
                        nextElement.style.display = 'flex';
                      }
                    }}
                  />
                  <div className="hidden w-full h-full items-center justify-center bg-gradient-to-br from-gray-100 to-gray-200">
                    <BoltIcon className="w-8 h-8 text-gray-400" />
                  </div>
                </div>
              </div>
              <div className="flex-1 min-w-0">
                <div className="flex items-start justify-between mb-1">
                  <h3 className="font-bold text-gray-900 text-lg group-hover:text-blue-600 transition-colors">
                    {product.name}
                  </h3>
                  <button
                    onClick={(e) => toggleFavorite(index, e)}
                    className={`p-1 rounded-full transition-colors ${
                      favorites.has(index) ? 'text-red-500' : 'text-gray-400 hover:text-red-500'
                    }`}
                  >
                    <StarIcon className={`w-5 h-5 ${favorites.has(index) ? 'fill-current' : ''}`} />
                  </button>
                </div>
                <p className="text-gray-600 text-sm mb-2 leading-relaxed">
                  {product.description}
                </p>
                <div className="flex items-center gap-2 mb-2">
                  <div className="flex text-yellow-400">
                    {[...Array(5)].map((_, i) => (
                      <StarIcon key={i} className={`w-4 h-4 ${i < Math.floor(product.rating) ? 'fill-current' : ''}`} />
                    ))}
                  </div>
                  <span className="text-sm font-medium text-gray-700">{product.rating}</span>
                  <span className="text-sm text-gray-500">({product.reviews})</span>
                </div>
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-2">
                    <span className="text-2xl font-bold text-gray-900">{product.price}</span>
                    <span className="text-sm text-gray-500 line-through">{product.originalPrice}</span>
                  </div>
                  <div className="flex gap-2">
                    <button className="bg-gray-100 text-gray-600 p-2 rounded-lg hover:bg-gray-200 transition-colors group-hover:scale-110 duration-200">
                      <ArrowTopRightOnSquareIcon className="w-4 h-4" />
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <div className={`absolute bottom-0 left-0 h-1 bg-gradient-to-r from-blue-500 to-purple-600 transition-all duration-300 ${
              hoveredProduct === index ? 'w-full' : 'w-0'
            }`}></div>
          </div>
        ))}
        <div className="text-center pt-4 border-t border-gray-100">
          <p className="text-gray-600 text-sm mb-3">
            💡 Spare bis zu <span className="font-bold text-green-600">200€</span> pro Jahr bei deiner Stromrechnung
          </p>
        </div>
      </div>
    </div>
  </div>
  );
}
