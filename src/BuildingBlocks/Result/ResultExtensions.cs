using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Result;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            statusCode: GetStatusCode(result.Error.Type),
            title: GetTitle(result.Error.Type),
            type: GetType(result.Error.Type),
            extensions: new Dictionary<string, object?>
            {
                ["errors"] = new[] { result.Error }
            }
        );
    }
    
    
    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.BadRequest => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.BadRequest => "Bad Request",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.NotFound => "Not Found",
            ErrorType.Conflict => "Conflict",
            ErrorType.Validation => "Validation Error",
            _ => "Internal Server Error"
        };

    private static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.BadRequest => "https://httpstatuses.com/400",
            ErrorType.Unauthorized => "https://httpstatuses.com/401",
            ErrorType.NotFound => "https://httpstatuses.com/404",
            ErrorType.Conflict => "https://httpstatuses.com/409",
            ErrorType.Validation => "https://httpstatuses.com/422",
            _ => "https://httpstatuses.com/500"
        };
}