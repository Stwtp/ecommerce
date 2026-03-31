'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { Typography, Row, Col, Image, Button, InputNumber, Spin, message, Descriptions, Tag } from 'antd';
import { ShoppingCartOutlined, ArrowLeftOutlined } from '@ant-design/icons';
import Link from 'next/link';
import api from '@/services/api';
import { API_ENDPOINTS } from '@/constants/api';
import { Product } from '@/types/product';
import { formatPrice } from '@/utils/format';
import { useCartStore } from '@/stores/cartStore';
import { useAuthStore } from '@/stores/authStore';

const { Title, Paragraph } = Typography;

export default function ProductDetailPage() {
  const params = useParams();
  const router = useRouter();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [quantity, setQuantity] = useState(1);

  const { addItem } = useCartStore();
  const { isAuthenticated } = useAuthStore();

  useEffect(() => {
    fetchProduct();
  }, [params.id]);

  const fetchProduct = async () => {
    setLoading(true);
    try {
      const response = await api.get<Product>(API_ENDPOINTS.PRODUCTS.DETAIL(Number(params.id)));
      setProduct(response.data);
    } catch (error) {
      console.error('Failed to fetch product:', error);
      message.error('Product not found');
      router.push('/products');
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = async () => {
    if (!isAuthenticated) {
      message.warning('Please login to add items to cart');
      router.push('/login');
      return;
    }
    if (!product) return;
    try {
      await addItem(product.id, quantity);
      message.success(`Added ${quantity} item(s) to cart`);
    } catch (error) {
      message.error('Failed to add to cart');
    }
  };

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: '50px' }}>
        <Spin size="large" />
      </div>
    );
  }

  if (!product) {
    return null;
  }

  return (
    <div>
      <Link href="/products">
        <Button icon={<ArrowLeftOutlined />} style={{ marginBottom: '16px' }}>
          Back to Products
        </Button>
      </Link>

      <Row gutter={[32, 32]}>
        <Col xs={24} md={12}>
          <div
            style={{
              height: 600,
              width: 600,
              background: '#f5f5f5',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              borderRadius: '8px',
            }}
          >
            {product.imageUrl ? (
              <Image
                src={product.imageUrl}
                alt={product.name}
                style={{ maxHeight: '100%', maxWidth: '100%', objectFit: 'contain', mixBlendMode: 'multiply' }}
              />
            ) : (
              <ShoppingCartOutlined style={{ fontSize: 64, color: '#d9d9d9' }} />
            )}
          </div>
        </Col>

        <Col xs={24} md={12}>
          <Title level={2}>{product.name}</Title>

          <Descriptions column={1} bordered style={{ marginBottom: '24px' }}>
            <Descriptions.Item label="Price">
              <span style={{ color: '#1890ff', fontWeight: 'bold', fontSize: '24px' }}>
                {formatPrice(product.price)}
              </span>
            </Descriptions.Item>
            <Descriptions.Item label="Category">
              <Tag color="blue">{product.categoryName}</Tag>
            </Descriptions.Item>
            <Descriptions.Item label="Stock">
              <Tag color={product.stock > 0 ? 'green' : 'red'}>
                {product.stock > 0 ? `${product.stock} available` : 'Out of stock'}
              </Tag>
            </Descriptions.Item>
          </Descriptions>

          {product.description && (
            <Paragraph style={{ marginBottom: '24px' }}>{product.description}</Paragraph>
          )}

          {product.stock > 0 && (
            <div style={{ display: 'flex', alignItems: 'center', gap: '16px', marginBottom: '24px' }}>
              <span>Quantity:</span>
              <InputNumber
                min={1}
                max={product.stock}
                value={quantity}
                onChange={(value) => setQuantity(value || 1)}
              />
            </div>
          )}

          <Button
            type="primary"
            size="large"
            icon={<ShoppingCartOutlined />}
            onClick={handleAddToCart}
            disabled={product.stock === 0}
          >
            Add to Cart
          </Button>
        </Col>
      </Row>
    </div>
  );
}
