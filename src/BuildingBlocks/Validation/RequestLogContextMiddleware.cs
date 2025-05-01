using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace BuildingBlocks.Validation;

public class RequestLogContextMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    
    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
        {
            return _next(context);
        }
    }
}