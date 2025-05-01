using Basket.API.Contracts;
using Basket.API.Data;
using Basket.API.Entities;
using BuildingBlocks.CQRS;
using BuildingBlocks.Validation;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;

namespace Basket.API.Features;

public static class StoreBasket
{
    public record Command : ICommand<Result<Guid>>
    {
        public string UserName { get; set; } = null!;
        
        public IReadOnlyList<ShoppingCartItemDto> Items { get; set; } = [];
        
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Basket must have at least one item.");
        }
    }
    
    internal class Handler(IBasketRepository basketRepository) : ICommandHandler<Command, Result<Guid>>
    {
        private readonly IBasketRepository _basketRepository = basketRepository;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var shoppingCart = new ShoppingCart()
            {
                UserName = command.UserName,
                Items = command.Items.Adapt<List<ShoppingCartItem>>()
            };

            await _basketRepository.StoreBasket(shoppingCart, cancellationToken);
            
            return shoppingCart.Id;
        }
    }
}

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/baskets", async (IMediator mediator, StoreBasketRequest request) =>
            {
                var command = request.Adapt<StoreBasket.Command>();

                var result = await mediator.Send(command);

                return result.IsSuccess ? Results.Created($"/baskets/{result.Value}", result.Value)  : result.ToProblemDetails();
            })
            .WithName("StoreBasket")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Store Basket")
            .WithDescription("Store Basket")
            .WithTags("Baskets");
    }
}