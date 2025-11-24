
namespace SmartPlate.Helpers
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception");

            var (status, message) = exception switch
            {
                UserNotFoundException => (404, exception.Message),
                InvalidPasswordException => (401, exception.Message),
                UserAlreadyExistsException => (409, exception.Message),

                _ => (500, "Internal server error")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;

            return context.Response.WriteAsJsonAsync(new { error = message });
        }
    }

}