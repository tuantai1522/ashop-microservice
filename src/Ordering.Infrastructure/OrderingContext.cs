using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.CustomerAggregate;
using Ordering.Core.Aggregate.OrderAggregate;
using Ordering.Core.Aggregate.ProductAggregate;

namespace Ordering.Infrastructure;

public class OrderingContext(DbContextOptions<OrderingContext> options) : DbContext(options), IUnitOfWork
{
    /// <summary>
    /// Define list entities in database
    /// </summary>
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ordering");
        
        // To apply all configurations in the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}