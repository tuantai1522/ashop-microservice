namespace Catalog.API.Entities;

public class Product
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public decimal Price { get; set; }
    
    public List<string> Category { get; set; } = [];
}