namespace Catalog.API.Contracts;

public record ProductDto(Guid Id , string Name, string? Description, decimal Price, string? ImageUrl, List<string> Category);
