// Envelope the API wraps around every response.
export interface ApiResponse<T> {
  success: boolean;
  statusCode: number;
  data: T | null;
  message: string | null;
  traceId: string | null;
}

// Extracts `data`, failing if the envelope reports an error.
export function unwrap<T>(res: ApiResponse<T>): T {
  if (!res.success || res.data === null) {
    throw new Error(res.message ?? 'Unexpected API response');
  }
  return res.data;
}
