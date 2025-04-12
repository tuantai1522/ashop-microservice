using BuildingBlocks.CQRS;
using Carter;
using Catalog.API.Contracts;
using Catalog.API.Entities;
using Marten;
using Marten.Pagination;
using MediatR;

namespace Catalog.API.Features;

public static class GetProducts
{
    public record Query(int? PageNumber = 1, int? PageSize = 10) : IQuery<IReadOnlyList<ProductDto>>;

    internal class Handler(IDocumentSession session) : IQueryHandler<Query, IReadOnlyList<ProductDto>>
    {
        private readonly IDocumentSession _session = session;

        public async Task<IReadOnlyList<ProductDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var products = await _session.Query<Product>()
                .OrderBy(p => p.Name)
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

            return products.Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.ImageUrl, p.Category)).ToList();
        }
    }
}

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProducts.Query request, ISender sender) =>
            {
                var result = await sender.Send(request);

                return Results.Ok(result);
            })
            .WithName("GetProducts")
            .Produces<IReadOnlyList<ProductDto>>()
            .WithSummary("Get Products")
            .WithDescription("Get Products")
            .WithTags("Products");

    }
}