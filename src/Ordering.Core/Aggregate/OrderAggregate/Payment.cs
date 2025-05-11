namespace Ordering.Core.Aggregate.OrderAggregate;

public record Payment
{
    public string CardName { get; } = null!;
    public string CardNumber { get; } = null!;
    public string? Expiration { get; }
    public string CVV { get; } = null!;

    protected Payment()
    {
    }

    private Payment(string cardName, string cardNumber, string expiration, string cvv)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
    }

    public static Payment Create(string cardName, string cardNumber, string expiration, string cvv)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        return new Payment(cardName, cardNumber, expiration, cvv);
    }
}