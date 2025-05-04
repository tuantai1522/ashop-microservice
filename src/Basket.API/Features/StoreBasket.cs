using Basket.API.Contracts;
using Basket.API.Data;
using Basket.API.Entities;
using BuildingBlocks.CQRS;
using BuildingBlocks.Validation;
using Carter;
using Discount.GRPC;
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
    
    internal class Handler(IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountProtoService) : ICommandHandler<Command, Result<Guid>>
    {
        private readonly IBasketRepository _basketRepository = basketRepository;
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService = discountProtoService;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var updatedItems = GetDiscount(command.Items, cancellationToken);
            
            var shoppingCart = new ShoppingCart()
            {
                UserName = command.UserName,
                Items = updatedItems.Adapt<List<ShoppingCartItem>>()
            };

            await _basketRepository.StoreBasket(shoppingCart, cancellationToken);
            
            return shoppingCart.Id;
        }
        
        /// <summary>
        /// To fetch discount from Discount service for each item
        /// </summary>
        /// <param name="shoppingCartItems">
        /// Request from client
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token to cancel the request
        /// </param>
        /// <returns></returns>
        private List<ShoppingCartItemDto> GetDiscount(IReadOnlyList<ShoppingCartItemDto> shoppingCartItems, CancellationToken cancellationToken)
        {
            return shoppingCartItems.Select(item =>
            {
                var response = _discountProtoService.GetDiscountByProductId(new GetDiscountByProductIdRequest
                {
                    ProductId = item.ProductId.ToString()
                }, cancellationToken: cancellationToken);

                var totalRate = response.Coupons.Sum(x => x.Rate);

                var newPrice = item.Price - item.Price * (decimal)totalRate;

                var updatedItem = item with { Price = newPrice };
                
                return updatedItem;
            }).ToList();
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