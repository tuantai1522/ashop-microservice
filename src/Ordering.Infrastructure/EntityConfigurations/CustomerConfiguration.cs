using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Aggregate.CustomerAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        // To configure Strongly typed IDs
        builder.Property(o => o.Id).HasConversion(
            ticket => ticket.Value,
            value => new CustomerId(value));
        
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(100).IsRequired();
        
    }
}