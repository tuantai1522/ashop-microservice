using Serilog.Context;

namespace Catalog.API;

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