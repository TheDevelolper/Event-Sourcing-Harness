import React from "react";
import clsx from "clsx";

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "primary" | "secondary" | "danger";
  size?: "sm" | "md" | "lg";
}

export const Button: React.FC<ButtonProps> = ({
  children,
  variant = "primary",
  size = "md",
  className,
  ...props
}) => {
  const baseStyles =
    "rounded-md font-medium transition-colors outline-none cursor-pointer";

  const variantStyles = {
    primary:
      "bg-[var(--color-primary-bg)] text-[var(--color-primary-fg)] hover:bg-[var(--color-primary-hover-bg)] hover:text-[var(--color-primary-hover-fg)]",
    secondary:
      "bg-[var(--color-secondary-bg)] text-[var(--color-secondary-fg)] hover:bg-[var(--color-secondary-hover-bg)] hover:text-[var(--color-secondary-hover-fg)]",
    success:
      "bg-[var(--color-success-bg)] text-[var(--color-success-fg)] hover:bg-[var(--color-success-bg)] hover:text-[var(--color-success-fg)]",
    danger:
      "bg-[var(--color-danger-bg)] text-[var(--color-danger-fg)] hover:bg-[var(--color-danger-bg)] hover:text-[var(--color-danger-fg)]",
    warning:
      "bg-[var(--color-warning-bg)] text-[var(--color-warning-fg)] hover:bg-[var(--color-warning-bg)] hover:text-[var(--color-warning-fg)]",
    info: "bg-[var(--color-info-bg)] text-[var(--color-info-fg)] hover:bg-[var(--color-info-bg)] hover:text-[var(--color-info-fg)]",
  };

  const sizeStyles = {
    sm: "px-3 py-1 text-sm",
    md: "px-4 py-2 text-base",
    lg: "px-6 py-3 text-lg",
  };

  return (
    <button
      className={clsx(
        baseStyles,
        variantStyles[variant],
        sizeStyles[size],
        className
      )}
      {...props}
    >
      {children}
    </button>
  );
};
