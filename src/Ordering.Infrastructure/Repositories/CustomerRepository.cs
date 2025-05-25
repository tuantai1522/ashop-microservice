using Microsoft.EntityFrameworkCore;
using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.CustomerAggregate;

namespace Ordering.Infrastructure.Repositories;

public class CustomerRepository(OrderingContext context) : ICustomerRepository
{
    private readonly OrderingContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.Id.Value == customerId, cancellationToken);

    }
}