using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Aggregate.CustomerAggregate;
using Ordering.Core.Aggregate.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        // To configure Strongly typed IDs
        builder.Property(o => o.Id).HasConversion(
            ticket => ticket.Value,
            value => new OrderId(value));
        
        builder.Property(o => o.CustomerId).HasConversion(
            ticket => ticket.Value,
            value => new CustomerId(value));
        
        builder.Property(p => p.OrderName).HasMaxLength(100).IsRequired();
        
        // One ticket has multiple ticket line items
        builder.HasMany(r => r.OrderItems)
            .WithOne()
            .HasForeignKey(p => p.OrderId);
        
        // Configure shipping address value object
        builder.OwnsOne(property => property.ShippingAddress, address =>
        {
            address.Property(p => p.Street)
                .HasColumnName("Street")
                .HasMaxLength(256)
                .IsRequired();

            address.Property(p => p.ZipCode)
                .HasColumnName("ZipCode")
                .HasMaxLength(50);

            address.Property(p => p.Country)
                .HasColumnName("Country")
                .HasMaxLength(100)
                .IsRequired();
            
            address.Property(p => p.State)
                .HasColumnName("State")
                .HasMaxLength(100)
                .IsRequired();
            
            address.Property(p => p.City)
                .HasColumnName("City")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Configure payment value object
        builder.OwnsOne(property => property.Payment, address =>
        {
            address.Property(p => p.CardName)
                .HasColumnName("CardName")
                .HasMaxLength(256)
                .IsRequired();

            address.Property(p => p.CardNumber)
                .HasColumnName("CardNumber")
                .HasMaxLength(256)
                .IsRequired();

            address.Property(p => p.CVV)
                .HasColumnName("CVV")
                .HasMaxLength(100)
                .IsRequired();
        });

    }
}