'use client';

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/contexts/auth-context';

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';

// Types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface ProblemDetails {
  title: string;
  detail: string;
  status: number;
  type?: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresAt?: string;
}

export interface RefreshTokenRequest {
  token: string;
  refreshToken: string;
}

export interface Client {
  id: string;
  name: string;
  email: string;
  balanceT: number;
  tags: Tag[];
}

export interface Payment {
  id: string;
  amount: number;
  date: string;
  description: string;
  clientId: string;
  clientEmail: string;
}

export interface ExchangeRate {
  exchangeRate: number;
  lastUpdated: string;
}

export interface UpdateRateRequest {
  newRate: number;
}

// Add new types for client operations
export interface CreateClientRequest {
  name: string;
  email: string;
  balanceT: number;
}

export interface Tag {
  name: string;
  color: string;
}

export interface UpdateClientRequest {
  id?: string;
  name: string;
  email: string;
  balanceT: number;
  tags: Tag[];
}

export interface ValidationError {
  [key: string]: string[];
}

export interface DeleteClientsRequest {
  clientIds: string[];
}

export class ApiError extends Error {
  constructor(
    message: string,
    public status: number,
    public validationErrors?: ValidationError,
    public problemDetails?: ProblemDetails
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

// Token refresh functionality
let isRefreshing = false;
let refreshPromise: Promise<string> | null = null;

const refreshAccessToken = async (): Promise<string> => {
  const token = localStorage.getItem('token');
  const refreshToken = localStorage.getItem('refreshToken');

  if (!token || !refreshToken) {
    throw new Error('No refresh token available');
  }

  const response = await fetch(`${API_BASE_URL}/auth/refresh`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      token,
      refreshToken,
    }),
  });

  if (!response.ok) {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    window.location.href = '/login';
    throw new Error('Token refresh failed');
  }

  const authResponse: AuthResponse = await response.json();

  localStorage.setItem('token', authResponse.token);
  if (authResponse.refreshToken) {
    localStorage.setItem('refreshToken', authResponse.refreshToken);
  }

  return authResponse.token;
};

const apiRequest = async (
  endpoint: string,
  options: RequestInit = {}
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
): Promise<any> => {
  const makeRequest = async (accessToken?: string): Promise<Response> => {
    const token = accessToken || localStorage.getItem('token');

    return fetch(`${API_BASE_URL}${endpoint}`, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
        ...options.headers,
      },
    });
  };

  let response = await makeRequest();

  // If we get a 401 and we have a refresh token, try to refresh
  if (response.status === 401) {
    const refreshToken = localStorage.getItem('refreshToken');

    if (refreshToken && !isRefreshing) {
      if (!refreshPromise) {
        isRefreshing = true;
        refreshPromise = refreshAccessToken().finally(() => {
          isRefreshing = false;
          refreshPromise = null;
        });
      }

      try {
        const newAccessToken = await refreshPromise;
        response = await makeRequest(newAccessToken);
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
      } catch (refreshError) {
        throw new Error('Authentication failed');
      }
    } else if (!refreshToken) {
      localStorage.removeItem('token');
      window.location.href = '/login';
      throw new Error('Authentication required');
    }
  }

  if (!response.ok) {
    const errorText = await response.text();
    let errorMessage = `HTTP ${response.status}: ${response.statusText}`;

    try {
      const errorData = JSON.parse(errorText);
      errorMessage = errorData.detail || errorData.message || errorMessage;
    } catch {
      if (errorText) {
        errorMessage = errorText;
      }
    }

    throw new Error(errorMessage);
  }

  if (response.status === 204) {
    return null;
  }

  const contentType = response.headers.get('content-type');
  if (contentType && contentType.includes('application/json')) {
    return response.json();
  }

  return response.text();
};

export const authApi = {
  login: async (credentials: LoginRequest): Promise<AuthResponse> => {
    return apiRequest('/auth/login', {
      method: 'POST',
      body: JSON.stringify(credentials),
    });
  },
  refresh: async (request: RefreshTokenRequest): Promise<AuthResponse> => {
    return apiRequest('/auth/refresh', {
      method: 'POST',
      body: JSON.stringify(request),
    });
  },
};

export const clientsApi = {
  getAll: async (): Promise<Client[]> => {
    return apiRequest('/clients');
  },
  getById: async (id: string): Promise<Client> => {
    return apiRequest(`/clients/${id}`);
  },
  create: async (data: CreateClientRequest): Promise<string> => {
    return apiRequest('/clients', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },
  update: async (id: string, data: UpdateClientRequest): Promise<void> => {
    return apiRequest(`/clients/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },
  deleteMultiple: async (data: DeleteClientsRequest): Promise<void> => {
    return apiRequest('/clients/delete-multiple', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },
};

export const paymentsApi = {
  getRecent: async (take = 5): Promise<Payment[]> => {
    return apiRequest(`/payments?take=${take}`);
  },
};

// Update the rateApi object to use the correct endpoint
export const rateApi = {
  get: async (): Promise<ExchangeRate> => {
    return apiRequest('/rates');
  },
  update: async (data: UpdateRateRequest): Promise<void> => {
    return apiRequest('/rates', {
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

// Add new React Query hooks for client operations
export const useCreateClient = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: clientsApi.create,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clients'] });
    },
  });
};

export const useUpdateClient = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateClientRequest }) =>
      clientsApi.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clients'] });
    },
  });
};

export const useDeleteClients = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: clientsApi.deleteMultiple,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clients'] });
    },
  });
};

// Hook for manual token refresh (optional)
export const useRefreshToken = () => {
  const { login, logout } = useAuth();

  return useMutation({
    mutationFn: async () => {
      const token = localStorage.getItem('token');
      const refreshToken = localStorage.getItem('refreshToken');

      if (!token || !refreshToken) {
        throw new Error('No tokens available');
      }

      return authApi.refresh({ token, refreshToken });
    },
    onSuccess: (data) => {
      login(data.token, data.refreshToken);
    },
    onError: () => {
      logout();
    },
  });
};
