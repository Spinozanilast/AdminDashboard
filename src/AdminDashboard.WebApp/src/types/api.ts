// Auth types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken?: string;
  expiresAt?: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

////////////////////////////////////////////////////////////////////////////////////////////////////

// Client types
export interface ClientDto {
  id: string;
  name: string;
  email: string;
  balanceT: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateClientRequest {
  name: string;
  email: string;
  balanceT: number;
}

export interface UpdateClientRequest {
  id: string;
  name: string;
  email: string;
  balanceT: number;
}

export interface DeleteClientsRequest {
  clientIds: string[];
}

////////////////////////////////////////////////////////////////////////////////////////////////////

// Payment types
export interface PaymentDto {
  id: string;
  clientId: string;
  clientName: string;
  amount: number;
  tokensAmount: number;
  status: string;
  createdAt: string;
}

export interface GetPaymentsRequest {
  take?: number;
}

////////////////////////////////////////////////////////////////////////////////////////////////////

// Exchange rate types
export interface ExchangeRateDto {
  rate: number;
  lastUpdated: string;
}

export interface UpdateExchangeRateRequest {
  newRate: number;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
