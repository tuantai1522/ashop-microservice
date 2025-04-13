using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace BuildingBlocks.Result;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occured: {Message}", ex.Message);
            
            var exceptionDetails = GetExceptionDetails(ex);

            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail,
            };

            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }
            
            context.Response.StatusCode = exceptionDetails.Status;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }


    private static ExceptionDetails GetExceptionDetails(Exception ex)
    {
        return ex switch
        {
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                ex.Message,
                "An unexpected error occurred.",
                null
            )
        };
    }

    private record ExceptionDetails(int Status, string Type, string Title, string Detail, IEnumerable<object>? Errors);
}