using BuildingBlocks.CQRS;
using Carter;
using Catalog.API.Contracts;
using Catalog.API.Entities;
using Mapster;
using Marten;
using MediatR;

namespace Catalog.API.Features;

public static class CreateProduct
{
    public record Command : ICommand<Guid>
    {
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string? ImageUrl { get; set; }

        public List<string> Category { get; set; } = [];
    }
    
    internal class Handler(IDocumentSession session) : ICommandHandler<Command, Guid>
    {
        private readonly IDocumentSession _session = session;

        public async Task<Guid> Handle(Command command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                ImageUrl = command.ImageUrl,
                Category = command.Category
            };

            _session.Store(product);
            
            await _session.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (IMediator mediator, CreateProductRequest request) =>
            {
                var command = request.Adapt<CreateProduct.Command>();

                var productId = await mediator.Send(command);

                return Results.Created($"/products/{productId}", productId);
            })
            .WithName("CreateProduct")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product")
            .WithTags("Products");
    }
}