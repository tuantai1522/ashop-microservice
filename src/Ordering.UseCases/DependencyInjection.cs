using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(
            assembly, 
            includeInternalTypes: true
        );

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
    
        });
        
        return services;
    }
}