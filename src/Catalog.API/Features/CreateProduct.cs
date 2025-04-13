using BuildingBlocks.CQRS;
using BuildingBlocks.Validation;
using Carter;
using Catalog.API.Contracts;
using Catalog.API.Entities;
using FluentValidation;
using Mapster;
using Marten;
using MediatR;

namespace Catalog.API.Features;

public static class CreateProduct
{
    public record Command : ICommand<Result<Guid>>
    {
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string? ImageUrl { get; set; }

        public List<string> Category { get; set; } = [];
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than 0.");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Product category is required.");
        }
    }
    
    internal class Handler(IDocumentSession session) : ICommandHandler<Command, Result<Guid>>
    {
        private readonly IDocumentSession _session = session;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
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

                var result = await mediator.Send(command);

                return result.IsSuccess ? Results.Created($"/products/{result.Value}", result.Value)  : result.ToProblemDetails();
            })
            .WithName("CreateProduct")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product")
            .WithTags("Products");
    }
}