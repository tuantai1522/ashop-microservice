using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Aggregate.OrderAggregate;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<OrderingContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        return services;
    }
}