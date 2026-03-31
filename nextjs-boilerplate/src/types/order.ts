export interface OrderItem {
  id: number;
  productId: number;
  productName: string | null;
  productImageUrl: string | null;
  quantity: number;
  price: number;
  subtotal: number;
}

export interface Order {
  id: number;
  status: string;
  shippingAddress: string | null;
  totalPrice: number;
  items: OrderItem[];
  createdAt: string;
  updatedAt: string;
}

export interface CheckoutRequest {
  shippingAddress: string;
  notes?: string;
}

export interface DashboardResponse {
  totalUsers: number;
  totalProducts: number;
  totalOrders: number;
  totalRevenue: number;
  pendingOrders: number;
  processingOrders: number;
  shippedOrders: number;
  deliveredOrders: number;
  cancelledOrders: number;
}
