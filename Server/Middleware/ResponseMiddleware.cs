using System.Diagnostics;
using System.Text.Json;

namespace API.Middleware
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ResponseMiddleware> _logger;

        public ResponseMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ResponseMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var method = context.Request.Method;
            var path = context.Request.Path;

            _logger.LogInformation("Request started {Method} {Path}", method, path);

            var originalBodyStream = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            try
            {
                await _next(context);
                await HandleSuccessResponse(context, newBody, originalBodyStream);
                _logger.LogInformation("Request completed {Method} {Path} → {StatusCode} in {ElapsedMs}ms",
                    method, path, context.Response.StatusCode, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                await HandleErrorResponse(context, ex, originalBodyStream, method, path);
                _logger.LogInformation("Request completed {Method} {Path} → {StatusCode} in {ElapsedMs}ms",
                    method, path, context.Response.StatusCode, sw.ElapsedMilliseconds);
            }
        }
        private async Task HandleSuccessResponse(HttpContext context, MemoryStream newBody, Stream originalBodyStream)
        {
            newBody.Seek(0, SeekOrigin.Begin);
            var BodyText = await new StreamReader(newBody).ReadToEndAsync();

            object? responseData = null;
            if (!string.IsNullOrEmpty(BodyText))
            {
                try
                {
                    responseData = JsonSerializer.Deserialize<object>(BodyText);
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to deserialize response body. Returning raw string.");
                    responseData = BodyText;
                    var ErrorResponse = new ApiResponse<object> { 
                        Message = $"Deserialization failure due to error: {ex.Message}", 
                        Success = false, 
                        StatusCode = context.Response.StatusCode, 
                        TraceId = context.TraceIdentifier 
                    };
                    await WriteResponse(context, ErrorResponse, originalBodyStream);
                }
            }

            var successResponse = new ApiResponse<object>
            {
                Success = context.Response.StatusCode < 400,
                StatusCode = context.Response.StatusCode,
                Message = context.Response.StatusCode >= 400
                    ? (_env.IsDevelopment() ? BodyText : "An internal error occurred")
                    : null,
                TraceId = context.TraceIdentifier,
                Data = context.Response.StatusCode < 400 ? responseData : null
            };
            await WriteResponse(context, successResponse, originalBodyStream);
        }


        private async Task HandleErrorResponse(HttpContext context, Exception ex, Stream originalBodyStream, string method, string path)
        {
            _logger.LogError(ex, "Unhandled exception during request {Method} {Path}", method, path);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var errorResponse = new ApiResponse<object>
            {
                Success = false,
                StatusCode = context.Response.StatusCode,
                Message = _env.IsDevelopment() ? ex.Message : "An internal error occurred",
                TraceId = context.TraceIdentifier
            };

            await WriteResponse(context, errorResponse, originalBodyStream);
        }


        private async Task WriteResponse(HttpContext context, ApiResponse<object> response, Stream originalBodyStream)
        {
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            var responseBody = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseBody);
        }
    }
}
