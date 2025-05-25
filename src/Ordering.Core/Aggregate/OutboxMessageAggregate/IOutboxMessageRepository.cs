using Ordering.Core.Abstraction;

namespace Ordering.Core.Aggregate.OutboxMessageAggregate;

public interface IOutboxMessageRepository : IRepository<OutboxMessage>
{
    /// <summary>
    /// To get all orders by customer id
    /// </summary>
    /// <param name="batchSize">
    /// Size to fetch every time when job is executed
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token to cancel the operation
    /// </param>
    /// <returns></returns>
    Task<IReadOnlyList<OutboxMessage>> GetOutboxMessageToExecute(int batchSize, CancellationToken cancellationToken);
}