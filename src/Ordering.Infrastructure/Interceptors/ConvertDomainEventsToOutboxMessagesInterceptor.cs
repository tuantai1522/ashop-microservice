using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;
using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.OutboxMessageAggregate;

namespace Ordering.Infrastructure.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var outboxMessages = dbContext.ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var domainEvents = x.GetDomainEvents();

                x.ClearDomainEvents();
                
                return domainEvents;

            })
            .Select(domainEvent => new OutboxMessage
                {
                    Type = domainEvent.GetType().Name,
                    Content = JsonConvert.SerializeObject(domainEvent),
                })
            .ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}