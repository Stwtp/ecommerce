'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import {
  Typography,
  Table,
  Button,
  InputNumber,
  Space,
  Empty,
  Card,
  Row,
  Col,
  Popconfirm,
  message,
  Spin,
} from 'antd';
import { DeleteOutlined, ShoppingOutlined } from '@ant-design/icons';
import Link from 'next/link';
import { useCartStore } from '@/stores/cartStore';
import { useAuthStore } from '@/stores/authStore';
import { formatPrice } from '@/utils/format';

const { Title } = Typography;

export default function CartPage() {
  const router = useRouter();
  const { items, totalItems, totalPrice, isLoading, fetchCart, updateItem, removeItem, clearCart } = useCartStore();
  const { isAuthenticated } = useAuthStore();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
      return;
    }
    fetchCart();
  }, [isAuthenticated, router, fetchCart]);

  const handleQuantityChange = async (id: number, quantity: number) => {
    try {
      await updateItem(id, quantity);
    } catch (error) {
      message.error('Failed to update quantity');
    }
  };

  const handleRemove = async (id: number) => {
    try {
      await removeItem(id);
      message.success('Item removed');
    } catch (error) {
      message.error('Failed to remove item');
    }
  };

  const handleClearCart = async () => {
    try {
      await clearCart();
      message.success('Cart cleared');
    } catch (error) {
      message.error('Failed to clear cart');
    }
  };

  if (!isAuthenticated) {
    return null;
  }

  const columns = [
    {
      title: 'Product',
      dataIndex: 'productName',
      key: 'productName',
    },
    {
      title: 'Price',
      dataIndex: 'price',
      key: 'price',
      render: (price: number) => formatPrice(price),
    },
    {
      title: 'Quantity',
      dataIndex: 'quantity',
      key: 'quantity',
      render: (quantity: number, record: { id: number }) => (
        <InputNumber
          min={1}
          value={quantity}
          onChange={(value) => handleQuantityChange(record.id, value || 1)}
        />
      ),
    },
    {
      title: 'Subtotal',
      dataIndex: 'subtotal',
      key: 'subtotal',
      render: (subtotal: number) => formatPrice(subtotal),
    },
    {
      title: 'Action',
      key: 'action',
      render: (_: unknown, record: { id: number }) => (
        <Popconfirm
          title="Remove item?"
          onConfirm={() => handleRemove(record.id)}
          okText="Yes"
          cancelText="No"
        >
          <Button type="link" danger icon={<DeleteOutlined />}>
            Remove
          </Button>
        </Popconfirm>
      ),
    },
  ];

  if (isLoading) {
    return (
      <div style={{ textAlign: 'center', padding: '50px' }}>
        <Spin size="large" />
      </div>
    );
  }

  return (
    <div>
      <Title level={2}>Shopping Cart</Title>

      {items.length === 0 ? (
        <Empty
          description="Your cart is empty"
          image={Empty.PRESENTED_IMAGE_SIMPLE}
        >
          <Link href="/products">
            <Button type="primary" icon={<ShoppingOutlined />}>
              Browse Products
            </Button>
          </Link>
        </Empty>
      ) : (
        <Row gutter={[24, 24]}>
          <Col xs={24} lg={16}>
            <Table
              dataSource={items}
              columns={columns}
              rowKey="id"
              pagination={false}
            />
            <div style={{ marginTop: '16px' }}>
              <Popconfirm
                title="Clear entire cart?"
                onConfirm={handleClearCart}
                okText="Yes"
                cancelText="No"
              >
                <Button danger>Clear Cart</Button>
              </Popconfirm>
            </div>
          </Col>

          <Col xs={24} lg={8}>
            <Card title="Order Summary">
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '16px' }}>
                <span>Items ({totalItems}):</span>
                <span>{formatPrice(totalPrice)}</span>
              </div>
              <div
                style={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  fontWeight: 'bold',
                  fontSize: '18px',
                  borderTop: '1px solid #f0f0f0',
                  paddingTop: '16px',
                }}
              >
                <span>Total:</span>
                <span>{formatPrice(totalPrice)}</span>
              </div>
              <Button
                type="primary"
                size="large"
                block
                style={{ marginTop: '16px' }}
                onClick={() => router.push('/checkout')}
              >
                Proceed to Checkout
              </Button>
            </Card>
          </Col>
        </Row>
      )}
    </div>
  );
}
