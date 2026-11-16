// Body of POST User/login.
export interface LoginRequest {
  email: string;
  password: string;
}

// Body of POST User/register.
export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  userType: 'BEEKEEPER';
}

// Data returned by login and register.
export interface AuthResult {
  token: string;
  tokenType: string;
  expiresAt: string;
  userId: string;
  email: string;
  userType: string;
}
