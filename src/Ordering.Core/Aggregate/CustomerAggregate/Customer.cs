using Ordering.Core.Abstraction;

namespace Ordering.Core.Aggregate.CustomerAggregate;

public class Customer : Entity, IAggregateRoot
{
    public CustomerId Id { get; init; } = CustomerId.CreateNew();

    public string Name { get; private set; } = null!;
    
    public string Email { get; private set; } = null!;

    private Customer()
    {
        
    }

    public static Customer Create(string name, string email)
    {
        return new Customer
        {
            Email = email,
            Name = name,
        };
    }
}