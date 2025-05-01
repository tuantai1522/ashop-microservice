using Basket.API.Entities;

namespace Basket.API.Contracts;

public record ShoppingCartItemDto(Color Color, Guid ProductId, string ProductName, decimal Price, decimal Quantity);
