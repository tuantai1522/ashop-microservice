using BuildingBlocks.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace BuildingBlocks.Behaviour;

public class RequestLoggingBehaviour<TRequest, TResponse>(ILogger<RequestLoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    private readonly ILogger _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Processing request: {@Request}", requestName);

        try
        {
            return await next(cancellationToken);

        }
        catch (ValidationException ex)
        {
            using (LogContext.PushProperty("Error", ex.Errors, true))
            {
                _logger.LogError("❌ Validation failed for {RequestName} with errors", requestName);
            }

            throw; // Vẫn throw để middleware xử lý response 400
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("Error", ex.Message, true))
            {
                _logger.LogError(ex, "Exception occured: {Message}", ex.Message);
            }
            
            throw;
        }
    }

}