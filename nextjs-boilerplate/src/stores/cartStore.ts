import { create } from 'zustand';
import api from '@/services/api';
import { API_ENDPOINTS } from '@/constants/api';
import { CartItem, AddToCartRequest } from '@/types/cart';

interface CartState {
  items: CartItem[];
  isLoading: boolean;
  error: string | null;
  totalItems: number;
  totalPrice: number;
  fetchCart: () => Promise<void>;
  addItem: (productId: number, quantity: number) => Promise<void>;
  updateItem: (id: number, quantity: number) => Promise<void>;
  removeItem: (id: number) => Promise<void>;
  clearCart: () => Promise<void>;
  clearError: () => void;
}

const calculateTotals = (items: CartItem[]) => ({
  totalItems: items.reduce((sum, item) => sum + item.quantity, 0),
  totalPrice: items.reduce((sum, item) => sum + item.subtotal, 0),
});

export const useCartStore = create<CartState>((set, get) => ({
  items: [],
  isLoading: false,
  error: null,
  totalItems: 0,
  totalPrice: 0,

  fetchCart: async () => {
    set({ isLoading: true, error: null });
    try {
      const response = await api.get<CartItem[]>(API_ENDPOINTS.CART.GET);
      const items = response.data;
      const totals = calculateTotals(items);
      set({ items, ...totals, isLoading: false });
    } catch {
      set({ isLoading: false, error: 'Failed to fetch cart' });
    }
  },

  addItem: async (productId: number, quantity: number) => {
    set({ isLoading: true, error: null });
    try {
      const data: AddToCartRequest = { productId, quantity };
      const response = await api.post<CartItem>(API_ENDPOINTS.CART.ADD, data);
      
      const existingIndex = get().items.findIndex(item => item.productId === productId);
      let newItems: CartItem[];
      
      if (existingIndex >= 0) {
        newItems = [...get().items];
        newItems[existingIndex] = response.data;
      } else {
        newItems = [...get().items, response.data];
      }
      
      const totals = calculateTotals(newItems);
      set({ items: newItems, ...totals, isLoading: false });
    } catch (error: unknown) {
      const err = error as { response?: { data?: string } };
      set({ error: err.response?.data || 'Failed to add item', isLoading: false });
      throw error;
    }
  },

  updateItem: async (id: number, quantity: number) => {
    set({ isLoading: true, error: null });
    try {
      const response = await api.put<CartItem>(API_ENDPOINTS.CART.UPDATE(id), { quantity });
      const newItems = get().items.map(item => item.id === id ? response.data : item);
      const totals = calculateTotals(newItems);
      set({ items: newItems, ...totals, isLoading: false });
    } catch (error: unknown) {
      const err = error as { response?: { data?: string } };
      set({ error: err.response?.data || 'Failed to update item', isLoading: false });
      throw error;
    }
  },

  removeItem: async (id: number) => {
    set({ isLoading: true, error: null });
    try {
      await api.delete(API_ENDPOINTS.CART.DELETE(id));
      const newItems = get().items.filter(item => item.id !== id);
      const totals = calculateTotals(newItems);
      set({ items: newItems, ...totals, isLoading: false });
    } catch {
      set({ error: 'Failed to remove item', isLoading: false });
    }
  },

  clearCart: async () => {
    set({ isLoading: true, error: null });
    try {
      await api.delete(API_ENDPOINTS.CART.CLEAR);
      set({ items: [], totalItems: 0, totalPrice: 0, isLoading: false });
    } catch {
      set({ error: 'Failed to clear cart', isLoading: false });
    }
  },

  clearError: () => set({ error: null }),
}));
