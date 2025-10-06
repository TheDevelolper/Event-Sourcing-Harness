import React, { useState } from "react";
import { Button } from "../../atoms/Button/Button";
import { CardLayout } from "../../organisms/CardLayout/CardLayout";
import { Card } from "../../molecules/card/Card";

export interface MenuItem {
  id: number;
  name: string;
  description: string;
  price: number;
  image: string;
}

const sampleMenu: Record<string, MenuItem[]> = {
  Mains: [
    {
      id: 1,
      name: "Romana Chicken Pizza",
      description: "Classic Italian pizza with fresh tomatoes and mozzarella.",
      price: 12.99,
      image: "https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg",
    },
    {
      id: 2,
      name: "Olive Chicken & Rocket Pizza",
      description: "Delicious pizza with olives, chicken, and rocket.",
      price: 13.99,
      image: "https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg",
    },
  ],
  Drinks: [
    {
      id: 3,
      name: "Coca Cola",
      description: "Refreshing soda drink.",
      price: 2.99,
      image: "https://images.pexels.com/photos/590598/pexels-photo-590598.jpeg",
    },
  ],
  Desserts: [
    {
      id: 4,
      name: "Tiramisu",
      description: "Classic Italian dessert.",
      price: 5.99,
      image: "https://images.pexels.com/photos/302680/pexels-photo-302680.jpeg",
    },
  ],
};

export const RestaurantMenu: React.FC = () => {
  const categories = Object.keys(sampleMenu);
  const [selectedCategory, setSelectedCategory] = useState(categories[0]);

  return (
    <div className="space-y-4 ">
      {/* Category selector */}
      <div className="flex justify-end mb-8">
        <label className="align-center p-2 pr-4" htmlFor="select-menu">
          Choose Menu:{" "}
        </label>

        <div className="relative w-full max-w-xs">
          <select
            id="select-menu"
            title="Select Menu"
            value={selectedCategory}
            onChange={(e) => setSelectedCategory(e.target.value)}
            className=" block w-full rounded-md border border-gray-300 bg-white py-2 px-3 text-gray-700 shadow-sm 
            focus:border-primary-500 focus:ring focus:ring-primary-200 focus:ring-opacity-50 transition cursor-pointer appearance-none"
          >
            {categories.map((cat) => (
              <option key={cat} value={cat}>
                {cat}
              </option>
            ))}
          </select>

          {/* Custom chevron */}
          <div className="pointer-events-none absolute inset-y-0 right-3 flex items-center">
            <svg
              className="w-4 h-4 text-gray-500"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M19 9l-7 7-7-7"
              ></path>
            </svg>
          </div>
        </div>
      </div>
      {/* Item grid */}
      <CardLayout className="max-w-300">
        {sampleMenu[selectedCategory].map((item) => (
          <Card
            key={item.id}
            padding="none"
            shadow
            className="overflow-hidden relative" // <-- add relative
          >
            <div
              className="w-full h-40 bg-cover bg-center"
              style={{ backgroundImage: `url('${item.image}')` }}
            />

            <div className="p-4 mb-2 h-12">
              <div className="flex items-center justify-between">
                <h3 className="text-xl font-bold">{item.name}</h3>
                <span className="text-lg font-semibold text-gray-800 self-start">
                  {item.price.toFixed(2)}
                </span>
              </div>
            </div>

            {/* give content extra bottom padding so the absolute button won't overlap */}
            <div className="p-4 pb-12 mb-4">
              <p className="text-gray-700">{item.description}</p>
            </div>

            {/* absolutely-placed button in bottom-right */}
            <div className="absolute right-4 bottom-4">
              <Button variant="primary">More</Button>
            </div>
          </Card>
        ))}
      </CardLayout>
    </div>
  );
};
