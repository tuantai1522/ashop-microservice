using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Aggregate.CustomerAggregate;
using Ordering.Core.Aggregate.OrderAggregate;
using Ordering.Core.Aggregate.OutboxMessageAggregate;
using Ordering.Infrastructure.BackgroundJobs;
using Ordering.Infrastructure.Interceptors;
using Ordering.Infrastructure.Repositories;
using Quartz;

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

        services
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        
        // Add quartz
        services.AddQuartz(configure =>
        {
            // This job will run in memory, not to store in database
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            // Repeat per 5 seconds
            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));
        });
        
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        
        return services;
    }
}