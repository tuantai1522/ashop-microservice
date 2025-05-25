using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Aggregate.OrderAggregate;
using Ordering.Infrastructure.Interceptors;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContextPool<OrderingContext>((provider, options) =>
        {
            var outboxMessageInterceptor = provider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            options.UseSqlServer(connectionString)
                .AddInterceptors(outboxMessageInterceptor!);
        });
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        return services;
    }
}