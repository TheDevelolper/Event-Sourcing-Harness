import React, { useState } from "react";
import { Button } from "../../atoms/Button/Button";
import { CardLayout } from "../../organisms/CardLayout/CardLayout";
import { Card } from "../../molecules/Card/Card";
import { MenuItem } from "@ui-models";

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
        <Button variant="solid" rounded={"none"}>More</Button>
      </div>
    </Card>
  );
};

export interface RestaurantMenuProps {
  items: Record<string, MenuItem[]>;
}

export const RestaurantMenu: React.FC<RestaurantMenuProps> = (
  props: RestaurantMenuProps
) => {
  const categories = Object.keys(props.items);
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
        {props.items[selectedCategory].map((item) => (
          <RestaurantMenuCard
            key={`menu-item-${item.id}`}
            item={item}
          ></RestaurantMenuCard>
        ))}
      </CardLayout>
    </div>
  );
};
