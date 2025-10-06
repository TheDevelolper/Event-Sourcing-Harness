import React, { useState } from "react";
import { CardLayout } from "../../organisms/CardLayout/CardLayout";
import { Card } from "../../molecules/Card/Card";
import { Button } from "../../atoms/Button/Button";

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
    <div className="space-y-4">
      {/* Category selector */}
      <select
        value={selectedCategory}
        onChange={(e) => setSelectedCategory(e.target.value)}
      >
        {categories.map((cat) => (
          <option key={cat} value={cat}>
            {cat}
          </option>
        ))}
      </select>

      {/* Item grid */}
      <CardLayout>
        {sampleMenu[selectedCategory].map((item) => (
          <Card key={item.id} padding="none" shadow className="overflow-hidden">
            <div
              className="w-full h-40 bg-cover bg-center"
              style={{ backgroundImage: `url('${item.image}')` }}
            />
            <div className="p-4 pb-0">
              <div className="flex items-center justify-between">
                <h3 className="text-xl font-bold">{item.name}</h3>
                <span className="text-lg align-top font-semibold text-gray-800 self-start">
                  {item.price.toFixed(2)}
                </span>
              </div>
            </div>
            <div className="p-4">
              <p className="text-gray-700">{item.description}</p>
              <div className="flex justify-end">
                <Button variant="primary">More</Button>
              </div>
            </div>
          </Card>
        ))}
      </CardLayout>
    </div>
  );
};
