namespace API.Middleware
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; } 
        public string? Message { get; set; }
        public string? TraceId { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, int statusCode, string traceId)
            => new()
            {
                Success = true,
                StatusCode = statusCode,
                TraceId = traceId,
                Message = null,
                Data = data
            };

        public static ApiResponse<string> FailResponse(string message, int statusCode, string traceId)
            => new()
            {
                Success = false,
                StatusCode = statusCode,
                Data = null,
                Message = message,
                TraceId = traceId
            };
    }
}
