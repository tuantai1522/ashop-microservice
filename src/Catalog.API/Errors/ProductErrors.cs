using BuildingBlocks.Validation;

namespace Catalog.API.Errors;

public static class ProductErrors
{
    /// <summary>
    /// Generates an error indicating that the product ID was not found.
    /// </summary>
    /// <param name="id">The ID of the product that was not found.</param>
    /// <returns>An <see cref="Error"/> object with the error details.</returns>
    public static Error NotFound(Guid id) => Error.NotFound("404", $"Product Id: {id} can not found");
}