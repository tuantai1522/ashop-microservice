using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Aggregate.OrderAggregate;
using Ordering.Core.Aggregate.ProductAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        // To configure Strongly typed IDs
        builder.Property(o => o.Id).HasConversion(
            ticket => ticket.Value,
            value => new OrderItemId(value));
        
        builder.Property(o => o.ProductId).HasConversion(
            ticket => ticket.Value,
            value => new ProductId(value));
        
        builder.Property(o => o.OrderId).HasConversion(
            ticket => ticket.Value,
            value => new OrderId(value));
        
    }
}