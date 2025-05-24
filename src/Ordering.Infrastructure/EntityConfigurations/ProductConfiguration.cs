using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Aggregate.ProductAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.Property(o => o.Price)
            .HasPrecision(18, 4);

        // To configure Strongly typed IDs
        builder.Property(o => o.Id).HasConversion(
            ticket => ticket.Value,
            value => new ProductId(value));
        
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        
    }
}