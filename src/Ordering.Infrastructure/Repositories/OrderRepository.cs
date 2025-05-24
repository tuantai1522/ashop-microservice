using Microsoft.EntityFrameworkCore;
using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.OrderAggregate;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderingContext context) : IOrderRepository
{
    private readonly OrderingContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
    }

    public async Task UpdateAsync(Guid oderId,
        string street, string city, string? state, string country, string? zipCode,
        string cardName, string cardNumber, string? expiration, string cvv,
        CancellationToken cancellationToken)
    {
        await _context.Orders
            .Where(b => b.Id.Value == oderId)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(b => b.ShippingAddress.Street, street)
                    .SetProperty(b => b.ShippingAddress.City, city)
                    .SetProperty(b => b.ShippingAddress.State, state)
                    .SetProperty(b => b.ShippingAddress.Country, country)
                    .SetProperty(b => b.Payment.CardName, cardName)
                    .SetProperty(b => b.Payment.CardName, cardName)
                    .SetProperty(b => b.Payment.CardNumber, cardNumber)
                    .SetProperty(b => b.Payment.Expiration, expiration)
                    .SetProperty(b => b.Payment.CVV, cvv),
                cancellationToken: cancellationToken);
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(x => x.Id.Value == orderId, cancellationToken);

    }

    public async Task<IReadOnlyList<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(x => x.CustomerId.Value == customerId)
            .ToListAsync(cancellationToken);
    }
}