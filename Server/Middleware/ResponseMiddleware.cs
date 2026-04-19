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
            var originalBodyStream = context.Response.Body;

            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await _next(context);

            newBody.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(newBody).ReadToEndAsync();
            context.Response.Body = originalBodyStream;
            object? data = null;

            if (!string.IsNullOrWhiteSpace(bodyText))
            {
                data = JsonSerializer.Deserialize<object>(bodyText);
            }

            
            var response = new ApiResponse<object>
            {
                Success = context.Response.StatusCode < 400,
                StatusCode = context.Response.StatusCode,
                Message = context.Response.StatusCode >= 400 ? (_env.IsDevelopment() ? bodyText : "An internal error occurred") : null,
                TraceId = context.TraceIdentifier,
                Data = context.Response.StatusCode < 400 ? data : null
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
