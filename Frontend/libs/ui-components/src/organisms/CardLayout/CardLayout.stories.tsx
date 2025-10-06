import React from "react";
import type { Meta, StoryObj } from "@storybook/react-vite";
import { CardLayout, CardLayoutProps } from "./CardLayout";
import { Card } from "../../molecules/Card/Card";
import { Button } from "../../atoms/Button/Button";

const meta: Meta<typeof CardLayout> = {
  title: "Organisms/CardLayout",
  component: CardLayout,
  tags: ["autodocs"],
};

export default meta;
type Story = StoryObj<typeof CardLayout>;

// A simple ProductCard component for the story
const ProductCardExample = () => (
  <Card
    padding="none"
    shadow
    className="overflow-hidden w-full mx-auto min-w-[33%]"
  >
    <div
      className="w-full h-40 bg-cover bg-center"
      style={{
        backgroundImage:
          "url('https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg')",
      }}
    ></div>
    <div className="p-4 pb-0">
      <div className="flex items-center justify-between">
        <h3 className="text-xl font-bold">Romana Chicken Pizza</h3>
        <span className="text-lg font-semibold text-gray-800 self-start">12.99</span>
      </div>
    </div>
    <div className="p-4">
      <p className="text-gray-700">
        Classic Italian pizza with fresh tomatoes, mozzarella cheese, and basil.
      </p>
      <div className="flex justify-end">
        <Button variant="primary">More</Button>
      </div>
    </div>
  </Card>
);

export const Default: Story = {
  args: {
    gap: "md",
    className: "md:min-w-[30rem] max-w-[80rem] mx-auto",
    children: [
      <ProductCardExample key={1} />,
      <ProductCardExample key={2} />,
      <ProductCardExample key={3} />,
      <ProductCardExample key={4} />,
      <ProductCardExample key={5} />,
    ],
  } as CardLayoutProps,
};
