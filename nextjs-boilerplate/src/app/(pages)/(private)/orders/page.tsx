'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { Typography, Table, Tag, Card, Spin, Empty, Button, message } from 'antd';
import { ShoppingOutlined, CreditCardOutlined } from '@ant-design/icons';
import Link from 'next/link';
import api from '@/services/api';
import { API_ENDPOINTS } from '@/constants/api';
import { Order } from '@/types/order';
import { formatPrice, formatDateTime } from '@/utils/format';
import { useAuthStore } from '@/stores/authStore';

const { Title } = Typography;

const statusColors: Record<string, string> = {
  Pending: 'orange',
  Processing: 'blue',
  Shipped: 'cyan',
  Delivered: 'green',
  Cancelled: 'red',
};

export default function OrdersPage() {
  const router = useRouter();
  const { isAuthenticated } = useAuthStore();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [payingId, setPayingId] = useState<number | null>(null);

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
      return;
    }
    fetchOrders();
  }, [isAuthenticated, router]);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      const response = await api.get<Order[]>(API_ENDPOINTS.ORDERS.LIST);
      setOrders(response.data);
    } catch (error) {
      console.error('Failed to fetch orders:', error);
    } finally {
      setLoading(false);
    }
  };

  const handlePayNow = async (orderId: number) => {
    setPayingId(orderId);
    try {
      // Step 1: Create payment intent
      const intentResponse = await api.post(API_ENDPOINTS.PAYMENTS.CREATE_INTENT, {
        orderId,
      });
      const paymentIntentId = intentResponse.data.paymentIntentId;

      // Step 2: Confirm payment (mock)
      await api.post(API_ENDPOINTS.PAYMENTS.CONFIRM, {
        paymentIntentId,
      });

      message.success('Payment successful!');
      fetchOrders();
    } catch (error) {
      message.error('Failed to process payment');
    } finally {
      setPayingId(null);
    }
  };

  if (!isAuthenticated) {
    return null;
  }

  const columns = [
    {
      title: 'Order #',
      dataIndex: 'id',
      key: 'id',
      render: (id: number) => `#${id}`,
    },
    {
      title: 'Date',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (date: string) => formatDateTime(date),
    },
    {
      title: 'Items',
      dataIndex: 'items',
      key: 'items',
      render: (items: unknown[]) => items.length,
    },
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
      render: (status: string) => (
        <Tag color={statusColors[status] || 'default'}>{status}</Tag>
      ),
    },
    {
      title: 'Action',
      key: 'action',
      render: (_: unknown, record: Order) => (
        record.status === 'Pending' ? (
          <Button
            type="primary"
            size="small"
            icon={<CreditCardOutlined />}
            loading={payingId === record.id}
            onClick={() => handlePayNow(record.id)}
          >
            Pay Now
          </Button>
        ) : record.status === 'Processing' ? (
          <Tag color="blue">Paid</Tag>
        ) : null
      ),
    },
  ];

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: '50px' }}>
        <Spin size="large" />
      </div>
    );
  }

  return (
    <div>
      <Title level={2}>My Orders</Title>

      {orders.length === 0 ? (
        <Empty
          description="No orders yet"
          image={Empty.PRESENTED_IMAGE_SIMPLE}
        >
          <Link href="/products">
            <Button type="primary" icon={<ShoppingOutlined />}>
              Start Shopping
            </Button>
          </Link>
        </Empty>
      ) : (
        <Table
          dataSource={orders}
          columns={columns}
          rowKey="id"
          expandable={{
            expandedRowRender: (record) => (
              <Card size="small" title="Order Items">
                <Table
                  dataSource={record.items}
                  rowKey="id"
                  pagination={false}
                  size="small"
                  columns={[
                    { title: 'Product', dataIndex: 'productName', key: 'productName' },
                    { title: 'Price', dataIndex: 'price', key: 'price', render: (p: number) => formatPrice(p) },
                    { title: 'Qty', dataIndex: 'quantity', key: 'quantity' },
                    { title: 'Subtotal', dataIndex: 'subtotal', key: 'subtotal', render: (s: number) => formatPrice(s) },
                  ]}
                />
              </Card>
            ),
          }}
        />
      )}
    </div>
  );
}
