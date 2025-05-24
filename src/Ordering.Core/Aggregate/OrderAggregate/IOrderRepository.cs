using Ordering.Core.Abstraction;

namespace Ordering.Core.Aggregate.OrderAggregate;

public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// To add new order in the database
    /// </summary>
    /// <param name="order">
    /// Order to be added
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token to cancel the operation
    /// </param>
    /// <returns></returns>
    Task AddAsync(Order order, CancellationToken cancellationToken);

    /// <summary>
    /// To update current order by order id
    /// </summary>
    /// <param name="oderId"></param>
    /// <param name="street"></param>
    /// <param name="city"></param>
    /// <param name="state"></param>
    /// <param name="country"></param>
    /// <param name="zipCode"></param>
    /// <param name="cardName"></param>
    /// <param name="cardNumber"></param>
    /// <param name="expiration"></param>
    /// <param name="cvv"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(Guid oderId,
        string street, string city, string? state, string country, string? zipCode,
        string cardName, string cardNumber, string? expiration, string cvv,
        CancellationToken cancellationToken);

    /// <summary>
    /// To get order by order id
    /// </summary>
    /// <param name="orderId">
    /// ID of order to be fetched
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token to cancel the operation
    /// </param>
    /// <returns></returns>
    Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken);
    
    /// <summary>
    /// To get all orders by customer id
    /// </summary>
    /// <param name="custommerId">
    /// ID of customer to be fetched
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token to cancel the operation
    /// </param>
    /// <returns></returns>
    Task<IReadOnlyList<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken);
}