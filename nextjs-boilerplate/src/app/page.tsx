'use client';

import Link from 'next/link';
import { Typography, Button, Card, Row, Col } from 'antd';
import {
  ShoppingOutlined,
  SafetyOutlined,
  TruckOutlined,
  CustomerServiceOutlined,
} from '@ant-design/icons';

const { Title, Paragraph } = Typography;

export default function Home() {
  const features = [
    {
      icon: <ShoppingOutlined style={{ fontSize: '32px', color: '#1890ff' }} />,
      title: 'Wide Selection',
      description: 'Browse our extensive collection of anime figures and collectibles',
    },
    {
      icon: <SafetyOutlined style={{ fontSize: '32px', color: '#52c41a' }} />,
      title: 'Secure Payment',
      description: 'Shop with confidence using our secure payment system',
    },
    {
      icon: <TruckOutlined style={{ fontSize: '32px', color: '#faad14' }} />,
      title: 'Fast Delivery',
      description: 'Get your items delivered quickly and safely',
    },
    {
      icon: <CustomerServiceOutlined style={{ fontSize: '32px', color: '#eb2f96' }} />,
      title: '24/7 Support',
      description: 'Our team is here to help you anytime',
    },
  ];

  return (
    <div style={{ textAlign: 'center', padding: '40px 0' }}>
      <Title level={1}>Welcome to Hobby</Title>
      <Paragraph style={{ fontSize: '18px', maxWidth: '600px', margin: '0 auto 40px' }}>
        Your one-stop shop for anime figures and collectibles. Discover amazing
        products from Pokemon, Digimon, Gundam, and more!
      </Paragraph>
      <Link href="/products">
        <Button type="primary" size="large">
          Browse Products
        </Button>
      </Link>

      <Row gutter={[24, 24]} style={{ marginTop: '60px' }}>
        {features.map((feature, index) => (
          <Col xs={24} sm={12} md={6} key={index}>
            <Card
              style={{ textAlign: 'center', height: '100%' }}
              hoverable
            >
              {feature.icon}
              <Title level={4} style={{ marginTop: '16px' }}>
                {feature.title}
              </Title>
              <Paragraph type="secondary">{feature.description}</Paragraph>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
}
