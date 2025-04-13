using BuildingBlocks.CQRS;
using BuildingBlocks.Result;
using Carter;
using Catalog.API.Contracts;
using Catalog.API.Entities;
using Marten;
using Marten.Pagination;
using MediatR;

namespace Catalog.API.Features;

public static class GetProductsByCategory
{
    public record Query(string CategoryName, int? PageNumber = 1, int? PageSize = 10) : IQuery<Result<IReadOnlyList<ProductDto>>>;

    internal class Handler(IDocumentSession session) : IQueryHandler<Query, Result<IReadOnlyList<ProductDto>>>
    {
        private readonly IDocumentSession _session = session;

        public async Task<Result<IReadOnlyList<ProductDto>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var products = await _session.Query<Product>()
                .Where(x => x.Category.Contains(query.CategoryName))
                .OrderBy(p => p.Name)
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

            return products.Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.ImageUrl, p.Category)).ToList();
        }
    }
}

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async ([AsParameters] GetProductsByCategory.Query request, ISender sender) =>
            {
                var result = await sender.Send(request);

                return result.IsSuccess ? Results.Ok(result.Value)  : result.ToProblemDetails();
            })
            .WithName("GetProductsByCategory")
            .Produces<IReadOnlyList<ProductDto>>()
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category")
            .WithTags("Products");

    }
}