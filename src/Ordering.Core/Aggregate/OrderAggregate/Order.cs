using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.CustomerAggregate;
using Ordering.Core.Event;

namespace Ordering.Core.Aggregate.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    public OrderId Id { get; init; } = OrderId.CreateNew();

    /// <summary>
    /// Name of order
    /// </summary>
    public string OrderName { get; private set; } = null!;
    
    /// <summary>
    /// Address to ship the order
    /// </summary>
    public Address ShippingAddress { get; private set; } = null!;
    
    /// <summary>
    /// Payment to pay for this order
    /// </summary>
    public Payment Payment { get; private set; } = null!;

    /// <summary>
    /// Status of the order
    /// </summary>
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    
    public CustomerId CustomerId { get; private set; }
    
    /// <summary>
    /// List order items
    /// </summary>
    private readonly List<OrderItem> _orderItems = [];
        
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal GetTotalPrice => OrderItems.Sum(x => x.Price * x.Quantity);

    private Order()
    {
        
    }
    
    public static Order Create(CustomerId customerId, Address shippingAddress, Payment payment)
    {
        var order = new Order
        {
            CustomerId = customerId,
            OrderName = Guid.CreateVersion7().ToString(),
            ShippingAddress = shippingAddress,
            Payment = payment,
            Status = OrderStatus.Pending
        };

        order.RaiseDomainEvent(new OrderCreatedEvent(order));

        return order;
    }
    
    public void Update(Address shippingAddress, Payment payment, OrderStatus status)
    {
        ShippingAddress = shippingAddress;
        Payment = payment;
        Status = status;

        // RaiseDomainEvent(new OrderUpdatedEvent(this));
    }
    
    public void AddOrderItem(OrderItem orderItem)
    {
        var existingOrderForProduct = _orderItems.FirstOrDefault(o => o.ProductId.Value == orderItem.ProductId.Value);

        if (existingOrderForProduct != null)
        {
            existingOrderForProduct.AddQuantity(orderItem.Quantity);
        }
        else
        {
            _orderItems.Add(orderItem);
        }
    }
    
    public void RemoveOrderItem(OrderItem orderItem)
    {
        var existingOrderForProduct = _orderItems.FirstOrDefault(o => o.ProductId.Value == orderItem.ProductId.Value);

        if (existingOrderForProduct != null)
        {
            _orderItems.Remove(existingOrderForProduct);
        }
    }
}