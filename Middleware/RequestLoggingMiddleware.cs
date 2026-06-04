using System.Diagnostics;

namespace hospital.Middleware
{
    // Custom middleware used to log details about each HTTP request
    public class RequestLoggingMiddleware
    {
        // Delegate representing the next middleware in the request pipeline
        private readonly RequestDelegate _next;

        // Logger instance used to write request logs
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        // Constructor for dependency injection
        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Method executed for every incoming HTTP request
        public async Task Invoke(HttpContext context)
        {
            // Starts a timer to measure request processing time
            var stopwatch = Stopwatch.StartNew();

            // Passes the request to the next middleware in the pipeline
            await _next(context);

            // Stops the timer after request processing is completed
            stopwatch.Stop();

            // Logs request method, URL path, response status code, and total time taken to process the request
            _logger.LogInformation(
                "Method: {Method} | Path: {Path} | StatusCode: {StatusCode} | ResponseTime: {Elapsed}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}