import type { Meta, StoryObj } from "@storybook/react-vite";
import { Button } from "./Button";

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
};

export default meta;
type Story = StoryObj<typeof Button>;

export const Examples: Story = {
  args: {
    children: "Example",
    size: "md",
    variant: "solid",
    rounded: "sm",
  },
  argTypes: {
    size: { control: "select" },
    variant: { control: "select" }, // <-- hides the color control
    color: { control: false, table: { disable: true } }, // <-- hides the color control
    rounded: { control: "radio" },
  },

  render: (args) => {
    return (
      <div className="flex gap-2">
        <Button
          rounded={args.rounded}
          color="brand"
          size={args.size}
          variant={args.variant}
        >
          {args.children}
        </Button>
        <Button
          rounded={args.rounded}
          color="info"
          size={args.size}
          variant={args.variant}
        >
          {args.children}
        </Button>
        <Button
          rounded={args.rounded}
          color="success"
          size={args.size}
          variant={args.variant}
        >
          {args.children}
        </Button>
        <Button
          rounded={args.rounded}
          color="warning"
          size={args.size}
          variant={args.variant}
        >
          {args.children}
        </Button>
        <Button
          rounded={args.rounded}
          color="danger"
          size={args.size}
          variant={args.variant}
        >
          {args.children}
        </Button>
      </div>
    );
  },
};

export const Solid: Story = {
  args: {
    children: "Button",
    variant: "solid",
  },
  argTypes: {
    children: { table: { disable: true } },
    color: { table: { disable: true } },
    variant: { table: { disable: true } },
    size: { table: { disable: true } },
  },

  render: (args) => {
    return (
      <div className="flex flex-col gap-2">
        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="lg"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="lg"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="lg"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="lg"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="lg"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>

        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="md"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="md"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="md"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="md"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="md"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>

        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="sm"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="sm"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="sm"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="sm"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="sm"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>
      </div>
    );
  },
};

export const Outline: Story = {
  args: {
    children: "Button",
    variant: "outline",
  },
  argTypes: {
    children: { table: { disable: true } },
    color: { table: { disable: true } },
    variant: { table: { disable: true } },
    size: { table: { disable: true } },
  },

  render: (args) => {
    return (
      <div className="flex flex-col gap-2">
        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="lg"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="lg"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="lg"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="lg"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="lg"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>

        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="md"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="md"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="md"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="md"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="md"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>

        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="sm"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="sm"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="sm"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="sm"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="sm"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>
      </div>
    );
  },
};

export const SemiOpaque: Story = {
  args: {
    children: "Button",
    variant: "semiOpaque",
  },
  argTypes: {
    children: { table: { disable: true } },
    color: { table: { disable: true } },
    variant: { table: { disable: true } },
    size: { table: { disable: true } },
  },

  render: (args) => {
    return (
      <div className="flex flex-col gap-2">
        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="lg"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="lg"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="lg"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="lg"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="lg"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>

        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="md"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="md"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="md"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="md"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="md"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>

        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="sm"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="sm"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="sm"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="sm"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="sm"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>
      </div>
    );
  },
};

export const Ghost: Story = {
  args: {
    children: "Button",
    variant: "ghost",
  },
  argTypes: {
    children: { table: { disable: true } },
    color: { table: { disable: true } },
    variant: { table: { disable: true } },
    size: { table: { disable: true } },
  },

  render: (args) => {
    return (
      <div className="flex flex-col gap-2">
        <div className="flex gap-2">
          <Button
            rounded={args.rounded}
            color="brand"
            size="md"
            variant={args.variant}
          >
            Brand
          </Button>
          <Button
            rounded={args.rounded}
            color="info"
            size="md"
            variant={args.variant}
          >
            Info
          </Button>
          <Button
            rounded={args.rounded}
            color="success"
            size="md"
            variant={args.variant}
          >
            Success
          </Button>
          <Button
            rounded={args.rounded}
            color="warning"
            size="md"
            variant={args.variant}
          >
            Warning
          </Button>
          <Button
            rounded={args.rounded}
            color="danger"
            size="md"
            variant={args.variant}
          >
            Danger
          </Button>
        </div>
      </div>
    );
  },
};
