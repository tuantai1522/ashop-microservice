using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.OrderAggregate;

namespace Ordering.Core.Aggregate.CustomerAggregate;

public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// To get customer by id
    /// </summary>
    /// <param name="customerId">
    /// ID of customer to be fetched
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token to cancel the operation
    /// </param>
    /// <returns></returns>
    Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken);
}