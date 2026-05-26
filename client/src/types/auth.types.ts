export interface RegisterRequest {
  email: string;
  password: string;
  name: string;
  type: UserType;
}

export interface RegisterResponse {   
  token: string;
}

export const UserType = {
  BEEKEEPER: 0,
  FARMER: 1,
  ADMIN: 2
} as const;

export type UserType = typeof UserType[keyof typeof UserType];


