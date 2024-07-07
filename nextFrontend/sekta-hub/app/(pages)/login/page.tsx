"use client";
import { Button } from "@/app/_lib";
import { Form, Input } from "antd";
import React from "react";

type FieldType = {
  username?: string;
  password?: string;
  remember?: string;
};
const Login = () => {
  const [form] = Form.useForm();
  const onFinish = (data: FieldType) => {
    console.log(data);
  };
  return (
    <div>
      <h1>Welcome to SektaGram</h1>
      <div>
        <Form form={form} layout="vertical" onFinish={onFinish}>
          <Form.Item label="Email" name="email">
            <Input name="email" placeholder="Email" />
          </Form.Item>
          <Form.Item label="Password" name="password">
            <Input name="password" placeholder="Password" />
          </Form.Item>
          <Button label="Login" htmlType="submit" />
        </Form>
      </div>
    </div>
  );
};

export default Login;
