using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Aggregate.CustomerAggregate;
using Ordering.Core.Aggregate.OrderAggregate;
using Ordering.Core.Event;

namespace Ordering.UseCases.Orders.Events;

/// <summary>
/// To execute something when an order is created.
/// </summary>
/// <param name="logger">
/// Logger to log the information.
/// </param>
/// <param name="orderRepository">
/// Order repository to fetch the order by id.
/// </param>
/// <param name="customerRepository">
/// Customer repository to fetch the customer by id.
/// </param>
public class OrderCreatedEventHandler(
    ILogger<OrderCreatedEventHandler> logger,
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository) : INotificationHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger = logger;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByIdAsync(notification.Id.Value, cancellationToken);

        if (order is null)
        {
            return;
        }
        
        var customer = await _customerRepository.GetCustomerByIdAsync(order.CustomerId.Value, cancellationToken);

        if (customer is null)
        {
            return;
        }
        
        // Todo: Add Email service to send email
        _logger.LogInformation("{Email} has created an order with ID: {OrderId} and total amount: {TotalAmount}.", customer.Email, order.Id.Value, order.GetTotalPrice());
    }
}