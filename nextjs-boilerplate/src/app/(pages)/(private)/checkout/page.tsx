'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import {
  Typography,
  Form,
  Input,
  Button,
  Card,
  Row,
  Col,
  Table,
  message,
  Spin,
  Steps,
} from 'antd';
import { ShoppingOutlined, CreditCardOutlined, CheckCircleOutlined } from '@ant-design/icons';
import Link from 'next/link';
import api from '@/services/api';
import { API_ENDPOINTS } from '@/constants/api';
import { useCartStore } from '@/stores/cartStore';
import { useAuthStore } from '@/stores/authStore';
import { formatPrice } from '@/utils/format';
import { PaymentResponse } from '@/types/payment';

const { Title, Paragraph } = Typography;
const { TextArea } = Input;

export default function CheckoutPage() {
  const router = useRouter();
  const { items, totalItems, totalPrice, isLoading: cartLoading, fetchCart, clearCart } = useCartStore();
  const { isAuthenticated } = useAuthStore();
  const [currentStep, setCurrentStep] = useState(0);
  const [shippingAddress, setShippingAddress] = useState('');
  const [orderId, setOrderId] = useState<number | null>(null);
  const [paymentIntentId, setPaymentIntentId] = useState<string | null>(null);
  const [paymentLoading, setPaymentLoading] = useState(false);
  const [form] = Form.useForm();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
      return;
    }
    fetchCart();
  }, [isAuthenticated, router, fetchCart]);

  const handleShippingSubmit = async () => {
    try {
      const values = await form.validateFields();
      setShippingAddress(values.shippingAddress);
      setCurrentStep(1);
    } catch (error) {
      // Form validation failed
    }
  };

  const handleCreateOrder = async () => {
    setPaymentLoading(true);
    try {
      const response = await api.post(API_ENDPOINTS.ORDERS.CHECKOUT, {
        shippingAddress,
      });
      setOrderId(response.data.id);
      setCurrentStep(2);
      message.success('Order created successfully!');
    } catch (error) {
      message.error('Failed to create order');
    } finally {
      setPaymentLoading(false);
    }
  };

  const handlePayment = async () => {
    if (!orderId) return;
    setPaymentLoading(true);
    try {
      // Step 1: Create payment intent
      const intentResponse = await api.post<PaymentResponse>(API_ENDPOINTS.PAYMENTS.CREATE_INTENT, {
        orderId,
      });
      const paymentId = intentResponse.data.paymentIntentId;
      setPaymentIntentId(paymentId);

      // Step 2: Confirm payment (mock)
      await api.post(API_ENDPOINTS.PAYMENTS.CONFIRM, {
        paymentIntentId: paymentId,
      });

      // Step 3: Clear cart
      await clearCart();

      message.success('Payment successful!');
      setCurrentStep(3);
    } catch (error) {
      message.error('Failed to process payment');
    } finally {
      setPaymentLoading(false);
    }
  };

  if (!isAuthenticated) {
    return null;
  }

  if (cartLoading) {
    return (
      <div style={{ textAlign: 'center', padding: '50px' }}>
        <Spin size="large" />
      </div>
    );
  }

  if (items.length === 0 && currentStep === 0) {
    return (
      <div style={{ textAlign: 'center', padding: '50px' }}>
        <Title level={3}>Your cart is empty</Title>
        <Link href="/products">
          <Button type="primary" icon={<ShoppingOutlined />}>
            Browse Products
          </Button>
        </Link>
      </div>
    );
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
      title: 'Qty',
      dataIndex: 'quantity',
      key: 'quantity',
    },
    {
      title: 'Subtotal',
      dataIndex: 'subtotal',
      key: 'subtotal',
      render: (subtotal: number) => formatPrice(subtotal),
    },
  ];

  const steps = [
    {
      title: 'Shipping',
      icon: <ShoppingOutlined />,
    },
    {
      title: 'Review',
      icon: <ShoppingOutlined />,
    },
    {
      title: 'Payment',
      icon: <CreditCardOutlined />,
    },
    {
      title: 'Complete',
      icon: <CheckCircleOutlined />,
    },
  ];

  return (
    <div>
      <Title level={2}>Checkout</Title>

      <Steps current={currentStep} items={steps} style={{ marginBottom: '32px' }} />

      {currentStep === 0 && (
        <Row gutter={[24, 24]}>
          <Col xs={24} lg={14}>
            <Card title="Shipping Information">
              <Form form={form} layout="vertical">
                <Form.Item
                  name="shippingAddress"
                  label="Shipping Address"
                  rules={[{ required: true, message: 'Please enter your shipping address' }]}
                >
                  <TextArea rows={4} placeholder="Enter your full shipping address" />
                </Form.Item>
                <Button type="primary" onClick={handleShippingSubmit}>
                  Continue to Review
                </Button>
              </Form>
            </Card>
          </Col>
          <Col xs={24} lg={10}>
            <Card title="Order Summary">
              <Table
                dataSource={items}
                columns={columns}
                rowKey="id"
                pagination={false}
                size="small"
              />
              <div style={{ marginTop: '16px', textAlign: 'right' }}>
                <Paragraph strong>Total: {formatPrice(totalPrice)}</Paragraph>
              </div>
            </Card>
          </Col>
        </Row>
      )}

      {currentStep === 1 && (
        <Row gutter={[24, 24]}>
          <Col xs={24} lg={14}>
            <Card title="Review Order">
              <Title level={5}>Shipping Address</Title>
              <Paragraph>{shippingAddress}</Paragraph>
              <Title level={5}>Items ({totalItems})</Title>
              <Table
                dataSource={items}
                columns={columns}
                rowKey="id"
                pagination={false}
                size="small"
              />
              <div style={{ marginTop: '24px' }}>
                <Button onClick={() => setCurrentStep(0)} style={{ marginRight: '8px' }}>
                  Back
                </Button>
                <Button
                  type="primary"
                  onClick={handleCreateOrder}
                  loading={paymentLoading}
                >
                  Create Order
                </Button>
              </div>
            </Card>
          </Col>
          <Col xs={24} lg={10}>
            <Card title="Order Total">
              <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '18px', fontWeight: 'bold' }}>
                <span>Total:</span>
                <span>{formatPrice(totalPrice)}</span>
              </div>
            </Card>
          </Col>
        </Row>
      )}

      {currentStep === 2 && (
        <Row gutter={[24, 24]}>
          <Col xs={24} lg={14}>
            <Card title="Payment">
              <Paragraph>
                Your order has been created. Click the button below to complete the payment.
              </Paragraph>
              <div style={{ background: '#f5f5f5', padding: '24px', borderRadius: '8px', marginTop: '16px' }}>
                <Title level={5}>Order #{orderId}</Title>
                <Paragraph>Total: {formatPrice(totalPrice)}</Paragraph>
                <Paragraph type="secondary">
                  This is a mock payment for testing purposes.
                </Paragraph>
              </div>
              <div style={{ marginTop: '24px' }}>
                <Button onClick={() => setCurrentStep(1)} style={{ marginRight: '8px' }}>
                  Back
                </Button>
                <Button
                  type="primary"
                  onClick={handlePayment}
                  loading={paymentLoading}
                  icon={<CreditCardOutlined />}
                >
                  Pay Now
                </Button>
              </div>
            </Card>
          </Col>
        </Row>
      )}

      {currentStep === 3 && (
        <Card style={{ textAlign: 'center', maxWidth: 500, margin: '0 auto' }}>
          <CheckCircleOutlined style={{ fontSize: 64, color: '#52c41a' }} />
          <Title level={3}>Order Complete!</Title>
          <Paragraph>
            Thank you for your order. Your order #{orderId} has been placed successfully.
          </Paragraph>
          <div style={{ marginTop: '24px' }}>
            <Link href={`/orders`}>
              <Button type="primary" style={{ marginRight: '8px' }}>
                View Orders
              </Button>
            </Link>
            <Link href="/products">
              <Button>Continue Shopping</Button>
            </Link>
          </div>
        </Card>
      )}
    </div>
  );
}
