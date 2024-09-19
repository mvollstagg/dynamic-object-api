using IODynamicObject.Core.Exceptions;
using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace IODynamicObject.API.Middleware
{
    public class IOExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IOExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public IOExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<IOExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                // Handle the exception and create IOResult
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Determine the status code and result status
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            IOResultStatusEnum resultStatus = IOResultStatusEnum.Error;

            // Set the response status code and content type
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            // Create IOResult with metadata and error messages
            var ioResult = new IOResult<string>(resultStatus);
            ioResult.Meta.AddMessage("Error", exception.Message);

            // Include stack trace in development environment
            if (_env.IsDevelopment())
            {
                ioResult.Meta.AddMessage("StackTrace", exception.StackTrace);
            }

            // Serialize IOResult to JSON
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonResult = JsonSerializer.Serialize(ioResult, options);

            // Write the response
            return context.Response.WriteAsync(jsonResult);
        }
    }
}