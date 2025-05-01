namespace Basket.API.Contracts;

/// <summary>
/// To define the request body for storing a basket.
/// </summary>
public class StoreBasketRequest
{
    public string UserName { get; set; } = null!;

    public IReadOnlyList<ShoppingCartItemDto> Items { get; set; } = [];
}