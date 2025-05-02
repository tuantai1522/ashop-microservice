using Basket.API.Data;
using Basket.API.Errors;
using BuildingBlocks.CQRS;
using BuildingBlocks.Validation;
using Carter;
using FluentValidation;
using MediatR;

namespace Basket.API.Features;

public static class DeleteBasket
{
    public record Command : ICommand<Result<Guid>>
    {
        public Guid Id { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");
        }
    }
    
    internal class Handler(IBasketRepository basketRepository) : ICommandHandler<Command, Result<Guid>>
    {
        private readonly IBasketRepository _basketRepository = basketRepository;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasketById(command.Id, cancellationToken);

            if (shoppingCart == null)
            {
                return Result.Failure<Guid>(ShoppingCartErrors.NotFound(command.Id));
            }

            await _basketRepository.DeleteBasketById(shoppingCart.Id, cancellationToken);
            
            return shoppingCart.Id;
        }
    }
}

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/baskets/{id}", async (Guid id, IMediator mediator) =>
            {
                var command = new DeleteBasket.Command()
                {
                    Id = id,
                };

                var result = await mediator.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .WithName("DeleteBasket")
            .Produces<Guid>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete basket")
            .WithDescription("Delete Basket")
            .WithTags("Baskets");
    }
}