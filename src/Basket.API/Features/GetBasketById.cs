using Basket.API.Contracts;
using Basket.API.Data;
using Basket.API.Errors;
using BuildingBlocks.CQRS;
using BuildingBlocks.Validation;
using Carter;
using FluentValidation;
using MediatR;

namespace Basket.API.Features;

public static class GetBasketById
{
    public record Query(Guid Id) : IQuery<Result<ShoppingCartDto>>;
    
    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Shopping Cart ID is required.");
        }
    }
    
    internal class Handler(IBasketRepository basketRepository) : IQueryHandler<Query, Result<ShoppingCartDto>>
    {
        private readonly IBasketRepository _basketRepository = basketRepository;

        public async Task<Result<ShoppingCartDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasketById(query.Id, cancellationToken);

            if (shoppingCart is null)
            {
                return Result.Failure<ShoppingCartDto>(ShoppingCartErrors.NotFound(query.Id));
            }

            return new ShoppingCartDto(shoppingCart.UserName,
                shoppingCart.Items
                    .Select(i => new ShoppingCartItemDto(i.Color, i.ProductId, i.ProductName, i.Price, i.Quantity))
                    .ToList(),
                shoppingCart.TotalPrice);
        }
    }

}

public class GetBasketByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{id}", async ([AsParameters] GetBasketById.Query request, ISender sender) =>
            {
                var result = await sender.Send(request);

                return result.IsSuccess ? Results.Ok(result.Value)  : result.ToProblemDetails();
            })
            .WithName("GetBasketById")
            .Produces<ShoppingCartDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Basket By Id")
            .WithDescription("Get Basket By Id")
            .WithTags("Baskets");

    }
}