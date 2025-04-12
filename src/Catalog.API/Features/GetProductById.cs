using BuildingBlocks.CQRS;
using Carter;
using Catalog.API.Contracts;
using Catalog.API.Entities;
using Marten;
using Marten.Pagination;
using MediatR;

namespace Catalog.API.Features;

public static class GetProductById
{
    public record Query(Guid Id) : IQuery<ProductDto>;

    internal class Handler(IDocumentSession session) : IQueryHandler<Query, ProductDto>
    {
        private readonly IDocumentSession _session = session;

        public async Task<ProductDto> Handle(Query query, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

            if (product is null)
            {
                throw new BadHttpRequestException($"Product with {query.Id} not found");
            }
            return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.ImageUrl, product.Category);
        }
    }
}

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async ([AsParameters] GetProductById.Query request, ISender sender) =>
            {
                var result = await sender.Send(request);

                return Results.Ok(result);
            })
            .WithName("GetProductById")
            .Produces<ProductDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id")
            .WithTags("Products");

    }
}