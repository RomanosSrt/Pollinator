export interface ApiResponse<T> {
  data?: T;
  message?: string;
}

/**
 * Error response wrapper
 * Backend returns: { error: "message", code?: "ERROR_CODE" }
 */
export interface ApiErrorResponse {
  error: string;
  code?: string;
  details?: Record<string, string[]>; // For validation errors
}

/**
 * Paginated response
 * Use for list endpoints that return multiple items
 */
export interface PaginatedResponse<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  hasMore: boolean;
}

/**
 * HTTP Status Codes (for reference)
 */
export const HttpStatus = {
  OK: 200,
  CREATED: 201,
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  CONFLICT: 409,
  INTERNAL_SERVER_ERROR: 500,
}

export type HttpStatus = typeof HttpStatus[keyof typeof HttpStatus];
