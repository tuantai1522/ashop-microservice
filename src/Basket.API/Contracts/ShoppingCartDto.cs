namespace Basket.API.Contracts;

public record ShoppingCartDto(string UserName, List<ShoppingCartItemDto> Items, decimal TotalPrice);
