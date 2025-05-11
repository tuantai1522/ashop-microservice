namespace Ordering.Core.Aggregate.CustomerAggregate;

/// <summary>
/// Strongly typed OrderId.
/// </summary>
/// <param name="Value"></param>
public readonly record struct CustomerId(Guid Value)
{
    public static CustomerId CreateNew() => new (Guid.CreateVersion7());
}