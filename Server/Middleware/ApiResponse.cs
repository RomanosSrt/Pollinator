namespace API.Middleware
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public T? Message { get; set; }
        public string? TraceId { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, int statusCode, string traceId)
            => new()
            {
                Success = true,
                StatusCode = statusCode,
                TraceId = traceId,
                Data = data
            };

        public static ApiResponse<T> FailResponse(T message, int statusCode, string traceId)
            => new()
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                TraceId = traceId
            };
    }
}
