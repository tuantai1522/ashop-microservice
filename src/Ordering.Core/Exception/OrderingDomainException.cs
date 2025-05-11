namespace Ordering.Core.Exception;

/// <summary>
/// To be used for domain exceptions in the Ordering context.
/// </summary>
public class OrderingDomainException : System.Exception
{
    public OrderingDomainException()
    {
    }

    public OrderingDomainException(string message)
        : base(message)
    {
    }

    public OrderingDomainException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}