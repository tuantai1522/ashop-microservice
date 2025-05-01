using BuildingBlocks.Validation;

namespace Basket.API.Errors;

public static class ShoppingCartErrors
{
    /// <summary>
    /// Generates an error indicating that the shopping cart ID was not found.
    /// </summary>
    /// <param name="id">The ID of the shopping cart that was not found.</param>
    /// <returns>An <see cref="Error"/> object with the error details.</returns>
    public static Error NotFound(Guid id) => Error.NotFound("404", $"Shopping Cart Id: {id} can not found");
}