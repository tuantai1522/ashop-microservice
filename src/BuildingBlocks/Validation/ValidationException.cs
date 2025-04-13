namespace BuildingBlocks.Validation;

public class ValidationException(Error[] errors) : Exception("Validation error occurred")
{
    public Error[] Errors { get; } = errors;
}