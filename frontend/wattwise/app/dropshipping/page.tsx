"use client";

import React from "react";

const products = [
  {
    name: "Smarte Steckdose",
    image: "/drop/steckdose.webp",
    description: "Automatisches Abschalten von Geräten – spart Energie.",
    link: "https://www.temu.com/at-en/smart-socket--smart-wifi-socket-with-20a-16a-plug-european-standard--remote-control-voice-control-via--google-home-timer-function-electricity--statistics-compatible-with-smartphones-g-601100636863449.html?_oak_name_id=5595224077884499059&_oak_mp_inf=ENn3vK%2Bq1ogBGiBkZjUyODMxYjQxZTE0Y2M2OGFkMjM5NDNlNmFhNTZlYyC1iu%2BL8zI%3D&top_gallery_url=https%3A%2F%2Fimg.kwcdn.com%2Fproduct%2Ffancy%2F104d77bc-d81c-4326-ad4d-0feb1e59f4bc.jpg&spec_gallery_id=601100636863449&refer_page_sn=10009&refer_source=0&freesia_scene=2&_oak_freesia_scene=2&_oak_rec_ext_1=Nzcx&_oak_gallery_order=1631954873%2C587802833%2C1958021719%2C696013309%2C382414597&search_key=smarte%20steckdose&refer_page_el_sn=200049&_x_sessn_id=g05l6qyb6a&refer_page_name=search_result&refer_page_id=10009_1748881890662_7aui07barz"
  },
  {
    name: "LED-Lampen",
    image: "/drop/led.webp",
    description: "Bis zu 80% weniger Stromverbrauch als herkömmliche Lampen.",
    link: "https://www.temu.com/at-en/6-pack--controlled-hexagonal-led-wall-lights-diy-assembly-energy--neutral--with-touch-night-light-plastic-material-wall-mounted-up-light--usb-powered-no-battery-included-g-601099727247303.html?_oak_name_id=4784076766542167587&_oak_mp_inf=EMev3v2m1ogBGiA1OGJmYjhkM2I0ODI0YmJkOGNiMDZiZjg2NWNmY2Y1ZCDj0r6L8zI%3D&top_gallery_url=https%3A%2F%2Fimg.kwcdn.com%2Fproduct%2Ffancy%2F85ea7a7a-6ca8-40a8-90f6-98e9436c0559.jpg&spec_gallery_id=4735412027&refer_page_sn=10009&refer_source=0&freesia_scene=2&_oak_freesia_scene=2&_oak_rec_ext_1=MjUxMQ&_oak_gallery_order=1159553324%2C1868039155%2C2126061149%2C1952349980%2C21728791&search_key=led&refer_page_el_sn=200049&_x_sessn_id=g05l6qyb6a&refer_page_name=search_result&refer_page_id=10009_1748881097356_sbyszeytwn"
  },
  {
    name: "Strom-Messgerät",
    image: "/drop/messgerät.webp",
    description: "Finde Stromfresser in deinem Haushalt.",
    link: "https://www.temu.com/at-en/1pc-multimeter-tester--digital-multimeter-with--ac-voltmeter-and-ohm-volt-amp-meter-measures-voltage-current--tests--continuity-g-601099564008562.html?_oak_name_id=7732034619030890094&_oak_mp_inf=EPKI86%2Bm1ogBGiA4ODc1NzE5ODYxODk0YjNmYjUyOTc5MzM3MjNhYjZlOCCM%2FMOL8zI%3D&top_gallery_url=https%3A%2F%2Fimg.kwcdn.com%2Fproduct%2Ffancy%2F0996025d-7500-4b6a-9392-47d5aebb9438.jpg&spec_gallery_id=4156428141&refer_page_sn=10009&refer_source=0&freesia_scene=2&_oak_freesia_scene=2&_oak_rec_ext_1=Nzk2&_oak_gallery_order=202075234%2C1115059680%2C1593853%2C902863820%2C937360932&search_key=messger%C3%A4t%20strom&refer_page_el_sn=200049&_x_sessn_id=g05l6qyb6a&refer_page_name=search_result&refer_page_id=10009_1748881184553_f97ponbidk"
  }
];

export function DropshippingWidget({ title = "Empfohlene Produkte", productsList = products }) {
  return (
    <div
      className="w-full max-w-md mx-auto rounded-2xl shadow-lg p-6"
      style={{
        background: "linear-gradient(180deg, #6D5DFB 0%, #70C6FF 100%)"
      }}
    >
      <h2 className="text-2xl font-bold text-white text-center mb-5 tracking-widest">
        {title}
      </h2>
      <div className="flex flex-col gap-4">
        {productsList.map((prod, idx) => (
          <a
            href={prod.link}
            target="_blank"
            rel="noopener noreferrer"
            key={idx}
            className="flex items-center gap-3 bg-white/90 rounded-xl px-3 py-2 hover:bg-indigo-50 transition"
          >
            <img
              src={prod.image}
              alt={prod.name}
              className="w-12 h-12 object-contain rounded-lg"
            />
            <div className="flex-1">
              <div className="font-semibold text-gray-800">{prod.name}</div>
              <div className="text-xs text-gray-600">{prod.description}</div>
            </div>
          </a>
        ))}
      </div>
    </div>
  );
}