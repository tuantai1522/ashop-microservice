namespace Basket.API.Entities;

public class ShoppingCartItem
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public Color Color { get; set; } = Color.Yellow;
    
    public string ProductName { get; set; } = null!;
    
    public Guid ProductId { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
}