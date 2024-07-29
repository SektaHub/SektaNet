"use client";
import { Button } from "@/app/_lib";
import { Form, Input } from "antd";
import axios from "axios";
import { useRouter } from "next/navigation";
import React from "react";

type FieldType = {
  email?: string;
  password?: string;
  remember?: string;
};
const Login = () => {
  const [form] = Form.useForm();
  const router = useRouter();
  const onFinish = (data: FieldType) => {
    axios
      .post("https://www.cicki.gratis/api/identity/login", {
        email: data.email,
        password: data.password,
      })
      .then((response) => {
        if (response.status !== 200) return;
        console.log(response);
        localStorage.setItem(
          "token",
          `${response.data.tokenType} ${response.data.accessToken}`
        );

        router.push("/dashboard", {});
        router.refresh();
      })
      .catch((error) => {
        if (error?.response?.status === 401) {
          alert("Incorrect email or password");
        }
      });
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
            <Input.Password name="password" placeholder="Password" />
          </Form.Item>
          <Button label="Login" htmlType="submit" />
        </Form>
      </div>
    </div>
  );
};

export default Login;
