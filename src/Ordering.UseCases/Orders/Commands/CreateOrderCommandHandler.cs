using BuildingBlocks.Validation;
using MediatR;
using Ordering.Core.Aggregate.CustomerAggregate;
using Ordering.Core.Aggregate.OrderAggregate;
using Ordering.Core.Aggregate.ProductAggregate;

namespace Ordering.UseCases.Orders.Commands;

/// <summary>
/// Handles the creation of a new order.
/// </summary>
/// <param name="orderRepository">The repository for managing order data.</param>
/// <remarks>
/// Implements the <see cref="IRequestHandler{TRequest,TResponse}"/> interface to process the <see cref="CreateOrderCommand"/>.
/// </remarks>
public class CreateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(
            new CustomerId(request.CustomerId),
            Address.Create(request.Street, request.City, request.State, request.Country, request.ZipCode),
            Payment.Create(request.CardName, request.CardNumber, request.Expiration, request.CVV));

        await _orderRepository.AddAsync(order, cancellationToken);
        
        // To add order Items into Order
        _ = request.Items.Select(x =>
        {
            var orderItem = OrderItem.Create(order.Id, new ProductId(x.ProductId), x.Quantity, x.Price);
            
            order.AddOrderItem(orderItem);

            return x;
        }).ToList();
        
        await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order.Id.Value);
    }
}