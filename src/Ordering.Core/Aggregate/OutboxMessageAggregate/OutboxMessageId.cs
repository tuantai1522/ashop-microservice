namespace Ordering.Core.Aggregate.OutboxMessageAggregate;

/// <summary>
/// Strongly typed OutboxMessageId.
/// </summary>
/// <param name="Value"></param>
public readonly record struct OutboxMessageId(Guid Value)
{
    public static OutboxMessageId CreateNew() => new (Guid.CreateVersion7());
}