namespace Ordering.Core.Aggregate.ProductAggregate;

/// <summary>
/// Strongly typed OrderId.
/// </summary>
/// <param name="Value"></param>
public readonly record struct ProductId(Guid Value)
{
    public static ProductId CreateNew() => new (Guid.CreateVersion7());
}