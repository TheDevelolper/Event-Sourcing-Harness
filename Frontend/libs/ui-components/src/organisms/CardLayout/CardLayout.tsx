import React from "react";
import clsx from "clsx";

export interface CardLayoutProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
  gap?: "sm" | "md" | "lg";
  className?: string;
}

export const CardLayout: React.FC<CardLayoutProps> = ({
  children,
  gap = "lg",
  className,
  ...props
}) => {
  const gapClasses = { sm: "gap-4", md: "gap-6", lg: "gap-8" };

  return (
    <div
      className={clsx(
        "grid grid-cols-1 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3",
        gapClasses[gap],
        className
      )}
    >
      {children}
    </div>
  );
};
