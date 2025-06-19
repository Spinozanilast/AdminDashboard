'use client';

import { useAuth } from '@/contexts/auth-context';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';

const API_BASE_URL = 'http://localhost:5000';

// Types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
}

export interface Client {
  id: string;
  name: string;
  email: string;
  balanceT: number;
}

export interface Payment {
  id: string;
  clientId: string;
  clientName: string;
  amount: number;
  tokensAmount: number;
  status: string;
  createdAt: string;
}

export interface ExchangeRate {
  rate: number;
  lastUpdated: string;
}

export interface UpdateRateRequest {
  newRate: number;
}

const apiRequest = async (endpoint: string, options: RequestInit = {}) => {
  const token = localStorage.getItem('token');

  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
      ...options.headers,
    },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `HTTP ${response.status}`);
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
};

export const authApi = {
  login: async (credentials: LoginRequest): Promise<AuthResponse> => {
    return apiRequest('/auth/login', {
      method: 'POST',
      body: JSON.stringify(credentials),
    });
  },
};

export const clientsApi = {
  getAll: async (): Promise<Client[]> => {
    return apiRequest('/clients');
  },
};

export const paymentsApi = {
  getRecent: async (take = 5): Promise<Payment[]> => {
    return apiRequest(`/payments?take=${take}`);
  },
};

export const rateApi = {
  get: async (): Promise<ExchangeRate> => {
    return apiRequest('/rate');
  },
  update: async (data: UpdateRateRequest): Promise<void> => {
    return apiRequest('/rate', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },
};

// React Query hooks
export const useLogin = () => {
  const { login } = useAuth();

  return useMutation({
    mutationFn: authApi.login,
    onSuccess: (data) => {
      login(data.token, data.refreshToken);
    },
  });
};

export const useClients = () => {
  return useQuery({
    queryKey: ['clients'],
    queryFn: clientsApi.getAll,
  });
};

export const usePayments = (take = 5) => {
  return useQuery({
    queryKey: ['payments', take],
    queryFn: () => paymentsApi.getRecent(take),
  });
};

export const useExchangeRate = () => {
  return useQuery({
    queryKey: ['exchangeRate'],
    queryFn: rateApi.get,
  });
};

export const useUpdateRate = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: rateApi.update,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['exchangeRate'] });
    },
  });
};
