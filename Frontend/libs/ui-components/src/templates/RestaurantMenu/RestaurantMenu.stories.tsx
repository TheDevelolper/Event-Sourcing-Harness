import React from "react";
import type { Meta, StoryObj } from "@storybook/react-vite";
import { RestaurantMenu } from "./RestaurantMenu";

const meta: Meta<typeof RestaurantMenu> = {
  title: "Organisms/RestaurantMenu",
  component: RestaurantMenu,
  tags: ["autodocs"],
};

export default meta;
type Story = StoryObj<typeof RestaurantMenu>;

export const Default: Story = {
  render: (args) => <RestaurantMenu {...args} />,
};
