import React from "react";
import type { Meta, StoryObj } from "@storybook/react-vite";
import { Card, CardProps } from "./Card";
import { Button } from "../../atoms/Button/Button";

const meta: Meta<typeof Card> = {
  title: "Molecules/Card",
  component: Card,
  tags: ["autodocs"],
};

export default meta;
type Story = StoryObj<typeof Card>;

export const ProductCardExample = (args: CardProps) => (
  <Card {...args} padding="none" shadow className="overflow-hidden relative">
    <div
      className="w-full h-40 bg-cover bg-center"
      style={{
        backgroundImage:
          "url('https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg')",
      }}
    />

    <div className="p-4 mb-2 h-12">
      <div className="flex items-center justify-between">
        <h3 className="text-xl font-bold">Romana Chicken Pizza</h3>
        <span className="text-lg font-semibold text-gray-800 self-start">
          12.99
        </span>
      </div>
    </div>

    {/* give content extra bottom padding so the absolute button won't overlap */}
    <div className="p-4 pb-12 mb-4">
      <p className="text-gray-700">
        Classic Italian pizza with fresh tomatoes, mozzarella cheese, and basil.
      </p>
    </div>

    {/* absolutely-placed button in bottom-right */}
    <div className="absolute right-4 bottom-4">
      <Button variant="primary">More</Button>
    </div>
  </Card>
);

export const Default: Story = {
  args: {
    children: "This is a simple card content.",
    variant: "default",
    padding: "md",
    shadow: true,
  } as CardProps,
};

export const Primary: Story = {
  args: {
    children: "Primary card content here.",
    variant: "primary",
    padding: "lg",
    shadow: true,
  } as CardProps,
};

export const NoShadow: Story = {
  args: {
    children: "Card without shadow.",
    shadow: false,
  } as CardProps,
};

export const ProductCard: Story = {
  args: {
    variant: "default",
    shadow: false,
    padding: "none",
  },

  render: (args) => ProductCardExample(args),
};
