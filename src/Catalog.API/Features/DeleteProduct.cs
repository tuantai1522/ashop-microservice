using BuildingBlocks.CQRS;
using Carter;
using Catalog.API.Entities;
using Marten;
using MediatR;

namespace Catalog.API.Features;

public static class DeleteProduct
{
    public record Command : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
    
    internal class Handler(IDocumentSession session) : ICommandHandler<Command, Guid>
    {
        private readonly IDocumentSession _session = session;

        public async Task<Guid> Handle(Command command, CancellationToken cancellationToken)
        {
            // To find the product by ID
            var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
            
            if (product is null)
            {
                throw new BadHttpRequestException($"Product with {command.Id} not found");
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

                var productId = await mediator.Send(command);

                return Results.Ok(productId);
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