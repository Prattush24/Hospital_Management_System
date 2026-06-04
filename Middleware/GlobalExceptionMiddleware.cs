using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace hospital.Middleware
{
       // Custom middleware used for handling exceptions globally
    public class GlobalExceptionMiddleware
    {
        // Delegate representing the next middleware in the request pipeline
        private readonly RequestDelegate _next;

        // Logger instance used for logging exception details
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        // Constructor for dependency injection
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Method invoked for every incoming HTTP request
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Passes the request to the next middleware in the pipeline
                await _next(context);
            }
            catch (SqlException ex)
            {
                // Handles SQL/database-related exceptions

                // Sets HTTP status code to 400 (Bad Request)
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                // Returns exception message as JSON response
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                // Handles invalid argument exceptions

                // Sets HTTP status code to 400 (Bad Request)
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                // Returns exception message as JSON response
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Handles all unhandled exceptions

                // Sets HTTP status code to 500 (Internal Server Error)
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // Returns exception message as JSON response
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}