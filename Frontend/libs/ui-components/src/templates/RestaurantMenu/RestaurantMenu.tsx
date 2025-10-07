import React, { useState } from "react";
import { Button } from "../../atoms/Button/Button";
import { CardLayout } from "../../organisms/CardLayout/CardLayout";
import { Card } from "../../molecules/Card/Card";
import { MenuItem } from "../../../../ui-models/src";

const sampleMenu: Record<string, MenuItem[]> = {
  Mains: [
    {
      id: 1,
      name: "Romana Chicken Pizza",
      description: "Classic Italian pizza with fresh tomatoes and mozzarella.",
      price: 12.99,
      image:
        "https://images.pexels.com/photos/29839587/pexels-photo-29839587.jpeg",
    },
    {
      id: 2,
      name: "Olive Chicken & Rocket Pizza",
      description: "Delicious pizza with olives, chicken, and rocket.",
      price: 13.99,
      image: "https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg",
    },
    {
      id: 3,
      name: "Pepperoni Feast Pizza",
      description: "Loaded with spicy pepperoni and mozzarella cheese.",
      price: 14.49,
      image: "https://images.pexels.com/photos/825661/pexels-photo-825661.jpeg",
    },

    {
      id: 4,
      name: "BBQ Chicken Pizza",
      description: "Grilled chicken, BBQ sauce, and red onions. ",
      price: 14.99,
      image:
        "https://images.pexels.com/photos/5639548/pexels-photo-5639548.jpeg",
    },
    {
      id: 5,
      name: "Margherita Pizza",
      description: "Fresh basil, tomatoes, and mozzarella on a thin crust.",
      price: 12.49,
      image:
        "https://images.pexels.com/photos/13814644/pexels-photo-13814644.jpeg",
    },
    {
      id: 6,
      name: "Four Cheese Pizza",
      description: "A blend of mozzarella, cheddar, parmesan, and blue cheese.",
      price: 15.49,
      image:
        "https://images.pexels.com/photos/33592983/pexels-photo-33592983.jpeg",
    },
    {
      id: 7,
      name: "Spicy Sausage Pizza",
      description: "Italian sausage, jalapeños, and mozzarella.",
      price: 14.99,
      image:
        "https://images.pexels.com/photos/27600445/pexels-photo-27600445.jpeg",
    },
    {
      id: 8,
      name: "Hawaiian Pizza",
      description: "Ham, pineapple, and mozzarella cheese.",
      price: 13.99,
      image:
        "https://images.pexels.com/photos/14334060/pexels-photo-14334060.jpeg",
    },
  ],
  Drinks: [
    {
      id: 10,
      name: "Coca Cola",
      description: "Refreshing soda drink.",
      price: 2.99,
      image: "https://images.pexels.com/photos/590598/pexels-photo-590598.jpeg",
    },
  ],
  Desserts: [
    {
      id: 11,
      name: "Tiramisu",
      description: "Classic Italian dessert.",
      price: 5.99,
      image: "https://images.pexels.com/photos/302680/pexels-photo-302680.jpeg",
    },
  ],
};

export interface RestaurantMenuCardProps {
  item: MenuItem;
}

export const RestaurantMenuCard: React.FC<RestaurantMenuCardProps> = ({
  item,
}) => {
  return (
    <Card
      key={item.id}
      padding="none"
      shadow
      className="overflow-hidden relative"
    >
      <div
        className="w-full h-60 bg-cover bg-center"
        style={{ backgroundImage: `url('${item.image}')` }}
      />

      <div className="p-4 pb-1">
        <div className="flex items-center justify-between">
          <h3 className="text-xl font-bold line-clamp-2">{item.name}</h3>
          <span className="text-lg font-semibold text-gray-800 self-start">
            {item.price.toFixed(2)}
          </span>
        </div>
      </div>

      <div className="p-4 pb-12 mb-4">
        <p className="text-gray-700 text-left line-clamp-2">
          {item.description}
        </p>
      </div>

      <div className="absolute right-4 bottom-4">
        <Button variant="primary">More</Button>
      </div>
    </Card>
  );
};

export const RestaurantMenu: React.FC = () => {
  const categories = Object.keys(sampleMenu);
  const [selectedCategory, setSelectedCategory] = useState(categories[0]);

  return (
    <div className="space-y-4">
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
          <RestaurantMenuCard
            key={`menu-item-${item.id}`}
            item={item}
          ></RestaurantMenuCard>
        ))}
      </CardLayout>
    </div>
  );
};
