import { create } from 'zustand';
import api from '@/services/api';
import { API_ENDPOINTS } from '@/constants/api';
import { User, LoginRequest, LoginResponse, RegisterRequest } from '@/types/user';

interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
  login: (data: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => void;
  loadUser: () => Promise<void>;
  clearError: () => void;
  hydrate: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  token: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,

  hydrate: () => {
    if (typeof window === 'undefined') return;
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');
    set({
      token,
      user: user ? JSON.parse(user) : null,
      isAuthenticated: !!token,
    });
  },

  login: async (data: LoginRequest) => {
    set({ isLoading: true, error: null });
    try {
      const response = await api.post<LoginResponse>(API_ENDPOINTS.AUTH.LOGIN, data);
      const { token, ...userData } = response.data;

      const user: User = {
        id: 0,
        email: userData.email,
        firstName: userData.firstName,
        lastName: userData.lastName,
        role: userData.role as 'Customer' | 'Admin',
        isActive: true,
        createdAt: new Date().toISOString(),
      };

      localStorage.setItem('token', token);
      localStorage.setItem('user', JSON.stringify(user));

      set({ user, token, isAuthenticated: true, isLoading: false });
    } catch (error: unknown) {
      const err = error as { response?: { data?: string } };
      set({ error: err.response?.data || 'Login failed', isLoading: false });
      throw error;
    }
  },

  register: async (data: RegisterRequest) => {
    set({ isLoading: true, error: null });
    try {
      await api.post(API_ENDPOINTS.AUTH.REGISTER, data);
      set({ isLoading: false });
    } catch (error: unknown) {
      const err = error as { response?: { data?: string } };
      set({ error: err.response?.data || 'Registration failed', isLoading: false });
      throw error;
    }
  },

  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    set({ user: null, token: null, isAuthenticated: false });
  },

  loadUser: async () => {
    const token = localStorage.getItem('token');
    if (!token) return;

    set({ isLoading: true });
    try {
      const response = await api.get<User>(API_ENDPOINTS.AUTH.ME);
      localStorage.setItem('user', JSON.stringify(response.data));
      set({ user: response.data, isAuthenticated: true, isLoading: false });
    } catch {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      set({ user: null, token: null, isAuthenticated: false, isLoading: false });
    }
  },

  clearError: () => set({ error: null }),
}));
