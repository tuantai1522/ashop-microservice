namespace Catalog.API.Contracts;

/// <summary>
/// To define the request body for updating a product.
/// </summary>
public class UpdateProductRequest
{
    public string Name { get; set; } = null!;
        
    public string? Description { get; set; }
        
    public decimal Price { get; set; }
        
    public string? ImageUrl { get; set; }

    public List<string> Category { get; set; } = [];
}