using BuildingBlocks.CQRS;
using BuildingBlocks.Result;
using Carter;
using Catalog.API.Contracts;
using Catalog.API.Entities;
using Catalog.API.Exceptions;
using Marten;
using MediatR;

namespace Catalog.API.Features;

public static class UpdateProduct
{
    public class Command : ICommand<Result<Guid>>
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string? ImageUrl { get; set; }

        public List<string> Category { get; set; } = [];
    }
    
    public class Handler(IDocumentSession session) : ICommandHandler<Command, Result<Guid>>
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

            // Update product properties
            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageUrl = command.ImageUrl;
            product.Price = command.Price;

            _session.Update(product);
            
            await _session.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id}", async (Guid id, IMediator mediator, UpdateProductRequest request) =>
            {
                var command = new UpdateProduct.Command()
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    ImageUrl = request.ImageUrl,
                    Category = request.Category,
                };

                var result = await mediator.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value)  : result.ToProblemDetails();
            })
            .WithName("UpdateProduct")
            .Produces<Guid>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Update Product")
            .WithDescription("Update Product")
            .WithTags("Products");
    }
}