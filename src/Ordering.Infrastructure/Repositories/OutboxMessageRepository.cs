using Microsoft.EntityFrameworkCore;
using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.CustomerAggregate;
using Ordering.Core.Aggregate.OutboxMessageAggregate;

namespace Ordering.Infrastructure.Repositories;

public class OutboxMessageRepository(OrderingContext context) : IOutboxMessageRepository
{
    private readonly OrderingContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;
    
    public async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessageToExecute(int batchSize, CancellationToken cancellationToken)
    {
        return await _context.OutboxMessages
            .Where(message => message.ProcessedAt == null)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

}