using System.Net;
using System.Text.Json;
using FluentValidation;
using ECommerceAPI.Application.Shared.Errors;

namespace ECommerceAPI.API.Middleware;

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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ValidationException => HttpStatusCode.BadRequest,
            InvalidOperationException => HttpStatusCode.Conflict,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        var message = exception switch
        {
            KeyNotFoundException ex => ex.Message,
            UnauthorizedAccessException ex => ex.Message,
            ValidationException => "Validation failed",
            InvalidOperationException ex => ex.Message,
            ArgumentException ex => ex.Message,
            _ => "An internal server error occurred"
        };

        context.Response.StatusCode = (int)statusCode;

        ApiErrorResponse response;

        if (exception is ValidationException validationEx)
        {
            response = new ApiErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = "Validation Error",
                Details = string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))
            };
        }
        else
        {
            response = new ApiErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = message
            };
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
