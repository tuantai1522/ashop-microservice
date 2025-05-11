namespace Ordering.Core.Aggregate.OrderAggregate;

/// <summary>
/// Strongly typed OrderId.
/// </summary>
/// <param name="Value"></param>
public readonly record struct OrderItemId(Guid Value)
{
    public static OrderItemId CreateNew() => new (Guid.CreateVersion7());
}