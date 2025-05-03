using Discount.GRPC.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Data;

public class DiscountContext(DbContextOptions<DiscountContext> options) : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductId = Guid.Parse("019684e3-9bdd-7cf5-812c-3bc984d94b9e"), Description = "IPhone Discount", Rate = 0.3 },
            new Coupon { Id = 2, ProductId = Guid.Parse("019684e3-9bdd-7cf5-812c-3bc984d94b9e"), Description = "IPhone Discount", Rate = 0.3 },
            new Coupon { Id = 3, ProductId = Guid.Parse("019684e3-9bdd-747f-a9f6-59f022b24aae"), Description = "Samsung Discount", Rate = 0.1 },
            new Coupon { Id = 4, ProductId = Guid.Parse("019684e3-9bdd-741e-b3a2-6869f6e0a414"), Description = "Macbook Discount", Rate = 0.1 }
        );
    }
}