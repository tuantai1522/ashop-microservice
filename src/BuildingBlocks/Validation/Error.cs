namespace BuildingBlocks.Validation;

public record Error
{
    public string Code { get; } = default!;

    public string Description { get; } = default!;

    public ErrorType Type { get; }
    
    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }
    
    protected Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }
    
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);
    
    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error BadRequest(string code, string description) =>
        new(code, description, ErrorType.BadRequest);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);
    
    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);
}