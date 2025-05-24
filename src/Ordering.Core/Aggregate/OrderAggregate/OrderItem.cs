using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.ProductAggregate;
using Ordering.Core.Exception;

namespace Ordering.Core.Aggregate.OrderAggregate;

public class OrderItem : Entity
{
    public OrderItemId Id { get; private set; } = OrderItemId.CreateNew();

    public OrderId OrderId { get; private set; }

    public ProductId ProductId { get; private set; }
    
    public int Quantity { get; private set; }

    public decimal Price { get; private set; }

    private OrderItem()
    {

    }

    public static OrderItem Create(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        return new OrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            Price = price,
        };
    }

    public void AddQuantity(int Quantity)
    {
        if (Quantity <= 0)
        {
            throw new OrderingDomainException("Quantity must be greater than 0");
        }
        
        this.Quantity += Quantity;
    }
}