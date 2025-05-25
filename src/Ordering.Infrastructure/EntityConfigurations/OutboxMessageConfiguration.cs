using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Aggregate.OutboxMessageAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");
        
        // To configure Strongly typed IDs
        builder.Property(o => o.Id).HasConversion(
            ticket => ticket.Value,
            value => new OutboxMessageId(value));

        builder.Property(o => o.Type)
            .IsRequired()
            .HasMaxLength(100);
        
        // builder.OwnsOne(x => x.Content, ownedNavigationBuilder =>
        // {
        //     ownedNavigationBuilder.ToJson();
        // });
        
        builder.Property(o => o.Error)
            .HasMaxLength(1024);

    }
}