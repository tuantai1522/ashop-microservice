using BuildingBlocks.Validation;
using MediatR;
using Ordering.Core.Aggregate.OrderAggregate;

namespace Ordering.UseCases.Orders.Commands;

public class CreateOrderCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// ID of customer
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Information of address to ship order
    /// </summary>
    public string Street { get; init; } = null!;

    public string City { get; init; } = null!;

    public string? State { get; init; }

    public string Country { get; init; } = null!;

    public string? ZipCode { get; init; }

    /// <summary>
    /// Information to pay for this order
    /// </summary>
    public string CardName { get; init; } = null!;

    public string CardNumber { get; init; } = null!;

    public string? Expiration { get; init; }

    public string CVV { get; init; } = null!;

    public IReadOnlyCollection<OrderItemDto> Items { get; init; } = [];

}

public record OrderItemDto(Guid ProductId, int Quantity, decimal Price)
{
    public Guid ProductId { get; } = ProductId;
    public int Quantity { get; } = Quantity;
    public decimal Price { get; } = Price;
}