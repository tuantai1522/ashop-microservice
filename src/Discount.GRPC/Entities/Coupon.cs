namespace Discount.GRPC.Entities;

public sealed class Coupon
{
    /// <summary>
    /// ID of Coupon
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// ProductID from Catalog Service
    /// </summary>
    public Guid ProductId { get; set; }
    
    public string? Description { get; set; }

    /// <summary>
    /// Rate which this coupon can be applied to the product
    /// </summary>
    public double Rate { get; set; }
}