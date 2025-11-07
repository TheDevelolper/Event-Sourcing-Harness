import React from "react";
import clsx from "clsx";

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  color?: "brand" | "success" | "warning" | "danger" | "info";
  size?: "sm" | "md" | "lg";
  rounded: "none" | "sm" | "md" | "lg";
  variant?: "solid" | "outline" | "ghost" | "semiOpaque";
}

export const Button: React.FC<ButtonProps> = ({
  children,
  color = "brand",
  size = "md",
  variant = "solid",
  rounded = "none",
  className,
  ...props
}) => {
  const baseStyles =
    "font-medium transition-colors outline-none cursor-pointer user-select-none";

  const bgStyles: Record<string, string> = {
    brand:
      "bg-[var(--color-brand-bg)] hover:bg-[var(--color-brand-hover-bg)] border-[var(--color-brand-bg)]",
    success:
      "bg-[var(--color-success-bg)] hover:bg-[var(--color-success-hover-bg)] border-[var(--color-success-bg)]",
    info: "bg-[var(--color-info-bg)] hover:bg-[var(--color-info-hover-bg)] border-[var(--color-info-bg)]",
    warning:
      "bg-[var(--color-warning-bg)] hover:bg-[var(--color-warning-hover-bg)] border-[var(--color-warning-bg)]",
    danger:
      "bg-[var(--color-danger-bg)] hover:bg-[var(--color-danger-hover-bg)] border-[var(--color-danger-bg)]",
  };

  const fgStyles: Record<string, string> = {
    brand:
      "text-[var(--color-brand-fg)] hover:text-[var(--color-brand-hover-fg)]",
    success:
      "text-[var(--color-success-fg)] hover:text-[var(--color-success-hover-fg)]",
    info: "text-[var(--color-info-fg)] hover:text-[var(--color-info-hover-fg)]",
    warning:
      "text-[var(--color-warning-fg)] hover:text-[var(--color-warning-hover-fg)]",
    danger:
      "text-[var(--color-danger-fg)] hover:text-[var(--color-danger-hover-fg)]",
  };

  const semiOpaqueStyles: Record<string, string> = {
    brand:
      "bg-[var(--color-brand-bg)]/80 hover:bg-[var(--color-brand-hover-bg)] border-[var(--color-brand-bg)] text-[var(--color-brand-fg)] dark:text-[var(--color-brand-hover-fg)] hover:text-[var(--color-brand-hover-fg)]",
    success:
      "bg-[var(--color-success-bg)]/40 dark:bg-[var(--color-success-bg)]/30 hover:bg-[var(--color-success-hover-bg)] border-[var(--color-success-bg) text-[var(--color-success-fg)] dark:text-[var(--color-success-bg)] hover:text-[var(--color-success-hover-fg)]",
    info: "bg-[var(--color-info-bg)]/40 dark:bg-[var(--color-info-bg)]/30  hover:bg-[var(--color-info-hover-bg)] border-[var(--color-info-bg)] text-[var(--color-info-fg)] dark:text-[var(--color-info-bg)] hover:text-[var(--color-info-hover-fg)]",
    warning:
      "bg-[var(--color-warning-bg)]/40 dark:bg-[var(--color-warning-bg)]/30 hover:bg-[var(--color-warning-hover-bg)] border-[var(--color-warning-bg)] text-[var(--color-warning-fg)] dark:text-[var(--color-warning-bg)] hover:text-[var(--color-warning-hover-fg)]",
    danger:
      "bg-[var(--color-danger-bg)]/40 dark:bg-[var(--color-danger-bg)]/30 hover:bg-[var(--color-danger-hover-bg)] border-[var(--color-danger-bg)] text-[var(--color-danger-fg)] dark:text-[var(--color-danger-bg)] hover:text-[var(--color-danger-hover-fg)]",
  };

  const outlineStyles: Record<string, string> = {
    brand:
      "border-[0.1rem] bg-[var(--color-brand-bg)]/20 border-[var(--color-brand-bg)] text-[var(--color-brand-fg)] dark:text-[var(--color-brand-fg)] hover:bg-[var(--color-brand-bg)] hover:text-[var(--color-brand-fg)]",
    success:
      "border-[0.1rem] border-[var(--color-success-bg)] text-[var(--color-success-fg)] dark:text-[var(--color-success-bg)] hover:bg-[var(--color-success-bg)] hover:text-[var(--color-success-fg)]",
    info: "border-[0.1rem] border-[var(--color-info-bg)] text-[var(--color-info-fg)] dark:text-[var(--color-info-bg)] hover:bg-[var(--color-info-bg)] hover:text-[var(--color-info-fg)]",
    warning:
      "border-[0.1rem] border-[var(--color-warning-bg)] text-[var(--color-warning-fg)] dark:text-[var(--color-warning-bg)] hover:bg-[var(--color-warning-bg)] hover:text-[var(--color-warning-fg)]",
    danger:
      "border-[0.1rem] border-[var(--color-danger-bg)] text-[var(--color-danger-fg)] dark:text-[var(--color-danger-bg)] hover:bg-[var(--color-danger-bg)] hover:text-[var(--color-danger-fg)]",
  };

  const ghostStyles: Record<string, string> = {
    brand:
      "text-[var(--color-brand-fg)] hover:bg-[var(--color-brand-bg)] hover:text-[var(--color-brand-hover-fg)]",
    success:
      "text-gray-700 dark:text-gray-300 hover:bg-[var(--color-success-hover-bg)] hover:text-[var(--color-success-hover-fg)]",
    info: "text-gray-700 dark:text-gray-300 hover:bg-[var(--color-info-hover-bg)] hover:text-[var(--color-info-hover-fg)]",
    warning:
      "text-gray-700 dark:text-gray-300 hover:bg-[var(--color-warning-hover-bg)] hover:text-[var(--color-warning-hover-fg)]",
    danger:
      "text-gray-700 dark:text-gray-300 hover:bg-[var(--color-danger-hover-bg)] hover:text-[var(--color-danger-hover-fg)]",
  };

  const borderStyles: Record<string, string> = {
    brand:
      "border-[0.05rem] border-white text-[var(--color-brand-bg)] hover:bg-[var(--color-brand-bg)/10]",
    success:
      "border-[0.05rem] border-white text-[var(--color-success-bg)] hover:bg-[var(--color-success-bg)/10]",
    info: "border-[0.05rem] border-white text-[var(--color-info-bg)] hover:bg-[var(--color-info-bg)/10]",
    warning:
      "border-[0.05rem] border-white text-[var(--color-warning-bg)] hover:bg-[var(--color-warning-bg)/10]",
    danger:
      "border-[0.05rem] border-white text-[var(--color-danger-bg)] hover:bg-[var(--color-danger-bg)/10]",
  };

  const sizeStyles: Record<string, string> = {
    sm: "px-3 py-1 text-sm",
    md: "px-4 py-2 text-base",
    lg: "px-6 py-3 text-lg",
  };

  const roundedStyles: Record<string, string> = {
    none: "",
    sm: "rounded-xs",
    md: "rounded-md",
    lg: "rounded-xl",
  };

  const variantStyles: Record<string, string> = {
    solid: `${fgStyles[color]} ${bgStyles[color]}`, // use foreground
    outline: outlineStyles[color], // text uses bg color
    ghost: ghostStyles[color], // same as outline
    semiOpaque: semiOpaqueStyles[color], // same as outline
  };

  // Only apply background from colorStyles for solid; outline/ghost handle themselves

  return (
    <>
      {color === "brand" ? (
        <></>
      ) : (
        <button
          className={clsx(
            baseStyles,
            variantStyles[variant],
            borderStyles[variant],
            roundedStyles[rounded],
            sizeStyles[size],
            className
          )}
          {...props}
        >
          {children}
        </button>
      )}
    </>
  );
};
