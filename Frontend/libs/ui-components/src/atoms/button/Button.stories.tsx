import React from "react";
import type { Meta, StoryObj } from "@storybook/react-vite";
import { Button, ButtonProps } from "./Button";

const meta: Meta<typeof Button> = {
  title: "Atoms/Button",
  component: Button,
  tags: ["autodocs"],
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
    variant: "primary",
    size: "md",
  },
};

export const Secondary: Story = {
  args: {
    children: "Secondary Button",
    variant: "secondary",
    size: "md",
  },
};

export const Info: Story = {
  args: {
    children: "Info Button",
    variant: "info",
    size: "md",
  },
};

export const Success: Story = {
  args: {
    children: "Success Button",
    variant: "success",
    size: "md",
  },
};

export const Warning: Story = {
  args: {
    children: "Warning Button",
    variant: "warning",
    size: "md",
  },
};

export const Danger: Story = {
  args: {
    children: "Danger Button",
    variant: "danger",
    size: "md",
  },
};
