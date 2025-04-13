using BuildingBlocks.CQRS;
using BuildingBlocks.Validation;
using Carter;
using Catalog.API.Contracts;
using Catalog.API.Entities;
using Catalog.API.Errors;
using FluentValidation;
using Marten;
using MediatR;

namespace Catalog.API.Features;

public static class GetProductById
{
    public record Query(Guid Id) : IQuery<Result<ProductDto>>;
    
    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required.");
        }
    }

    internal class Handler(IDocumentSession session) : IQueryHandler<Query, Result<ProductDto>>
    {
        private readonly IDocumentSession _session = session;

        public async Task<Result<ProductDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);

            if (product is null)
            {
                return Result.Failure<ProductDto>(ProductErrors.NotFound(query.Id));
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

                return result.IsSuccess ? Results.Ok(result.Value)  : result.ToProblemDetails();
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