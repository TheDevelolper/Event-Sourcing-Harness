import React from "react";
import type { Meta, StoryObj } from "@storybook/react-vite";
import { Card, CardProps } from "./Card";
import { Button } from "../../atoms/button/Button";

const meta: Meta<typeof Card> = {
  title: "Organisms/Card",
  component: Card,
  tags: ["autodocs"],
};

export default meta;
type Story = StoryObj<typeof Card>;

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

  render: (args) => (
    <Card {...args} className="overflow-hidden w-90 h-100">
      {/* Image section */}
      <div
        className="w-full h-1/2 bg-cover bg-center"
        style={{
          backgroundImage:
            "url('https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg')",
        }}
      ></div>

      {/* Text section */}
      <div className="p-4 pb-0">
        <div className="flex items-center justify-between">
          <h3 className="text-xl font-bold">Romana Chicken Pizza</h3>
          <span className="text-lg font-semibold text-gray-800">12.99</span>
        </div>
      </div>
      <div className="p-4">
        <p className="text-gray-700">
          Classic Italian pizza with fresh tomatoes, mozzarella cheese, and
          basil. Perfectly baked with a thin, crispy crust.
        </p>

        <div className="flex justify-end">
          <Button variant="primary">More</Button>
        </div>
      </div>
    </Card>
  ),
};
