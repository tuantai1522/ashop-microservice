using BuildingBlocks.CQRS;
using BuildingBlocks.Validation;
using Carter;
using Catalog.API.Entities;
using Catalog.API.Errors;
using FluentValidation;
using Marten;
using MediatR;

namespace Catalog.API.Features;

public static class DeleteProduct
{
    public record Command : ICommand<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
    
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required.");
        }
    }
    
    internal class Handler(IDocumentSession session) : ICommandHandler<Command, Result<Guid>>
    {
        private readonly IDocumentSession _session = session;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            // To find the product by ID
            var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
            
            if (product is null)
            {
                return Result.Failure<Guid>(ProductErrors.NotFound(command.Id));
            }

            _session.Delete(product);
            
            await _session.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("{id}", async (Guid id, IMediator mediator) =>
            {
                var command = new DeleteProduct.Command()
                {
                    Id = id,
                };

                var result = await mediator.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value)  : result.ToProblemDetails();

            })
            .WithName("DeleteProduct")
            .Produces<Guid>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product")
            .WithTags("Products");
    }
}