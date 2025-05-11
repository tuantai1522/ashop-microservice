namespace Ordering.Core.Aggregate.OrderAggregate;

public record Address
{
    public string Street { get; private set; }
    
    public string City { get; private set; }
    
    public string State { get; private set; }
    
    public string Country { get; private set; }
    
    public string ZipCode { get; private set; }
    
    private Address()
    {
    }

    public static Address Create(string street, string city, string state, string country, string zipCode)
    {
        return new Address()
        {
            Street = street,
            City = city,
            Country = country,
            State = state,
            ZipCode = zipCode,
        };
    }
}