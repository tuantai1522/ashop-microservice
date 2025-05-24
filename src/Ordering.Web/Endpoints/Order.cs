using BuildingBlocks.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.UseCases.Orders.Commands;

namespace Ordering.Web.Endpoints;

public static class Order
{
    public static IEndpointRouteBuilder MapOrdersApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/orders");

        api.MapPost("/", CreateOrderAsync)
            .WithName("CreateOrder")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Create Order")
            .WithDescription("Create Order")
            .WithTags("Orders");

        return api;
    }

    private static async Task<IResult> CreateOrderAsync(
        [FromServices] IMediator mediator,
        [FromBody] CreateOrderCommand command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.Created($"/orders/{result.Value}", result.Value)
            : result.ToProblemDetails();
    }
}