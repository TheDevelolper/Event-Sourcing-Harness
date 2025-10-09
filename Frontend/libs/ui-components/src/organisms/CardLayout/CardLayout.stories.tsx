import type { Meta, StoryObj } from "@storybook/react-vite";
import { ProductCardExample } from "../../molecules/card/Card.stories";
import { CardLayout, CardLayoutProps } from "./CardLayout";

const meta: Meta<typeof CardLayout> = {
  title: "Organisms/Card Layout",
  component: CardLayout,
  tags: ["autodocs"],
};

export default meta;
type Story = StoryObj<typeof CardLayout>;

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
