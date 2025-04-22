namespace Basket.API.Entities;

public class ShoppingCart
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    
    public string UserName { get; set; } = null!;
    
    public List<ShoppingCartItem> Items { get; set; } = [];
    
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    //Required for Mapping
    public ShoppingCart()
    {
    }
}