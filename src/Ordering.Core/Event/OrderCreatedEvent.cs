using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.OrderAggregate;

namespace Ordering.Core.Event;

public record OrderCreatedEvent(Order Order) : IDomainEvent;