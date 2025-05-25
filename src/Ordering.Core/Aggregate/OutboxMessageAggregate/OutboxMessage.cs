using Ordering.Core.Abstraction;

namespace Ordering.Core.Aggregate.OutboxMessageAggregate;

public class OutboxMessage : Entity, IAggregateRoot
{
    public OutboxMessageId Id { get; init; } = OutboxMessageId.CreateNew();
    
    /// <summary>
    /// Type of message, e.g. OrderCreatedEvent
    /// </summary>
    public required string Type { get; init; }
    
    /// <summary>
    /// Content of the message, serialized as JSON
    /// </summary>
    public required string Content { get; init; }
    
    public DateTime? ProcessedAt { get; set; }
    
    public string? Error { get; set; }
}