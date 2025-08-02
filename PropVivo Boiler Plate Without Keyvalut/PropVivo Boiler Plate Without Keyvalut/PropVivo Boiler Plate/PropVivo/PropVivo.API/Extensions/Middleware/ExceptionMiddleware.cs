using PropVivo.Application.Common.Exceptions;
using System.Data.SqlClient;
using System.Security.Authentication;
using System.Text.Json;
using ApplicationException = PropVivo.Application.Common.Exceptions.ApplicationException;

namespace PropVivo.API.Extensions.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }

        private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
        {
            IReadOnlyDictionary<string, string[]>? errors = null;
            if (exception is ValidationException validationException)
            {
                errors = validationException.ErrorsDictionary;
            }
            return errors;
        }

        private static int GetStatusCode(Exception exception) =>
      exception switch
      {
          AuthenticationException => StatusCodes.Status401Unauthorized,
          BadRequestException => StatusCodes.Status400BadRequest,
          NotFoundException => StatusCodes.Status404NotFound,
          ValidationException => StatusCodes.Status422UnprocessableEntity,
          SqlException e when e.Number == 5000 => StatusCodes.Status400BadRequest,
          _ => StatusCodes.Status500InternalServerError
      };

        private static string GetTitle(Exception exception) =>
            exception switch
            {
                ApplicationException applicationException => applicationException.Title,
                _ => "Server Error"
            };

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new
            {
                title = GetTitle(exception),
                statuscode = statusCode,
                message = exception.Message,
                detail = exception.StackTrace,
                errors = GetErrors(exception)
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}