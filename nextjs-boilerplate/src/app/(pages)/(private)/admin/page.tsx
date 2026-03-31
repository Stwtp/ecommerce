'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import {
  Typography,
  Card,
  Row,
  Col,
  Statistic,
  Table,
  Tag,
  Spin,
  Tabs,
  Select,
  Button,
  message,
} from 'antd';
import {
  UserOutlined,
  ShoppingOutlined,
  DollarOutlined,
  ClockCircleOutlined,
  CheckCircleOutlined,
  TruckOutlined,
  CloseCircleOutlined,
} from '@ant-design/icons';
import api from '@/services/api';
import { API_ENDPOINTS } from '@/constants/api';
import { DashboardResponse } from '@/types/order';
import { Product } from '@/types/product';
import { formatPrice, formatDateTime } from '@/utils/format';
import { useAuthStore } from '@/stores/authStore';

const { Title } = Typography;

interface AdminOrder {
  id: number;
  userId: number;
  userEmail: string;
  status: string;
  totalPrice: number;
  shippingAddress: string;
  itemCount: number;
  createdAt: string;
  updatedAt: string;
}

interface AdminUser {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  isActive: boolean;
  createdAt: string;
}

const statusColors: Record<string, string> = {
  Pending: 'orange',
  Processing: 'blue',
  Shipped: 'cyan',
  Delivered: 'green',
  Cancelled: 'red',
};

export default function AdminDashboardPage() {
  const router = useRouter();
  const { user, isAuthenticated } = useAuthStore();
  const [loading, setLoading] = useState(true);
  const [dashboard, setDashboard] = useState<DashboardResponse | null>(null);
  const [orders, setOrders] = useState<AdminOrder[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [users, setUsers] = useState<AdminUser[]>([]);

  useEffect(() => {
    if (!isAuthenticated || user?.role !== 'Admin') {
      router.push('/');
      return;
    }
    fetchDashboardData();
  }, [isAuthenticated, user, router]);

  const fetchDashboardData = async () => {
    setLoading(true);
    try {
      const [dashboardRes, ordersRes, productsRes, usersRes] = await Promise.all([
        api.get<DashboardResponse>(API_ENDPOINTS.ADMIN.DASHBOARD),
        api.get(API_ENDPOINTS.ADMIN.ORDERS),
        api.get(API_ENDPOINTS.ADMIN.PRODUCTS),
        api.get(API_ENDPOINTS.ADMIN.USERS),
      ]);
      setDashboard(dashboardRes.data);
      setOrders(ordersRes.data.items);
      setProducts(productsRes.data.items);
      setUsers(usersRes.data.items);
    } catch (error) {
      console.error('Failed to fetch dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleStatusUpdate = async (orderId: number, status: string) => {
    try {
      await api.put(API_ENDPOINTS.ADMIN.UPDATE_ORDER_STATUS(orderId), { status });
      message.success('Order status updated');
      fetchDashboardData();
    } catch (error) {
      message.error('Failed to update status');
    }
  };

  if (!isAuthenticated || user?.role !== 'Admin') {
    return null;
  }

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: '50px' }}>
        <Spin size="large" />
      </div>
    );
  }

  const orderColumns = [
    { title: 'Order #', dataIndex: 'id', key: 'id', render: (id: number) => `#${id}` },
    { title: 'Customer', dataIndex: 'userEmail', key: 'userEmail' },
    {
      title: 'Date',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (date: string) => formatDateTime(date),
    },
    { title: 'Items', dataIndex: 'itemCount', key: 'itemCount' },
    {
      title: 'Total',
      dataIndex: 'totalPrice',
      key: 'totalPrice',
      render: (price: number) => formatPrice(price),
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (status: string, record: AdminOrder) => (
        <Select
          value={status}
          style={{ width: 120 }}
          onChange={(value) => handleStatusUpdate(record.id, value)}
          options={[
            { value: 'Pending', label: 'Pending' },
            { value: 'Processing', label: 'Processing' },
            { value: 'Shipped', label: 'Shipped' },
            { value: 'Delivered', label: 'Delivered' },
            { value: 'Cancelled', label: 'Cancelled' },
          ]}
        />
      ),
    },
  ];

  const productColumns = [
    { title: 'ID', dataIndex: 'id', key: 'id' },
    { title: 'Name', dataIndex: 'name', key: 'name' },
    {
      title: 'Price',
      dataIndex: 'price',
      key: 'price',
      render: (price: number) => formatPrice(price),
    },
    { title: 'Stock', dataIndex: 'stock', key: 'stock' },
    {
      title: 'Status',
      dataIndex: 'isActive',
      key: 'isActive',
      render: (isActive: boolean) => (
        <Tag color={isActive ? 'green' : 'red'}>
          {isActive ? 'Active' : 'Inactive'}
        </Tag>
      ),
    },
  ];

  const userColumns = [
    { title: 'ID', dataIndex: 'id', key: 'id' },
    { title: 'Email', dataIndex: 'email', key: 'email' },
    {
      title: 'Name',
      key: 'name',
      render: (_: unknown, record: AdminUser) => `${record.firstName} ${record.lastName}`,
    },
    {
      title: 'Role',
      dataIndex: 'role',
      key: 'role',
      render: (role: string) => (
        <Tag color={role === 'Admin' ? 'purple' : 'blue'}>{role}</Tag>
      ),
    },
    {
      title: 'Status',
      dataIndex: 'isActive',
      key: 'isActive',
      render: (isActive: boolean) => (
        <Tag color={isActive ? 'green' : 'red'}>
          {isActive ? 'Active' : 'Inactive'}
        </Tag>
      ),
    },
    {
      title: 'Joined',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (date: string) => formatDateTime(date),
    },
  ];

  const tabItems = [
    {
      key: 'orders',
      label: 'Orders',
      children: <Table dataSource={orders} columns={orderColumns} rowKey="id" />,
    },
    {
      key: 'products',
      label: 'Products',
      children: <Table dataSource={products} columns={productColumns} rowKey="id" />,
    },
    {
      key: 'users',
      label: 'Users',
      children: <Table dataSource={users} columns={userColumns} rowKey="id" />,
    },
  ];

  return (
    <div>
      <Title level={2}>Admin Dashboard</Title>

      <Row gutter={[16, 16]} style={{ marginBottom: '24px' }}>
        <Col xs={12} sm={6}>
          <Card>
            <Statistic
              title="Total Users"
              value={dashboard?.totalUsers || 0}
              prefix={<UserOutlined />}
            />
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card>
            <Statistic
              title="Total Products"
              value={dashboard?.totalProducts || 0}
              prefix={<ShoppingOutlined />}
            />
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card>
            <Statistic
              title="Total Orders"
              value={dashboard?.totalOrders || 0}
              prefix={<ShoppingOutlined />}
            />
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card>
            <Statistic
              title="Total Revenue"
              value={dashboard?.totalRevenue || 0}
              prefix={<DollarOutlined />}
              formatter={(value) => formatPrice(Number(value))}
            />
          </Card>
        </Col>
      </Row>

      <Row gutter={[16, 16]} style={{ marginBottom: '24px' }}>
        <Col xs={8} sm={4}>
          <Card size="small">
            <Statistic
              title="Pending"
              value={dashboard?.pendingOrders || 0}
              styles={{ content: { color: '#fa8c16' } }}
              prefix={<ClockCircleOutlined />}
            />
          </Card>
        </Col>
        <Col xs={8} sm={4}>
          <Card size="small">
            <Statistic
              title="Processing"
              value={dashboard?.processingOrders || 0}
              styles={{ content: { color: '#1890ff' } }}
              prefix={<ClockCircleOutlined />}
            />
          </Card>
        </Col>
        <Col xs={8} sm={4}>
          <Card size="small">
            <Statistic
              title="Shipped"
              value={dashboard?.shippedOrders || 0}
              styles={{ content: { color: '#13c2c2' } }}
              prefix={<TruckOutlined />}
            />
          </Card>
        </Col>
        <Col xs={8} sm={4}>
          <Card size="small">
            <Statistic
              title="Delivered"
              value={dashboard?.deliveredOrders || 0}
              styles={{ content: { color: '#52c41a' } }}
              prefix={<CheckCircleOutlined />}
            />
          </Card>
        </Col>
        <Col xs={8} sm={4}>
          <Card size="small">
            <Statistic
              title="Cancelled"
              value={dashboard?.cancelledOrders || 0}
              styles={{ content: { color: '#ff4d4f' } }}
              prefix={<CloseCircleOutlined />}
            />
          </Card>
        </Col>
      </Row>

      <Card>
        <Tabs items={tabItems} />
      </Card>
    </div>
  );
}
