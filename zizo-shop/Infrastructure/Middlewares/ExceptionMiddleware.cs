using System.Net;

namespace zizo_shop.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try { await _next(httpContext); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex, _env.IsDevelopment());
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, bool isDevelopment)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            string message;

            switch (exception)
            {
                case FluentValidation.ValidationException valEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    var errors = valEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                    context.Response.StatusCode = statusCode;
                    return context.Response.WriteAsJsonAsync(new { StatusCode = statusCode, Message = "Validation Failed", Errors = errors });

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = "Unauthorized access.";
                    break;

                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case InvalidOperationException:
                case ArgumentException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(new
            {
                StatusCode = statusCode,
                Message = message,
                Details = isDevelopment ? exception.StackTrace : null
            });
        }
    }
}
