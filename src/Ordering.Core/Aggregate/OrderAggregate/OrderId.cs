namespace Ordering.Core.Aggregate.OrderAggregate;

/// <summary>
/// Strongly typed OrderId.
/// </summary>
/// <param name="Value"></param>
public readonly record struct OrderId(Guid Value)
{
    public static OrderId CreateNew() => new (Guid.CreateVersion7());
}