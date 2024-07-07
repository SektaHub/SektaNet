import { ButtonProps } from "antd";
import React from "react";
import { Button as AntButton } from "antd";

interface CustomButtonProps extends ButtonProps {
  backgroundColor?: string;
  label: string;
}
export const Button = ({
  style,
  backgroundColor,
  children,
  label,
  disabled,
  ...props
}: CustomButtonProps) => {
  return (
    <AntButton
      style={{ ...style, backgroundColor }}
      {...props}
      disabled={disabled}
    >
      {label || children}
    </AntButton>
  );
};
