'use client';

import { useEffect, useState } from 'react';
import { Typography, Row, Col, Card, Input, Select, Pagination, Spin, Empty, Button, message } from 'antd';
import { ShoppingCartOutlined } from '@ant-design/icons';
import Link from 'next/link';
import api from '@/services/api';
import { API_ENDPOINTS } from '@/constants/api';
import { Product, Category, PaginatedResponse } from '@/types/product';
import { formatPrice } from '@/utils/format';
import { useCartStore } from '@/stores/cartStore';
import { useAuthStore } from '@/stores/authStore';
import { useRouter } from 'next/navigation';

const { Title } = Typography;
const { Search } = Input;
const { Meta } = Card;

export default function ProductsPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [pageSize] = useState(12);
  const [search, setSearch] = useState('');
  const [categoryId, setCategoryId] = useState<number | undefined>();
  const [sortBy, setSortBy] = useState('createdAt');
  const [sortOrder, setSortOrder] = useState('desc');

  const { addItem } = useCartStore();
  const { isAuthenticated } = useAuthStore();
  const router = useRouter();

  useEffect(() => {
    fetchCategories();
  }, []);

  useEffect(() => {
    fetchProducts();
  }, [page, search, categoryId, sortBy, sortOrder]);

  const fetchCategories = async () => {
    try {
      const response = await api.get<Category[]>(API_ENDPOINTS.CATEGORIES.LIST);
      setCategories(response.data);
    } catch (error) {
      console.error('Failed to fetch categories:', error);
    }
  };

  const fetchProducts = async () => {
    setLoading(true);
    try {
      const params = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString(),
        ...(search && { search }),
        ...(categoryId && { categoryId: categoryId.toString() }),
        sortBy,
        sortOrder,
      });

      const response = await api.get<PaginatedResponse<Product>>(
        `${API_ENDPOINTS.PRODUCTS.LIST}?${params}`
      );
      setProducts(response.data.items);
      setTotal(response.data.totalCount);
    } catch (error) {
      console.error('Failed to fetch products:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = async (productId: number) => {
    if (!isAuthenticated) {
      message.warning('Please login to add items to cart');
      router.push('/login');
      return;
    }
    try {
      await addItem(productId, 1);
      message.success('Added to cart');
    } catch (error) {
      message.error('Failed to add to cart');
    }
  };

  return (
    <div>
      <Title level={2}>Products</Title>

      <Row gutter={[16, 16]} style={{ marginBottom: '24px' }}>
        <Col xs={24} sm={12} md={8}>
          <Search
            placeholder="Search products..."
            allowClear
            onSearch={(value) => {
              setSearch(value);
              setPage(1);
            }}
          />
        </Col>
        <Col xs={12} sm={6} md={4}>
          <Select
            placeholder="Category"
            allowClear
            style={{ width: '100%' }}
            onChange={(value) => {
              setCategoryId(value);
              setPage(1);
            }}
            options={[
              { value: undefined, label: 'All Categories' },
              ...categories.map((cat) => ({ value: cat.id, label: cat.name })),
            ]}
          />
        </Col>
        <Col xs={12} sm={6} md={4}>
          <Select
            placeholder="Sort by"
            style={{ width: '100%' }}
            value={`${sortBy}-${sortOrder}`}
            onChange={(value) => {
              const [by, order] = value.split('-');
              setSortBy(by);
              setSortOrder(order);
            }}
            options={[
              { value: 'createdAt-desc', label: 'Newest' },
              { value: 'createdAt-asc', label: 'Oldest' },
              { value: 'price-asc', label: 'Price: Low to High' },
              { value: 'price-desc', label: 'Price: High to Low' },
              { value: 'name-asc', label: 'Name: A-Z' },
              { value: 'name-desc', label: 'Name: Z-A' },
            ]}
          />
        </Col>
      </Row>

      {loading ? (
        <div style={{ textAlign: 'center', padding: '50px' }}>
          <Spin size="large" />
        </div>
      ) : products.length === 0 ? (
        <Empty description="No products found" />
      ) : (
        <>
          <Row gutter={[16, 16]}>
            {products.map((product) => (
              <Col xs={24} sm={12} md={8} lg={6} key={product.id}>
                <Card
                  hoverable
                  cover={
                    <div
                      style={{
                        height: 200,
                        background: '#f5f5f5',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                      }}
                    >
                      {product.imageUrl ? (
                        <img
                          alt={product.name}
                          src={product.imageUrl}
                          style={{ maxHeight: '100%', maxWidth: '100%', objectFit: 'cover', mixBlendMode: 'multiply' }}
                        />
                      ) : (
                        <ShoppingCartOutlined style={{ fontSize: 48, color: '#d9d9d9' }} />
                      )}
                    </div>
                  }
                  actions={[
                    <Link href={`/products/${product.id}`} key="view">
                      View Details
                    </Link>,
                    <Button
                      type="link"
                      key="cart"
                      icon={<ShoppingCartOutlined />}
                      onClick={() => handleAddToCart(product.id)}
                    >
                      Add
                    </Button>,
                  ]}
                >
                  <Meta
                    title={product.name}
                    description={
                      <>
                        <div style={{ color: '#1890ff', fontWeight: 'bold', fontSize: '16px' }}>
                          {formatPrice(product.price)}
                        </div>
                        <div style={{ color: '#999', fontSize: '12px' }}>
                          {product.categoryName}
                        </div>
                        <div style={{ color: product.stock > 0 ? '#52c41a' : '#ff4d4f', fontSize: '12px' }}>
                          {product.stock > 0 ? `${product.stock} in stock` : 'Out of stock'}
                        </div>
                      </>
                    }
                  />
                </Card>
              </Col>
            ))}
          </Row>

          <div style={{ textAlign: 'center', marginTop: '24px' }}>
            <Pagination
              current={page}
              pageSize={pageSize}
              total={total}
              onChange={setPage}
              showSizeChanger={false}
            />
          </div>
        </>
      )}
    </div>
  );
}
