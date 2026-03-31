export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: 'Customer' | 'Admin';
  isActive: boolean;
  createdAt: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  token: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface RegisterResponse {
  email: string;
  firstName: string;
  lastName: string;
  role: string;
}
