export const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5001';

export const API_ENDPOINTS = {
  // Auth
  AUTH: {
    LOGIN: '/api/auth/login',
    REGISTER: '/api/auth/register',
    ME: '/api/auth/user',
  },
  // Products
  PRODUCTS: {
    LIST: '/api/products',
    DETAIL: (id: number) => `/api/products/${id}`,
    CREATE: '/api/products',
    UPDATE: (id: number) => `/api/products/${id}`,
    DELETE: (id: number) => `/api/products/${id}`,
  },
  // Categories
  CATEGORIES: {
    LIST: '/api/categories',
    CREATE: '/api/categories',
  },
  // Cart
  CART: {
    GET: '/api/cart',
    ADD: '/api/cart',
    UPDATE: (id: number) => `/api/cart/${id}`,
    DELETE: (id: number) => `/api/cart/${id}`,
    CLEAR: '/api/cart',
  },
  // Orders
  ORDERS: {
    CHECKOUT: '/api/orders/checkout',
    LIST: '/api/orders',
    DETAIL: (id: number) => `/api/orders/${id}`,
    UPDATE_STATUS: (id: number) => `/api/orders/${id}/status`,
  },
  // Payments
  PAYMENTS: {
    CREATE_INTENT: '/api/payments/create-intent',
    CONFIRM: '/api/payments/confirm',
    WEBHOOK: '/api/payments/webhook',
  },
  // Admin
  ADMIN: {
    DASHBOARD: '/api/admin/dashboard',
    ORDERS: '/api/admin/orders',
    ORDER_DETAIL: (id: number) => `/api/admin/orders/${id}`,
    UPDATE_ORDER_STATUS: (id: number) => `/api/admin/orders/${id}/status`,
    USERS: '/api/admin/users',
    PRODUCTS: '/api/admin/products',
  },
};
