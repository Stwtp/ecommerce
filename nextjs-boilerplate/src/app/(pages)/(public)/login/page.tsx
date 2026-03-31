'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { Typography, Form, Input, Button, Card, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import Link from 'next/link';
import { useAuthStore } from '@/stores/authStore';

const { Title } = Typography;

export default function LoginPage() {
  const router = useRouter();
  const { login, isLoading, error, clearError } = useAuthStore();
  const [form] = Form.useForm();

  const onFinish = async (values: { email: string; password: string }) => {
    try {
      await login(values);
      message.success('Login successful');
      router.push('/');
    } catch (error) {
      // Error is handled by store
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: '0 auto', paddingTop: '40px' }}>
      <Card>
        <Title level={3} style={{ textAlign: 'center' }}>
          Login
        </Title>

        {error && (
          <div style={{ color: 'red', marginBottom: '16px', textAlign: 'center' }}>
            {error}
          </div>
        )}

        <Form
          form={form}
          name="login"
          onFinish={onFinish}
          layout="vertical"
          onFocus={clearError}
        >
          <Form.Item
            name="email"
            rules={[
              { required: true, message: 'Please input your email!' },
              { type: 'email', message: 'Please enter a valid email!' },
            ]}
          >
            <Input
              prefix={<UserOutlined />}
              placeholder="Email"
              size="large"
            />
          </Form.Item>

          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Please input your password!' }]}
          >
            <Input.Password
              prefix={<LockOutlined />}
              placeholder="Password"
              size="large"
            />
          </Form.Item>

          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              loading={isLoading}
              block
              size="large"
            >
              Login
            </Button>
          </Form.Item>

          <div style={{ textAlign: 'center' }}>
            Don&apos;t have an account?{' '}
            <Link href="/register">Register</Link>
          </div>
        </Form>
      </Card>
    </div>
  );
}
