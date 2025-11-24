
namespace SmartPlate.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger; // Logger to record exceptions

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Attempt to pass the request to the next middleware
                await _next(context);
            }
            // If an exception occurs, handle it 
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // Handling of the exception and forming a proper HTTP response
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception");

            // Determine HTTP status code and message based on exception type
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