using MediatR;

namespace Ordering.Core.Abstraction;

/// <summary>
/// Type entity is base class for all entities.
/// </summary>
public abstract class Entity : IAuditableEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public string? CreatedBy { get; set; }
    
    public DateTime? LastModified { get; set; }
    
    public string? LastModifiedBy { get; set; }
    
    /// <summary>
    /// Active is status of this item
    /// </summary>
    public bool Active { get; private set; } = true;
    
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}