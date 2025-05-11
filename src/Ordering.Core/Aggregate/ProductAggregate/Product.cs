using Ordering.Core.Abstraction;

namespace Ordering.Core.Aggregate.ProductAggregate;

public class Product : Entity, IAggregateRoot
{
    public ProductId Id { get; init; } = ProductId.CreateNew();
    
    public string Name { get; private set; } = null!;
    
    public decimal Price { get; private set; }

    private Product()
    {
        
    }
    
    public static Product Create(string name, decimal price)
    {
        return new Product
        {
            Name = name,
            Price = price,
        };
    }
}