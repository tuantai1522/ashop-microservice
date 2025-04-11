using Catalog.API.Entities;
using Marten;
using Marten.Schema;

namespace Catalog.API.Data;

/// <summary>
/// To fake Catalog data, we need to implement the IInitialData interface.
/// </summary>
public sealed class InitialCatalogData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
        {
            return;
        }
        
        session.Store(GetProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetProducts() => new List<Product>
    {
        new()
        {
            Name = "iPhone 15", Description = "Apple smartphone", Price = 999, Category = ["Electronics", "Mobile"],
        },
        new()
        {
            Name = "Samsung Galaxy S23", Description = "Samsung flagship", Price = 899,
            Category = ["Electronics", "Mobile"],
        },
        new()
        {
            Name = "MacBook Pro", Description = "Apple laptop", Price = 1999, Category = ["Electronics", "Laptop"],
        },
        new()
        {
            Name = "Nike Air Max", Description = "Comfortable sneakers", Price = 120,
            Category = ["Footwear", "Fashion"],
        },
        new()
        {
            Name = "Sony WH-1000XM5", Description = "Noise cancelling headphones", Price = 350,
            Category = ["Electronics", "Audio"],
        },
        new()
        {
            Name = "Canon EOS R5", Description = "Micro less camera", Price = 3200,
            Category = ["Electronics", "Camera"],
        },
        new()
        {
            Name = "Levi's Jeans", Description = "Classic denim", Price = 60, Category = ["Fashion"],

        },
        new()
        {
            Name = "LG OLED TV", Description = "Smart 4K TV", Price = 1500, Category = ["Electronics", "TV"],
        },
        new()
        {
            Name = "KitchenAid Mixer", Description = "Stand mixer for baking", Price = 300,
            Category = ["Home", "Appliances"],
        },
        new()
        {
            Name = "Dell XPS 13", Description = "Ultrabook laptop", Price = 1400, Category = ["Electronics", "Laptop"],
        },
    };
}