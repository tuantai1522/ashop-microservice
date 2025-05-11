namespace Ordering.Core.Abstraction;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; init; }
    
    public string? CreatedBy { get; set; }
    
    public DateTime? LastModified { get; set; }
    
    public string? LastModifiedBy { get; set; }
}