import React from "react";
import clsx from "clsx";

export interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  variant?: "default" | "primary" | "accent";
  shadow?: boolean;
  padding?: "none" | "sm" | "md" | "lg";
  className?: string;
}

export const Card: React.FC<CardProps> = ({
  children,
  variant = "default",
  shadow = true,
  padding = "md",
  className,
  ...props
}) => {
  const baseStyles = "rounded-lg border transition-shadow duration-200";

  const variantStyles = {
    default: "bg-white border-gray-200",
    primary: "bg-primary-50 border-primary-200",
    accent: "bg-accent-50 border-accent-200",
  };

  const paddingStyles = {
    none: "p-0",
    sm: "p-2",
    md: "p-4",
    lg: "p-6",
  };

  const shadowStyle = shadow ? "shadow-md hover:shadow-lg" : "shadow-none";

  return (
    <div
      className={clsx(
        baseStyles,
        variantStyles[variant],
        paddingStyles[padding],
        shadowStyle,
        className
      )}
      {...props}
    >
      {children}
    </div>
  );
};
