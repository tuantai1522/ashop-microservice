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
    public class Command : ICommand<Guid>
    {
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string? ImageUrl { get; set; }

        public List<string> Category { get; set; } = [];
    }
    
    public class Handler(IDocumentSession session) : ICommandHandler<Command, Guid>
    {
        private readonly IDocumentSession _session = session;

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                Category = request.Category
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
        .WithTags("Products");
    }
}