using System.Net;

namespace zizo_shop.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Internal Server Error from the custom middleware.";
            if (exception != null)
            {
                message = exception.Message;
            }
            if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = "Unauthorized Access";
            }
            else if (exception is KeyNotFoundException || exception.Message.Contains("not found"))
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = "Not Found";
            }
            else if (exception is ArgumentException || exception is InvalidOperationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Bad Request";
            }
            else if (exception is FluentValidation.ValidationException valEx)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                var errors=valEx.Errors.Select(e=>new { e.PropertyName , e.ErrorMessage});
                message = string.Join("; ", valEx.Errors.Select(e => e.ErrorMessage));
                return context.Response.WriteAsJsonAsync(new
                {
                    statusCode = 400,
                    Message = "Validation Failed",
                    Errors = errors
                });
            }
            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode = statusCode
                ,
                Message = message,
                Details = exception.StackTrace?.ToString()
            };
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
