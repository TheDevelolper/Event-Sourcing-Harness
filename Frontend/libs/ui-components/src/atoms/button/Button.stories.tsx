import React from "react";
import type { Meta, StoryObj } from "@storybook/react-vite";
import { Button, ButtonProps } from "./Button";

const a11y = {
  // options passed to axe-core
  context: ".sb-show-main",
  config: {
    rules: [
      { id: "color-contrast", enabled: true },
      { id: "label", enabled: false }, // disable specific rule
    ],
  },
};

const meta: Meta<typeof Button> = {
  title: "Atoms/Button",
  component: Button,
  tags: ["autodocs"],

  parameters: {
    a11y,
  },
  argTypes: {
    variant: {
      control: { type: "radio" },
      options: ["primary", "secondary", "danger"],
    },
    size: {
      control: { type: "radio" },
      options: ["sm", "md", "lg"],
    },
  },
};

export default meta;
type Story = StoryObj<typeof Button>;

export const Primary: Story = {
  args: {
    children: "Primary Button",
    color: "primary",
    size: "md",
  },
};

export const Secondary: Story = {
  args: {
    children: "Secondary Button",
    color: "secondary",
    size: "md",
  },
};

export const Info: Story = {
  args: {
    children: "Info Button",
    color: "info",
    size: "md",
  },
};

export const Success: Story = {
  args: {
    children: "Success Button",
    color: "success",
    size: "md",
  },
};

export const Warning: Story = {
  args: {
    children: "Warning Button",
    color: "warning",
    size: "md",
  },
};

export const Danger: Story = {
  args: {
    children: "Danger Button",
    color: "danger",
    size: "md",
  },
};
