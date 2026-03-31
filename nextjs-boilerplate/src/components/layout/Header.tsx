'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { Layout, Menu, Badge, Avatar, Dropdown, Space, Skeleton } from 'antd';
import {
  ShoppingCartOutlined,
  UserOutlined,
  LoginOutlined,
  LogoutOutlined,
  ShoppingOutlined,
  DashboardOutlined,
  HomeOutlined,
  AppstoreOutlined,
} from '@ant-design/icons';
import { useAuthStore } from '@/stores/authStore';
import { useCartStore } from '@/stores/cartStore';
import type { MenuProps } from 'antd';

const { Header: AntHeader } = Layout;

export default function Header() {
  const [mounted, setMounted] = useState(false);
  const { user, isAuthenticated, logout, hydrate } = useAuthStore();
  const { totalItems, fetchCart } = useCartStore();

  useEffect(() => {
    hydrate();
    setMounted(true);
  }, [hydrate]);

  useEffect(() => {
    if (mounted && isAuthenticated) {
      fetchCart();
    }
  }, [mounted, isAuthenticated, fetchCart]);

  const userMenuItems: MenuProps['items'] = [
    {
      key: 'orders',
      icon: <ShoppingOutlined />,
      label: <Link href="/orders">My Orders</Link>,
    },
    ...(user?.role === 'Admin'
      ? [
          {
            key: 'admin',
            icon: <DashboardOutlined />,
            label: <Link href="/admin">Admin Dashboard</Link>,
          },
        ]
      : []),
    { type: 'divider' as const },
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Logout',
      onClick: logout,
    },
  ];

  const navItems: MenuProps['items'] = [
    {
      key: 'home',
      icon: <HomeOutlined />,
      label: <Link href="/">Home</Link>,
    },
    {
      key: 'products',
      icon: <AppstoreOutlined />,
      label: <Link href="/products">Products</Link>,
    },
  ];

  return (
    <AntHeader
      style={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        padding: '0 50px',
        background: '#fff',
        borderBottom: '1px solid #f0f0f0',
      }}
    >
      <div style={{ display: 'flex', alignItems: 'center', gap: '20px' }}>
        <Link href="/" style={{ fontSize: '20px', fontWeight: 'bold', color: '#1890ff' }}>
          Hobby
        </Link>
        <Menu
          mode="horizontal"
          items={navItems}
          style={{ border: 'none', flex: 1, minWidth: 0 }}
        />
      </div>

      <Space size="large">
        <Link href="/cart">
          <Badge count={mounted ? totalItems : 0} size="small">
            <ShoppingCartOutlined style={{ fontSize: '20px' }} />
          </Badge>
        </Link>

        {!mounted ? (
          <Skeleton.Avatar size="small" active />
        ) : isAuthenticated ? (
          <Dropdown menu={{ items: userMenuItems }} placement="bottomRight">
            <Avatar
              style={{ backgroundColor: '#1890ff', cursor: 'pointer' }}
              icon={<UserOutlined />}
            >
              {user?.firstName?.[0]}
            </Avatar>
          </Dropdown>
        ) : (
          <Link href="/login">
            <Space>
              <LoginOutlined />
              Login
            </Space>
          </Link>
        )}
      </Space>
    </AntHeader>
  );
}
